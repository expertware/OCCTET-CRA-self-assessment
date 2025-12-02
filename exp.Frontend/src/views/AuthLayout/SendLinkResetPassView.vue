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
        <h1 class="title">Reset password</h1>
        <h6 class="mt-4">Enter your email to reset password. Check inbox for a recovery link. Follow the link to create a new password and regain access to your account securely.</h6>
        <Form @submit="SendLink()" ref="formLogin" :validation-schema="schema" v-slot="{ errors }">
          <div class="mt-4">
            <label for="forgot-pass-email" class="form-label">Email</label>
            <Field
              type="email"
              v-model="user.Email"
              class="form-control"
              :class="{ 'border-danger': errors.email }"
              id="forgot-pass-email"
              placeholder="Email"
              aria-describedby="email"
              name="email"
            ></Field>
            <ErrorMessage name="email" class="error-message"></ErrorMessage>
          </div>
          <div class="text-end mt-4 pointer">
              <router-link class="color-P-400 fw-600" :to="{name: 'login'}" >Login</router-link>
          </div>
          
          <hr class="my-4" />
          <button type="submit" class="btn btn-primary w-100">Send reset link</button>
        </Form>
      </div>
    </div>
  </div>
</template>
<script>
import * as yup from 'yup'
import { Form, Field, ErrorMessage } from 'vee-validate'
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
        Email: null,
        CaptchaToken:null,
      },
    }
  },
  methods: {
    async SendLink() {
      this.$store.commit('SET_LOADER', true)
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'forgotPassword',
      })
      this.user.CaptchaToken = token
      this.$axios
        .post(`/api/Auth/forgot-password`, this.user)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotify('success', 'Check your email!')
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
      })
    },
  },
  created(){
    if(this.$route.params.email)
    this.user.Email=this.$route.params.email
  },
  mounted() {
    this.$loadRecaptchaScript();
  },
}
</script>
<style scoped>
.card {
  border: 1px solid var(--neutral-100);
  width:100%;
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

.logo-width{
  width: 100%;
}
</style>
