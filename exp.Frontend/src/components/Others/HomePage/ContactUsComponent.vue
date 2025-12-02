<template>
  <div class="container-lg p-se-20-px">
    <div class="contact-us">
      <Form
        @submit="SendMessage"
        ref="formContactUs"
        :validation-schema="schema"
        v-slot="{ errors }"
      >
        <div class="row">
          <div class="col-12 col-lg-5 col-xl-6 paddings px-5 px-lg-auto">
            <h1 class="title">Contact us</h1>
            <div>
              <label for="contact-us-name" class="form-label">Name</label>
              <Field
                type="text"
                v-model="message.Name"
                class="form-control"
                :class="{ 'border-danger': errors.name }"
                id="contact-us-name"
                placeholder="Name"
                aria-describedby="contact-us-name"
                name="name"
              ></Field>
              <ErrorMessage name="name" class="error-message"></ErrorMessage>
            </div>
            <div class="mt-3">
              <label for="contact-us-email" class="form-label">Email</label>
              <Field
                type="email"
                v-model="message.Email"
                class="form-control"
                :class="{ 'border-danger': errors.email }"
                id="contact-us-email"
                placeholder="Email"
                aria-describedby="email"
                name="email"
              ></Field>
              <ErrorMessage name="email" class="error-message"></ErrorMessage>
            </div>
            <div class="mt-3">
              <label for="contact-us-message" class="form-label">Message</label>
              <Field
                as="textarea"
                rows="3"
                v-model="message.Message"
                class="form-control"
                :class="{ 'border-danger': errors.message }"
                id="contact-us-message"
                placeholder="Message"
                aria-describedby="message"
                name="message"
              ></Field>
              <ErrorMessage name="message" class="error-message"></ErrorMessage>
            </div>

            <button class="btn btn-secondary mt-4" type="submit">Send</button>
          </div>
          <div class="col-12 col-lg-7 col-xl-6 bg-blue">
            <img class="figure-1" src="@/assets/images/contact-us-figure-1.png" alt="" />
            <img class="figure-2" src="@/assets/images/contact-us-figure-2.png" alt="" />
            <div class="get-in-touch">
              <h2 class="color-white fw-600 roboto-cond mb-40-px">Get in touch</h2>

              <a class="source" rel="noopener noreferrer" href="mailto:mailto:occtet-eu@eclipse.org"
                ><img src="@/assets/images/contact-icon-email.svg" alt="email" />
                <h4>occtet-eu@eclipse.org</h4></a
              >

              <a
                class="source"
                rel="noopener noreferrer"
                target="_blank"
                href="https://www.youtube.com/@occtetproject"
                ><img src="@/assets/images/contact-icon-yt.svg" alt="youtube" />
                <h4>Youtube account</h4></a
              >

              <a
                class="source"
                rel="noopener noreferrer"
                target="_blank"
                href="https://www.linkedin.com/company/open-cybersecurity-compliance-toolkit-occtet"
                ><img src="@/assets/images/contact-icon-linkedin.svg" alt="linkedin" />
                <h4>Linkedin account</h4></a
              >

              <a
                class="source"
                rel="noopener noreferrer"
                target="_blank"
                href="https://bsky.app/profile/occtetproject.bsky.social"
                ><img src="@/assets/images/contact-icon-bluesky.svg" alt="bluesky" />
                <h4>Bluesky account</h4></a
              >
            </div>
          </div>
        </div>
      </Form>
    </div>
  </div>
</template>
<script>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
export default {
  name: '',
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  data() {
    return {
      message: {
        Name: '',
        Email: '',
        Message: '',
        CaptchaToken: null,
      },
    }
  },
  methods: {
    async SendMessage() {
      this.$store.commit('SET_LOADER', true)
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'constactUs',
      })
      this.message.CaptchaToken = token
      this.$axios
        .post(`/api/Organisation/contactUs`, this.message)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotify('success', 'Your message has been sent!')
          this.$refs.formContactUs.resetForm()
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
        name: yup
          .string()
          .required('This field is required')
          .min(3, 'Minimum 3 characters')
          .max(500, 'Maximum 500 characters'),
        email: yup
          .string()
          .required('This field is required')
          .email('Invalid address')
          .min(3, 'Minimum 3 characters')
          .max(100, 'Maximum 100 characters'),
        message: yup
          .string()
          .required('This field is required')
          .min(3, 'Minimum 3 characters')
          .max(1000, 'Maximum 1000 characters'),
      })
    },
  },
  mounted() {
    this.$loadRecaptchaScript()
  },
}
</script>
<style scoped>
.contact-us {
  margin-top: 95px;
  background-color: #ffffff;
  border-radius: 24px;
  box-shadow: 0px 12px 64px 0px #64879e29;
  overflow: hidden;
}

.title {
  font-family: 'Roboto Condensed';
  font-size: 36px;
  font-weight: 600 !important;
  color: var(--neutral-900);
  margin-bottom: 35px;
}

.bg-blue {
  background-image: url('@/assets/images/bg-contact-responsive.webp');
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  background-repeat: no-repeat;
  background-size: cover;
  min-height: 600px;
}

.paddings {
  padding: 40px 0px 40px 40px;
}

.figure-1 {
  display: none;
}
.figure-2 {
  position: absolute;
  bottom: 0px;
  right: 0px;
}

.get-in-touch {
  display: flex;
  flex-direction: column;
}
.get-in-touch .source {
  display: flex;
  align-items: anchor-center;
  gap: 8px;
  color: #ffffff;
  margin-bottom: 10px;
}
.source a {
  color: #ffffff;
}

.source h4 {
  text-transform: uppercase;
}
.source a:hover {
  color: #dddddd;
}

@media(min-width:768px){
  .contact-us {
  margin-top: 120px;
}
}

@media (min-width: 992px) {
  .bg-blue {
    background-image: url('@/assets/images/bg-contact.webp');
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    background-repeat: no-repeat;
    background-size: cover;
  }
  .figure-1 {
    display: block;
    position: absolute;
    top: 0px;
    right: 0px;
  }
}
@media (min-width: 1200px) {
  .title {
    font-size: 48px;
    margin-bottom: 56px;
  }
}
</style>
