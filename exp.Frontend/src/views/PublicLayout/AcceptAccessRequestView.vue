<template>
  <MenuSecondaryPage></MenuSecondaryPage>

  <div class="hero-bg">
    <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
  </div>
  <div class="card">
    <div class="card-body" v-if="isAccepted != true">
      <h1 class="title">Accept request</h1>
      <Form
        ref="requestAccessForm"
        @submit="AcceptRequest()"
        :validation-schema="schema"
        v-slot="{ errors }"
      >
        <label class="form-label">Organisation VAT</label>
        <div class="disabled-text-box">{{ request.organisationVat }}</div>
        <label class="form-label mt-3">You're giving access to:</label>
        <div class="disabled-text-box">{{ request.MemberRequestEmail }}</div>
        <div>
          <label class="form-label mt-3">Access code</label>
          <Field
            type="text"
            v-model="request.AccessCode"
            class="form-control"
            :class="{ 'border-danger': errors.accessCode }"
            id="accept-access-code"
            placeholder="Access code"
            aria-describedby="access code"
            autocomplete="off"
            name="accessCode"
          ></Field>
          <ErrorMessage name="accessCode" class="error-message"></ErrorMessage>
        </div>
        <button type="submit" class="btn btn-primary mt-40-px mx-auto">Accept request</button>
      </Form>
    </div>
    <div class="card-body text-center" v-else>
      <img class="mb-40-px" src="@/assets/images/icon-card-ok.svg" alt="" />
      <h1 class="title mb-3">Request accepted</h1>
      <h4 class="color-N-700">
        <b>{{ request.MemberRequestEmail }}</b> can now access the organization’s surveys.
      </h4>
      <h4 class="color-N-700 mt-4">
        Go to
        <router-link
          :to="{
            name: 'home',
          }"
        >
          home
        </router-link>
      </h4>
    </div>
  </div>
</template>
<script>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
import MenuSecondaryPage from '@/components/Others/MenuSecondaryPageComponent.vue'
export default {
  name: '',
  components: {
    Form,
    Field,
    ErrorMessage,
    MenuSecondaryPage,
  },
  data() {
    return {
      isAccepted: false,
      request: {
        organisationVat: '',
        MemberRequestEmail: '',
        AccessCode: '',
        CaptchaToken:null,
      },
    }
  },
  methods: {
    async AcceptRequest() {
      this.$store.commit('SET_LOADER', true)
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'login',
      })
      this.request.CaptchaToken = token
      this.$axios
        .post(
          `/api/Organisation/acceptMemberRequest?${new URLSearchParams({ ...this.request, AccessCode: this.request.AccessCode.trim() })}`,
        )
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotify('success', 'Access request has been accepted!')
          this.isAccepted = true
        })
        .catch((error) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  async created() {
    if (this.$route.query.organisationVat && this.$route.query.userEmailRequest) {
      this.request.organisationVat = this.$route.query.organisationVat
      this.request.MemberRequestEmail = this.$route.query.userEmailRequest
    }
  },
  mounted() {
    this.$loadRecaptchaScript();
  },
  computed: {
    schema() {
      return yup.object({
        accessCode: yup.string().required('This field is required'),
      })
    },
  },
}
</script>
<style scoped>
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
  font-size: 36px;
  color: var(--neutral-900);
  font-weight: 600 !important;
  margin-top: 12px;
  margin-bottom: 24px;
  text-align: center;
}

/*-------------------------------------*/
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
</style>
