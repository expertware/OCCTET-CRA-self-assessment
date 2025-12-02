<template>
  <div class="hero-bg-light">
    <div class="hero-bg">
      <div class="hero-section container-md">
        <div class="row">
          <div class="col-12 col-xl-6 col-xxl p-se-20-px z-index-1">
            <h1 class="hero-section-title">
              OCCTET - Self <br class="d-none d-xxl-block" />
              Assessment Portal
            </h1>
            <h3 class="subtitle">
              Evaluate your Business Cyber Resilience Readinees. Free, instant, and aligned with the
              EU Cyber Resilience Act.
            </h3>
            <div class="row mb-buttons">
              <div class="col-12 col-sm position-relative">
                <input
                  v-model="accessCode"
                  type="text"
                  class="input-opaque w-100"
                  placeholder="Enter your code"
                  aria-label="Your code"
                  aria-describedby="Your code"
                />
                <button
                  type="button"
                  class="btn arrow"
                  :class="{ loading: loginLoading }"
                  @click="LogIn()"
                ></button>
                <h6 class="error-message mt-2 min-height-15-px">
                  <span v-if="errorCode">Your code is not valid</span>
                </h6>
              </div>

              <div class="col-12 col-sm-auto mt-2 mt-md-3">
                <h4
                  class="color-white fw-600 text-center my-auto mb-2 mb-md-0"
                  :class="{ 'mb-3': errorCode }"
                >
                  OR
                </h4>
              </div>
              <div class="col-12 col-sm-auto">
                <button
                  type="button"
                  class="btn btn-light w-100 w-md-auto"
                  @click="$emit('scroll', 'register')"
                >
                  Register
                </button>
              </div>
            </div>
          </div>
          <div class="col-12 col-xl-6 col-xxl-5 z-index-1">
            <div
              class="d-flex wrap justify-content-center justify-content-xl-end align-items-end h-100 gap-40-px mt-logos mt-sm-3 mt-md-0"
            >
              <img src="@/assets/images/EU-logo1.svg" alt="logo-eu" />
              <img src="@/assets/images/EU-logo2.svg" alt="logo-eccc" />
            </div>
          </div>
        </div>
      </div>
      <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
    </div>
  </div>
</template>
<script>
import tokenService from '@/services/token.service'
export default {
  name: '',
  emits: ['scroll'],
  data() {
    return {
      accessCode: null,
      errorCode: false,
      recaptcha: null,
      loginLoading: false,
    }
  },
  mounted() {
    this.$loadRecaptchaScript()
  },

  methods: {
    async LogIn() {
      this.loginLoading = true
      this.errorCode = false

      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'Login',
      })
      this.recaptcha = token

      if (!this.accessCode) {
        this.loginLoading = false
        return
      }
      const surveyId = localStorage.getItem('surveyId')
      const body = {
        AccessCode: this.accessCode.trim(),
        ...(surveyId && { OrganisationSurveyId: surveyId }),
        CaptchaToken: this.recaptcha,
      }

      this.$axios
        .post(`/api/Auth/signInOrganisation`, body)
        .then(async (response) => {
          this.loginLoading = false
          this.errorCode = false

          //set new access and refresh token
          tokenService.setJwtToken(response.data.AccessToken)
          tokenService.setRefreshToken(response.data.RefreshToken)

          this.$router.push({
            name: 'surveys',
          })
        })
        .catch((error) => {
          this.loginLoading = false
          this.errorCode = true
        })
    },
  },
}
</script>
<style scoped>
.hero-section {
  padding-top: 104px;
}
.hero-bg {
  background-image: url('@/assets/images/hero-section-bg-mobile.svg');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: right;
  height: 537px;
  position: relative;
}

.hero-bg-curve {
  position: absolute;
  bottom: -8px;
  width: 100%;
  height: 78px;
}

.hero-bg-light {
  background-image: url('@/assets/images/hero-section-bg-light.png');
  background-size: cover;
  background-repeat: repeat;
}

.hero-section-title {
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
.mb-buttons {
  margin-bottom: 95px;
}

.min-width-400-px {
  min-width: 400px;
}

.min-height-15-px {
  min-height: 0px;
}

@media (min-width: 576px) {
  .hero-bg {
    height: 380px;
  }
}

@media (min-width: 768px) {
  .hero-bg {
    background-image: url('@/assets/images/hero-section-bg.webp');
    height: 356px;
  }
  .min-height-15-px {
    min-height: 15px;
  }
}

@media (min-width: 992px) {
  .hero-section-title {
    font-size: 55px;
    text-transform: uppercase;
  }
  .subtitle {
    color: #ffffff;
    opacity: 80%;
    margin-bottom: 64px;
    font-size: 24px;
  }
  .hero-bg {
    height: 629px;
    background-position: center;
  }
  .mb-buttons {
    margin-bottom: 156px;
  }
  .hero-section {
    padding-top: 208px;
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

  .mb-buttons {
    margin-bottom: 44px;
  }
}

@media (min-width: 1400px) {
  .mb-buttons {
    margin-bottom: 61px;
  }
  .hero-bg-curve {
    bottom: -91px;
    width: 100%;
    height: 342px;
  }
}

@media (min-width: 413px) and (max-width: 575px) {
  .mt-logos {
    margin-top: 40px;
  }
}
</style>
