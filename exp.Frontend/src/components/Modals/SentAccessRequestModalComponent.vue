<template>
  <div
    class="modal fade"
    id="sentAccessRequest"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body text-center">
          <h1 class="title">Organization already exists!</h1>
          <h4 class="mb-2 color-N-700">You can request access from the owner to continue.</h4>
          <button type="button" @click="SentRequest()" class="btn btn-primary mt-4 mx-auto">
            Send request
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: 'SeeAccessRequestModal',
  props: {
    name: '',
    email: '',
    vat: '',
  },
  data() {
    return {
      request: {
        Name: '',
        Email: '',
        Vat: '',
      },
    }
  },
  methods: {
    async SentRequest() {
      this.$store.commit('SET_LOADER', true)
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'login',
      })
      const searchParams = {
        OrganisationVat: this.request.Vat,
        MemberRequestEmail: this.request.Email,
        MemberRequestName: this.request.Name,
        CaptchaToken:token
      }
      this.$axios
        .post(`/api/Organisation/sendEmailToOwner?${new URLSearchParams(searchParams)}`)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          $('#sentAccessRequest').modal('hide')
          this.$utils.ToastNotify('success', 'Access request has been sent')
        })
        .catch(() => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  watch: {
    name(newVal, oldVal) {
      this.request.Name = JSON.parse(JSON.stringify(newVal))
    },
    email(newVal, oldVal) {
      this.request.Email = JSON.parse(JSON.stringify(newVal))
    },
    vat(newVal, oldVal) {
      this.request.Vat = JSON.parse(JSON.stringify(newVal))
    },
  },
  mounted() {
    this.$loadRecaptchaScript();
  },
}
</script>
<style scoped>
.modal-content {
  border: none;
  padding: 40px;
  border-radius: 12px;
}
.modal-header {
  padding: 0px;
  border-bottom: 0px;
}
.modal-body {
  padding: 0px;
}

.title {
  font-size: 30px;
  color: var(--neutral-900);
  font-weight: 600 !important;
  margin-top: 12px;
  margin-bottom: 24px;
  text-align: center;
}

h6 {
  color: var(--neutral-700);
  font-size: 12px;
}
</style>
