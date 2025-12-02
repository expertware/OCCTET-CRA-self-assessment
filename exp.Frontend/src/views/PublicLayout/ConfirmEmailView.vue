<template>
  <MenuSecondaryPage></MenuSecondaryPage>

  <div class="hero-bg position-relative">
    <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
  </div>

  <div class="card" v-if="isError == false || isError == true">
    <div class="card-body" v-if="isError == false">
      <h1 class="title">Start now</h1>
      <h4 class="mb-40-px text-center">Please log in to gain access to the surveys.</h4>

      <div class="code">
        <span ref="myCode">{{ codeOrganisation }}</span>
        <div class="absolute-elements">
          <img @click="Copy" class="copy" src="@/assets/images/icon-copy.svg" alt="" />
          <img
            @click="GoToSurvey()"
            class="btn copy"
            src="@/assets/images/arrow-confirm-email.svg"
            alt=""
          />
        </div>
      </div>
      <hr class="mb-40-px mt-40-px" />
      <div class="box">
        <h3 class="color-N-900 fw-600 mb-3">Saved to email</h3>
        <h4>
          The code has been saved to email. Please ensure it is kept secure and shared only with
          authorized personnel from the organization.
        </h4>
      </div>
    </div>
    <div v-if="isError == true" class="card-body text-center">
      <h1 class="title">Email already confirmed!</h1>
      <h4 class="mb-40-px text-center">Need the code again? We can send it to you.</h4>
      <button type="button" class="btn btn-primary m-auto" @click="ResendCode()">
        Resend code
      </button>
    </div>
  </div>
</template>
<script>
import MenuSecondaryPage from '@/components/Others/MenuSecondaryPageComponent.vue'
import tokenService from '@/services/token.service'
export default {
  name: '',
  components: {
    MenuSecondaryPage,
  },
  data() {
    return {
      codeOrganisation: null,
      isError: null,
      recaptcha: null,
    }
  },
  methods: {
    Copy() {
      const text = this.$refs.myCode.innerText
      navigator.clipboard
        .writeText(text)
        .then(() => {
          this.$utils.ToastNotify('success', 'Copied to clipboard!')
        })
        .catch((error) => {
          if (error.response) this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    ConfirmEmail(code) {
      this.$axios
        .post(`/api/Organisation/confirmEmail?code=${code}`)
        .then((response) => {
          this.codeOrganisation = response.data
          this.isError = false
        })
        .catch((error) => {
          if ((error.response.data.message == 'Organisation already confirmed!')) {
            this.isError = true
          } else {
            this.$utils.ToastNotifySomethingWentWrong()
          }
        })
    },
    async GoToSurvey() {
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'Login',
      })
      this.recaptcha = token
      if (!this.codeOrganisation) return
      const surveyId = localStorage.getItem('surveyId')
      const body = {
        AccessCode: this.codeOrganisation.trim(),
        ...(surveyId && { OrganisationSurveyId: surveyId }),
        CaptchaToken: this.recaptcha,
      }
      this.$axios
        .post(`/api/Auth/signInOrganisation`, body)
        .then(async (response) => {
          tokenService.setJwtToken(response.data.AccessToken)
          tokenService.setRefreshToken(response.data.RefreshToken)
          this.$router.push({
            name: 'surveys',
          })
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    ResendCode() {
      this.$axios
        .post(`/api/Organisation/resendCode?code=${this.$route.query.code}`)
        .then((response) => {
          this.$utils.ToastNotify('success', 'The code has been resent')
        })
        .catch((error) => {})
    },
  },
  created() {
    if (this.$route.query.code) {
      this.ConfirmEmail(this.$route.query.code)
    }
  },
  mounted() {
    this.$loadRecaptchaScript()
  },
}
</script>
<style scoped>
.btn {
  font-weight: 600;
  transition: all 0.3s;
}
.btn:active {
  border: none;
}
.btn.copy:hover {
  transform: scale(1.2);
}
.card {
  border: 1px solid var(--neutral-100);
  max-width: 630px;
  width: 100%;
  padding: 40px;
  border-radius: 12px;
  z-index: 4;
  position: absolute;
  top: 45%;
  left: 50%;
  transform: translate(-50%, -50%);
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
.copy {
  cursor: pointer;
  transition: all 0.3s;
}
.copy:hover {
  transform: scale(1.2);
}

.code {
  min-height: 64px;
  background-color: var(--neutral-50);
  padding-left: 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-radius: 12px;
  width: 100%;
  color: var(--neutral-900);
  font-size: 18px;
}

.hero-bg {
  background-image: url('@/assets/images/hero-section-bg-mobile.svg');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: right;
  height: 479px;
  position: relative;
}

.hero-bg-curve {
  position: absolute;
  bottom: -20px;
  width: 100%;
  height: 78px;
}

@media (min-width: 768px) {
  .hero-bg {
    background-image: url('@/assets/images/bg-secondary-page.webp');
  }
}

@media (min-width: 992px) {
  .hero-bg {
    height: 629px;
  }
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

.absolute-elements {
  display: flex;
  gap: 16px;
}
</style>
