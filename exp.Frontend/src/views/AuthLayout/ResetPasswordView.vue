<template>
  <div class="hero-bg">
    <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
  </div>
  <div class="text-position p-se-20-px">
    <div class="flex">
      <router-link :to="{ name: 'home' }"
        ><img class="mb-5 pe-5 w-100" src="@/assets/images/logo-occtet-big.svg" alt=""
      /></router-link>
    </div>
    <div class="card">
      <div class="card-body">
        <h1 class="title">Reset password</h1>
        <Form @submit="Save()" ref="formLogin" :validation-schema="schema" v-slot="{ errors }">
          <div class="position-relative mt-4">
            <label for="new-reset-password" class="form-label">New password</label>
            <Field
              :type="isPasswordVisible === true ? 'text' : 'password'"
              v-model="user.Password"
              class="form-control"
              :class="{ 'border-danger': errors.password }"
              id="new-reset-password"
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
          <div class="position-relative mt-4">
            <label for="repeat-reset-password" class="form-label">Repeat password</label>
            <Field
              :type="isPasswordRepeatVisible === true ? 'text' : 'password'"
              v-model="user.ConfirmPassword"
              class="form-control"
              :class="{ 'border-danger': errors.passwordRepeat }"
              id="repeat-reset-password"
              placeholder="Password"
              aria-describedby="password"
              name="passwordRepeat"
            ></Field>
            <ErrorMessage name="passwordRepeat" class="error-message"></ErrorMessage>
            <img
              v-if="isPasswordRepeatVisible == true"
              src="@/assets/images/eye-open.svg"
              alt="visible password"
              @click="ToggleShowPasswordRepeat()"
              class="eye-visibility"
            />
            <img
              v-if="isPasswordRepeatVisible == false"
              src="@/assets/images/eye-closed.svg"
              alt="hidden password"
              @click="ToggleShowPasswordRepeat()"
              class="eye-visibility"
            />
          </div>
          <div class="text-end mt-4 pointer">
            <router-link class="color-P-400 fw-600" :to="{ name: 'forgot-password' }"
              >Login</router-link
            >
          </div>
          <hr class="my-4" />
          <button type="submit" class="btn btn-primary w-100">Reset password</button>
        </Form>
      </div>
    </div>
  </div>
</template>
<script>
import * as yup from 'yup'
import { Form, Field, ErrorMessage } from 'vee-validate'
import tokenService from '@/services/token.service'
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
        ConfirmPassword: null,
        CaptchaToken: null,
      },
      isPasswordVisible: false,
      isPasswordRepeatVisible: false,
    }
  },
  methods: {
    ToggleShowPassword() {
      this.isPasswordVisible = !this.isPasswordVisible
    },
    ToggleShowPasswordRepeat() {
      this.isPasswordRepeatVisible = !this.isPasswordRepeatVisible
    },
    async Save() {
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'resetPassword',
      })

      this.user.CaptchaToken = token
      this.$store.commit('SET_LOADER', true)
      this.$axios
        .post(`/api/Auth/reset-password`, this.user)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.$router.push({ name: 'login' })
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
        password: yup
          .string()
          .required('This field is required')
          .min(12, 'Minimum 12 characters')
          .max(100, 'Maximum 100 characters')
          .test(
            'password-complecity',
            'The password must contain at least one uppercase letter, one special character and one number',
            (password) => {
              const specialCharRegex = /[!@#$%^&*()_+{}[\]:;<>,.?~]/
              const numberRegex = /[0-9]/
              const uppercaseRegex = /[A-Z]/
              return (
                specialCharRegex.test(password) &&
                numberRegex.test(password) &&
                uppercaseRegex.test(password)
              )
            },
          ),
        passwordRepeat: yup
          .string()
          .required('This field is required')
          .oneOf([yup.ref('password'), null], 'The password does not match'),
      })
    },
  },

  created() {
    if (!(this.$route.query.code && this.$route.query.email)) {
      this.$router.push({ name: 'login' })
    }
    this.user.Code = this.$route.query.code
    this.user.Email = this.$route.query.email
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
  z-index: 4;
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
