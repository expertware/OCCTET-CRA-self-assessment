<template>
  <div class="container-lg p-se-20-px">
    <div class="register-company-section">
      <div class="row mx-0">
        <div class="col-12 col-lg-4">
          <div class="add-code-card">
            <h2 class="roboto-cond fw-600 mb-40-px mt-5 mt-lg-0">
              {{ forgetCode ? 'Reset your code' : 'Enter your code' }}
            </h2>
            <h4 class="white-opc-75 mb-40-px">
              {{
                forgetCode
                  ? 'Enter your email address to reset your code'
                  : 'To access all surveys, you need an access code received after registration.'
              }}
            </h4>
            <div class="position-relative mb-2 mb-md-0">
              <input
                v-if="!forgetCode"
                v-model="accessCode"
                type="text"
                class="input-opaque w-100"
                placeholder="Your code"
                aria-label="Your code"
                aria-describedby="Your code"
              />

              <Form
                v-else
                @submit="ResetCode"
                ref="formReset"
                :validation-schema="schema2"
                v-slot="{ errors }"
              >
                <Field
                  v-model="emailForResetCode"
                  type="text"
                  class="input-opaque w-100"
                  placeholder="Enter email"
                  aria-label="Enter email"
                  aria-describedby="Enter email"
                  autocomplete="off"
                  name="resetEmail"
                ></Field>

                <button type="submit" class="btn arrow" @click="ResetCode()"></button>
                <ErrorMessage name="resetEmail" class="color-white mt-3"></ErrorMessage>
              </Form>
              <button
                v-if="!forgetCode"
                type="button"
                class="btn arrow"
                :class="{ loading: loginLoading }"
                @click="LogIn()"
              ></button>
            </div>
            <h6 class="color-white mt-2" v-if="errorCode">Your code is not valid !</h6>
            
          </div>
        </div>
        <div class="col-12 col-lg-auto padding-es-28px">
          <div class="separator-cols d-none d-lg-flex">
            <img src="@/assets/images/register-bar-1.png" alt="" />
            <h6 class="fw-700">OR</h6>
            <img src="@/assets/images/register-bar-2.png" alt="" />
          </div>
          <div class="separator-cols d-none d-sm-flex d-lg-none">
            <img src="@/assets/images/register-bar-mobile.png" alt="" />
            <h6 class="fw-700">OR</h6>
            <img src="@/assets/images/register-bar-mobile.png" alt="" />
          </div>
          <div class="flex my-4 color-N-900 d-sm-none">
            <h6 class="fw-700">OR</h6>
          </div>
        </div>
        <div class="col-12 col-lg px-4 px-lg-0 pe-lg-5">
          <div class="register-form">
            <h2 class="title-form">Register your company</h2>
            <Form
              @submit="Register"
              ref="formRegister"
              :validation-schema="schema"
              v-slot="{ errors }"
            >
              <div class="row">
                <div class="col-12 col-sm-6 col-xl-8">
                  <label for="company-name" class="form-label">Company name</label>
                  <Field
                    type="text"
                    v-model="request.Name"
                    class="form-control"
                    :class="{ 'border-danger': errors.companyName }"
                    id="company-name"
                    placeholder="Company name"
                    aria-describedby="company-name"
                    autocomplete="off"
                    name="companyName"
                  ></Field>
                  <ErrorMessage name="companyName" class="error-message"></ErrorMessage>
                </div>
                <div class="col-12 col-sm-6 col-xl-4 mt-3 mt-sm-0 position-relative">
                  <div class="position-relative" ref="popupWrapper">
                    <div class="flex-align-center">
                      <label for="Vat" class="form-label flex-align-center position-relative"
                        >Company Number / VAT
                      </label>
                      <img
                        ref="popupTrigger"
                        @click.stop="OpenPopup"
                        width="16"
                        class="ms-2 pointer mb-2"
                        src="@/assets/images/icon-info.png"
                        alt=""
                      />
                    </div>

                    <div
                      v-if="popupVatDetails"
                      class="custom-tooltip w-100"
                      ref="popupContent"
                      v-html="popupContent"
                    ></div>
                  </div>

                  <Field
                    type="text"
                    v-model="request.Vat"
                    class="form-control"
                    :class="{ 'border-danger': errors.vat }"
                    id="Vat"
                    placeholder="VAT"
                    aria-describedby="vat"
                    name="vat"
                  ></Field>
                  <ErrorMessage name="vat" class="error-message"></ErrorMessage>
                </div>
              </div>
              <div class="row mt-3">
                <div class="col-12 col-sm-6">
                  <label for="domain" class="form-label">Website URL</label>
                  <Field
                    type="text"
                    v-model="request.Domain"
                    class="form-control"
                    :class="{ 'border-danger': errors.domain }"
                    id="domain"
                    placeholder="Domain"
                    aria-describedby="domain"
                    name="domain"
                  ></Field>
                  <ErrorMessage name="domain" class="error-message"></ErrorMessage>
                </div>
                <div class="col-12 col-sm-6 mt-3 mt-sm-0">
                  <label class="form-label">Country</label>
                  <Field name="country" v-model="request.Country">
                    <multiselect
                      class="custom-multiselect"
                      :class="{ 'field-error': errors.country }"
                      v-model="request.Country"
                      :options="countries"
                      placeholder="Country"
                      selected-label=""
                      deselect-label=""
                      select-label=""
                      label="Name"
                      track-by="Id"
                      :loading="loaders.loaderMultiselectCountries"
                      @open="GetCountries()"
                    ></multiselect
                  ></Field>
                  <ErrorMessage name="country" class="error-message"></ErrorMessage>
                </div>
              </div>
              <div class="row mt-3">
                <div class="col-12 col-sm-6">
                  <label class="form-label">Company size</label>
                  <Field name="companySize" v-model="request.CompanySize">
                    <multiselect
                      class="custom-multiselect"
                      :class="{ 'field-error': errors.companySize }"
                      v-model="request.CompanySize"
                      :options="companySizes"
                      placeholder="Company size"
                      selected-label=""
                      deselect-label=""
                      select-label=""
                    ></multiselect
                  ></Field>
                  <ErrorMessage name="companySize" class="error-message"></ErrorMessage>
                </div>
                <div class="col-12 col-sm-6 mt-3 mt-sm-0">
                  <label class="form-label">Business sectors</label
                  ><span class="color-grey fs-12px ms-2"
                    ><i
                      >(Check your sector
                      <router-link
                        target="_blank"
                        :to="{
                          name: 'domains',
                        }"
                      >
                        here
                      </router-link>
                      )</i
                    ></span
                  >
                  <!-- {{ request.ActivitySectors }} -->
                  <Field name="activitySectors" v-model="request.ActivitySectors">
                    <multiselect
                      class="custom-multiselect"
                      :class="{ 'field-error': errors.activitySectors }"
                      v-model="request.ActivitySectors"
                      :options="activitySectors"
                      placeholder="Select sectors"
                      selected-label=""
                      deselect-label=""
                      select-label=""
                      label="Name"
                      track-by="Id"
                      :multiple="true"
                      :loading="loaders.loaderMultiselectActivitySectors"
                      @open="GetActivitySectors()"
                    ></multiselect
                  ></Field>
                  <ErrorMessage name="activitySectors" class="error-message"></ErrorMessage>
                </div>
              </div>

              <div class="col-12 mt-3">
                <label class="form-label">Select countries where you sale</label>
                <Field name="deliveryCountries" v-model="request.DeliveryCountries">
                  <multiselect
                    class="custom-multiselect"
                    :class="{ 'field-error': errors.deliveryCountries }"
                    v-model="request.DeliveryCountries"
                    :options="deliveryCountries"
                    placeholder="Countries"
                    selected-label=""
                    deselect-label=""
                    select-label=""
                    :multiple="true"
                    label="Name"
                    track-by="Id"
                    :loading="loaders.loaderMultiselectDeliveryCountries"
                    @open="GetDeliveryCountries"
                  ></multiselect
                ></Field>
                <ErrorMessage name="deliveryCountries" class="error-message"></ErrorMessage>
                <h6 class="color-grey fs-12px mt-2 ms-2">
                  <i>*If you don’t sell in European countries, the CRA does not apply to you.</i>
                </h6>
              </div>

              <div class="separator"></div>

              <div class="row mb-2">
                <div class="col-12 col-md mt-3 mt-lg-0">
                  <label for="company-contact-person" class="form-label">Contact person</label>
                  <Field
                    type="text"
                    v-model="request.ContactName"
                    class="form-control"
                    :class="{ 'border-danger': errors.contactPerson }"
                    id="company-contact-person"
                    placeholder="Name"
                    aria-describedby="company-contact-person"
                    name="contactPerson"
                  ></Field>
                  <ErrorMessage name="contactPerson" class="error-message"></ErrorMessage>
                </div>
                <div class="col-12 col-md mt-3 mt-lg-0">
                  <label for="company-email" class="form-label">Contact email</label>
                  <Field
                    type="text"
                    v-model="request.ContactEmail"
                    class="form-control"
                    :class="{ 'border-danger': errors.contactEmail }"
                    id="company-email"
                    placeholder="Email"
                    aria-describedby="contact-email"
                    name="contactEmail"
                  ></Field>
                  <ErrorMessage name="contactEmail" class="error-message"></ErrorMessage>
                </div>
                <div class="col-12 col-md mt-3 mt-lg-0">
                  <label for="company-contact-role" class="form-label">Role</label>
                  <Field
                    type="text"
                    v-model="request.ContactPersonRole"
                    class="form-control"
                    :class="{ 'border-danger': errors.contactRole }"
                    id="company-contact-role"
                    placeholder="Role"
                    aria-describedby="company-name"
                    name="contactRole"
                  ></Field>
                  <ErrorMessage name="contactRole" class="error-message"></ErrorMessage>
                </div>
              </div>
              <h6 class="color-grey fs-12px">
                <i>*You must be the authorized legal representative of your organization.</i>
              </h6>
              <div class="form-check mt-3 mt-md-2">
                <Field
                  v-slot="{ field }"
                  name="agreement"
                  id="agreement-box"
                  v-model="request.Agreement"
                >
                  <input
                    v-bind="field"
                    v-model="request.Agreement"
                    class="form-check-input pointer"
                    type="checkbox"
                    id="flexCheckDefault"
                /></Field>

                <label class="form-check-label pointer" for="flexCheckDefault">
                  I confirm that I am the legally authorized representative of my organization and
                  consent to the processing of my personal data in accordance with the
                  <router-link target="_blank" :to="{ name: 'gdpr' }"
                    >Privacy Policy and GDPR</router-link
                  >
                  requirements, for the purpose of managing my company’s CRA compliance
                  assessments.</label
                >
              </div>
              <ErrorMessage name="agreement" class="error-message"></ErrorMessage>
              <div class="d-flex justify-content-center justify-content-lg-end mb-lg-4 mt-40-px">
                <button class="btn btn-secondary" type="submit">Register</button>
              </div>
            </Form>
          </div>
          
        </div>
      </div>
    </div>
  </div>

  <SentAccessRequestModalComponent
    :name="request.ContactName"
    :email="request.ContactEmail"
    :vat="request.Vat"
  ></SentAccessRequestModalComponent>
</template>
<script>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
import SentAccessRequestModalComponent from '@/components/Modals/SentAccessRequestModalComponent.vue'
import tokenService from '@/services/token.service'
export default {
  name: '',
  components: {
    Form,
    Field,
    ErrorMessage,
    SentAccessRequestModalComponent,
  },
  data() {
    return {
      loginLoading: false,
      loaders: {},
      countries: [],
      companySizes: ['Micro (1–9)', 'Small (10–49)', 'Medium (50–249)', 'Large (250+)'],
      activitySectors: [],
      deliveryCountries: [],
      request: {
        Name: '',
        Vat: null,
        Domain: null,
        ActivitySectors: [],
        ContactName: '',
        ContactEmail: '',
        ContactPersonRole: null,
        CompanySize: null,
        Country: null,
        OrganisationSurveyId: null,
        DeliveryCountries: [],
        CaptchaToken: null,
        Agreement: false,
      },
      errorRegister: null,
      accessCode: '',
      errorCode: false,
      forgetCode: false,
      emailForResetCode: null,
      loginRecaptcha: null,
      popupVatDetails: false,
      popupContent: `
      Please provide your Company Identification Number:
        <ul>
          <li>If your company has a VAT number, enter it here.</li>
          <li>If not, you may enter your EUID (European Unique Identifier) instead.</li>
        </ul>
      `,
    }
  },
  methods: {
    OpenPopup(popupValue) {
      this.popupVatDetails = !this.popupVatDetails
    },
    async LogIn() {
      this.loginLoading = true
      this.errorCode = false
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'ContactUs',
      })
      this.loginRecaptcha = token
      if (!this.accessCode) {
        this.loginLoading = false
        return
      }
      const surveyId = localStorage.getItem('surveyId')
      const body = {
        AccessCode: this.accessCode.trim(),
        ...(surveyId && { OrganisationSurveyId: surveyId }),
        CaptchaToken: this.loginRecaptcha,
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
    async Register() {
      this.$store.commit('SET_LOADER', true)
      await new Promise((resolve) => grecaptcha.ready(resolve))
      const token = await grecaptcha.execute(import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC, {
        action: 'registerOrganisation',
      })
      this.request.CaptchaToken = token
      const request = {
        ...this.request,
        Domain:
          this.request.Domain?.startsWith('http://') || this.request.Domain?.startsWith('https://')
            ? this.request.Domain
            : `https://${this.request.Domain}`,
        CountryId: this.request.Country.Id,
        DeliveryCountries: this.request.DeliveryCountries.map((item) => item.Id),
        ActivitySectors: this.request.ActivitySectors.map((item) => item.Id),
      }
      this.$axios
        .post(`/api/Account/registerOrganisation`, request)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          if (response.data == 'ConfirmEmail') {
            this.$swal.fire({
              icon: 'success',
              title: 'Register request has been sent!',
              text: 'Check your email!',
              showCancelButton: false,
            })
          }

          if (response.data == 'Pending') {
            this.$swal.fire({
              icon: 'success',
              title: 'Register request has been sent!',
              text: ' Wait for administrator confirmation!',
              showCancelButton: false,
            })
          }

          this.$refs.formRegister.resetForm()
        })
        .catch((error) => {
          this.$store.commit('SET_LOADER', false)
          if (error.response.data.message == 'Organisation already exists!') {
            $('#sentAccessRequest').modal('show')
          } else {
            this.$utils.ToastNotifySomethingWentWrong()
          }
        })
    },
    GetActivitySectors() {
      this.activitySectors = []
      this.loaders.loaderMultiselectActivitySectors = true
      this.$axios
        .get(`/api/Organisation/getActivitySectors`)
        .then((response) => {
          this.loaders.loaderMultiselectActivitySectors = false
          this.activitySectors = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectActivitySectors = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetCountries() {
      this.countries = []
      this.loaders.loaderMultiselectCountries = true
      this.$axios
        .get(`/api/Organisation/getCountries`)
        .then((response) => {
          this.loaders.loaderMultiselectCountries = false
          this.countries = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectCountries = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetDeliveryCountries() {
      this.deliveryCountries = []
      this.loaders.loaderMultiselectDeliveryCountries = true
      this.$axios
        .get(`/api/Organisation/getDeliveryCountries`)
        .then((response) => {
          this.loaders.loaderMultiselectDeliveryCountries = false
          this.deliveryCountries = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectDeliveryCountries = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    HandleClickOutsidePopup(event) {
      const wrapper = this.$refs.popupWrapper
      if (this.popupVatDetails && wrapper && !wrapper.contains(event.target)) {
        this.popupVatDetails = false
      }
    },
  },
  computed: {
    schema() {
      return yup.object({
        companyName: yup
          .string()
          .required('This field is required')
          .min(3, 'Minimum 3 characters')
          .max(500, 'Maximum 500 characters'),
        agreement: yup.boolean().oneOf([true], 'You must agree to the Terms and Conditions.'),
        vat: yup.string().required('This field is required'),
       
        domain: yup
          .string()
          .nullable()
          .trim()
          .transform((currentValue) => {
            if (!currentValue) return ''

            const doesNotStartWithHttp = !(
              currentValue.startsWith('http://') || currentValue.startsWith('https://')
            )

            if (doesNotStartWithHttp) {
              return `http://${currentValue}`
            }
            return currentValue
          })
          .required('This field is required')
          .url('Invalid URL'),
        country: yup.object().required('This field is required'),
        companySize: yup.string().required('This field is required'),
        activitySectors: yup
          .array()
          .min(1, 'Select at least one sector')
          .required('This field is required'),
        deliveryCountries: yup.array().min(1, 'Select a country').required('Select a country'),
        contactPerson: yup
          .string()
          .required('This field is required')
          .min(3, 'Minimum 3 characters')
          .max(500, 'Maximum 500 characters'),
        contactEmail: yup
          .string()
          .required('This field is required')
          .email('Address is not correct')
          .max(500, 'Maximum 500 characters'),
        contactRole: yup
          .string()
          .required('This field is required')
          .min(3, 'Minimum 3 characters')
          .max(200, 'Maximum 200 characters'),
      })
    },
    schema2() {
      return yup.object({
        resetEmail: yup
          .string()
          .required('This field is required')
          .email('Address is not correct')
          .max(500, 'Maximum 500 characters'),
      })
    },
  },

  mounted() {
    this.$loadRecaptchaScript()
    document.addEventListener('click', this.HandleClickOutsidePopup)
  },
  beforeUnmount() {
    document.removeEventListener('click', this.HandleClickOutsidePopup)
  },
}
</script>
<style scoped>
.register-company-section {
  padding: 12px 0px;
  margin-top: 80px;
  background-color: #ffffff;
  min-height: 639px;
  box-shadow: 0px 12px 64px 0px #64879e29;
  border-radius: 24px;
}
.add-code-card {
  background-image: url('@/assets/images/join-bg.png');
  background-repeat: no-repeat;
  background-size: cover;
  height: 100%;
  padding: 24px 40px;
  color: #ffffff;
  border-radius: 16px;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
.register-form {
  min-height: 623px;
  padding-top: 0px;
}

.title-form {
  color: var(--neutral-900);
  font-size: 24px;
  margin-bottom: 24px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
}
.radius-16-px {
  border-radius: 16px;
}
.separator-cols img {
  width: fit-content;
  height: 100%;
}
.separator-cols {
  padding: 32px 0px;
  display: flex;
  color: var(--neutral-900);
  align-items: center;
  justify-content: space-between;
  gap: 40px;
}

.padding-es-28px {
  padding-left: 28px;
  padding-right: 28px;
}

.separator {
  height: 1px;
  width: 100%;
  background-color: var(--neutral-200);
  margin: 24px 0px;
}

@media (min-width: 992px) {
  .title-form {
    font-size: 36px;
    margin-bottom: 40px;
  }
  .separator-cols img {
    width: fit-content;
    height: 100%;
  }
  .separator-cols {
    height: 100%;
    display: flex;
    flex-direction: column;
    color: var(--neutral-900);
    align-items: center;
    justify-content: space-between;
    gap: 40px;
    padding: 0px 12px;
  }
  .register-company-section {
    margin-top: 160px;
  }

  .register-form {
    min-height: 623px;
    padding-top: 24px;
  }
}

.input-opaque {
  padding: 15px 74px 15px 16px !important;
}

@media (min-width: 1200px) {
  .register-company-section {
    margin-top: 140px;
  }
}

.custom-tooltip {
  position:absolute;
  top: 100%;
  left: 0;
  background: #ffffff;
  padding: 8px;
  border-radius: 6px;
  white-space: normal;
  z-index: 1;
  width: 100%;
  border: 1px solid var(--neutral-600);
  word-wrap: break-word;
  font-weight: 400;
  font-size: 13px;
}
</style>
