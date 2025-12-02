<template>
  <div class="hero-bg">
    <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
  </div>
  <div class="text-position p-se-20-px">
    <div class="flex">
      <router-link :to="{ name: 'home' }"
        ><img class="mb-5 pe-5 logo-width" src="@/assets/images/logo-occtet-big.svg" alt=""
      /></router-link>
    </div>
    <div class="card">
      <div class="card-body">
        <h2 v-if="isLoginSent" class="text-center">
          <img src="@/assets/images/info-icon-card.svg" alt="" />
        </h2>
        <h1 class="title">{{ isLoginSent ? 'Two Step Authentication ' : 'Login' }}</h1>
        <div v-if="!isLoginSent">
          <Form @submit="Login()" ref="formLogin" :validation-schema="schema" v-slot="{ errors }">
            <div class="mt-4">
              <label for="login-email" class="form-label">Email</label>
              <Field
                type="email"
                v-model="user.Username"
                class="form-control"
                :class="{ 'border-danger': errors.email }"
                id="login-email"
                placeholder="Email"
                aria-describedby="email"
                name="email"
              ></Field>
              <ErrorMessage name="email" class="error-message"></ErrorMessage>
            </div>

            <div class="position-relative mt-3">
              <label for="login-password" class="form-label">Password</label>
              <Field
                :type="isPasswordVisible === true ? 'text' : 'password'"
                v-model="user.Password"
                class="form-control"
                :class="{ 'border-danger': errors.password }"
                id="login-password"
                placeholder="Password"
                aria-describedby="password"
                name="password"
              ></Field>
              <ErrorMessage name="password" class="error-message"></ErrorMessage>
              <img
                v-if="isPasswordVisible == true"
                src="@/assets/images/eye-open.svg"
                alt="visible password"
                @click="ToggleShowPassword()"
                class="eye-visibility"
              />
              <img
                v-if="isPasswordVisible == false"
                src="@/assets/images/eye-closed.svg"
                alt="hidden password"
                @click="ToggleShowPassword()"
                class="eye-visibility"
              />
            </div>
            <div class="text-end mt-4 pointer">
              <router-link
                class="color-P-400 fw-600"
                :to="{ name: 'forgot-password', params: { email: user.Username } }"
                >Forgot password</router-link
              >
            </div>
            <hr class="my-4" />
            <button type="submit" class="btn btn-primary w-100">Login</button>
          </Form>
        </div>
        <div v-else>
          <h6 class="mb-3 mt-4">
            To complete the login process, a second verification step is needed. Please check your
            email and enter the code we sent you below.

            <Form
              @submit="CheckCode()"
              ref="checkCode"
              :validation-schema="schema2"
              v-slot="{ errors }"
            >
              <div class="mt-4">
                <Field
                  type="text"
                  v-model="code"
                  class="form-control"
                  :class="{ 'border-danger': errors.code }"
                  id="code-auth"
                  placeholder="Code"
                  aria-describedby="Code"
                  name="code"
                ></Field>
                <ErrorMessage name="code" class="error-message"></ErrorMessage>
              </div>
              <hr class="my-4" />
              <button type="submit" class="btn btn-primary w-100">Login</button>
            </Form>
          </h6>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import * as yup from 'yup'
import { Form, Field, ErrorMessage } from 'vee-validate'
import tokenService from '@/services/token.service'
import menuAdmin from '../../utils/menuAdmin.json'
export default {
  name: '',
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  data() {
    return {
      user: {
        Username: null,
        Password: null,
        CaptchaToken: null,
      },
      isPasswordVisible: false,
      isLoginSent: false,
      provider: null,
      code: null,
      menuAdmin: menuAdmin,
    }
  },
  methods: {
    ToggleShowPassword() {
      this.isPasswordVisible = !this.isPasswordVisible
    },
    async SelectPageByRole() {
      await this.$store.dispatch('getLoggedUser')
      const user = this.$store.getters.getUser
      const userRoles = user.UserRoles || []
      const redirectPages = menuAdmin.filter((item) => item.PageRedirect == true)
      const matchingPage = redirectPages.find((page) =>
        page.Roles.some((role) => userRoles.includes(role)),
      )
      this.$router.push({ name: matchingPage.Link })
    },
    async Login() {
      this.$store.commit('SET_LOADER', true)

      // ---------------------
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'login',
      })

      this.user.CaptchaToken = token
      this.$axios
        .post(`/api/Auth/login`, this.user)
        .then((response) => {
          if (response.data.Is2FactorRequired) {
            this.$accessCode.removeData()
            tokenService.removeAuthTokens()
            this.provider = response.data.Provider
            this.$store.commit('SET_LOADER', false)
            this.isLoginSent = true
          } else {
            this.$accessCode.removeData()
            tokenService.setJwtToken(response.data.AccessToken)
            tokenService.setRefreshToken(response.data.RefreshToken)
            this.$store.commit('SET_LOADER', false)
            this.SelectPageByRole()
          }
        })
        .catch((error) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })

      //-------------------------
    },
    async CheckCode() {
      this.$store.commit('SET_LOADER', true)
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'login',
      })
      this.user.CaptchaToken = token
      const obj = {
        Email: this.user.Username,
        Token: this.code,
        Provider: this.provider,
        CaptchaToken:this.user.CaptchaToken
      }
      this.$axios
        .post(`/api/Auth/validateOTPFor2Factor`, obj)
        .then((response) => {
          this.$accessCode.removeData()
          tokenService.setJwtToken(response.data.AccessToken)
          tokenService.setRefreshToken(response.data.RefreshToken)
          this.$store.commit('SET_LOADER', false)
          this.SelectPageByRole()
        })
        .catch((error) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  computed: {
    schema() {
      return yup.object({
        email: yup
          .string()
          .required('This field is required')
          .email('Invalid address')
          .min(3, 'Minimum 3 characters')
          .max(100, 'Maximum 100 characters'),
        password: yup.string().required('This field is required'),
      })
    },
    schema2() {
      return yup.object({
        code: yup
          .string()
          .required('This field is required')
          .min(3, 'Minimum 3 characters')
          .max(100, 'Maximum 100 characters'),
      })
    },
  },
  mounted() {
    this.$loadRecaptchaScript()
  },
}
</script>
<style scoped>
.card {
  border: 1px solid var(--neutral-100);
  width: 100%;
  max-width: 460px;
  padding: 40px;
  border-radius: 12px;
  box-shadow: 0px 12px 64px 0px #64879e29;
}
.card-body {
  padding: 0px;
}

.title {
  text-align: center;
  font-size: 36px;
  color: var(--neutral-900);
  font-weight: 600 !important;
  margin-top: 12px;
  margin-bottom: 16px;
}

.hero-bg {
  background-image: url('@/assets/images/hero-section-bg-mobile.svg');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: right;
  height: 479px;
  position: relative;
  width: 100%;
  z-index: 1;
  position: fixed;
}

.hero-bg-curve {
  position: absolute;
  bottom: -20px;
  width: 100%;
  height: 78px;
}

.section-title {
  font-size: 36px;
  color: #ffffff;
  line-height: 110.00000000000001%;
  margin-bottom: 22px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
}

.subtitle {
  color: #ffffff;
  opacity: 80%;
  margin-bottom: 40px;
  font-size: 15px;
}

@media (min-width: 768px) {
  .hero-bg {
    background-image: url('@/assets/images/bg-secondary-page.webp');
  }
}

@media (min-width: 992px) {
  .section-title {
    font-size: 60px;
    text-transform: uppercase;
  }
  .subtitle {
    color: #ffffff;
    opacity: 80%;
    margin-bottom: 64px;
    font-size: 28px;
  }
  .hero-bg {
    height: 629px;
  }
}
.text-position {
  margin-top: 120px;
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  position: relative;
  z-index: 2;
}

@media (min-width: 1200px) {
  .hero-bg {
    height: 658px;
  }
  .hero-bg-curve {
    bottom: -91px;
    width: 100%;
    height: 271px;
  }
}

@media (min-width: 1400px) {
  .hero-bg-curve {
    bottom: -36px;
    width: 100%;
    height: 342px;
  }
}

.eye-visibility {
  position: absolute;
  top: 41px;
  right: 15px;
  cursor: pointer;
}

.logo-width {
  width: 100%;
}
</style>
