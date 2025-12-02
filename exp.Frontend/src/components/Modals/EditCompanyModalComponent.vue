<template>
  <div
    class="modal fade"
    id="editCompanyModal"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <Form
          @submit="EditCompany()"
          ref="editCompany"
          :validation-schema="schema"
          v-slot="{ errors }"
        >
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Edit company</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            <div class="row">
              <div class="col-12 col-sm-8">
                <label for="company-name" class="form-label">Company name</label>
                <Field
                  type="text"
                  v-model="company.Name"
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
              <div class="col-12 col-sm-4 mt-3 mt-sm-0">
                <label for="Vat" class="form-label">Company Number / VAT</label>
                <h5 class="inactive-control">{{ company.Vat }}</h5>
              </div>
            </div>
            <div class="row mt-3">
              <div class="col-12 col-sm-6">
                <label for="domain" class="form-label">Website URL</label>
                <Field
                  type="text"
                  v-model="company.Domain"
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
                <Field name="country" v-model="company.Country">
                  <multiselect
                    class="custom-multiselect"
                    :class="{ 'field-error': errors.country }"
                    v-model="company.Country"
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
                <Field name="companySize" v-model="company.CompanySize">
                  <multiselect
                    class="custom-multiselect"
                    :class="{ 'field-error': errors.companySize }"
                    v-model="company.CompanySize"
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
                    <span class="color-P-500 pointer" @click="ShowDomains()">here</span>
                    )</i
                  ></span
                >
                <Field name="activitySectors" v-model="company.ActivitySectors">
                  <multiselect
                    class="custom-multiselect"
                    :class="{ 'field-error': errors.activitySectors }"
                    v-model="company.ActivitySectors"
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
              <Field name="deliveryCountries" v-model="company.DeliveryCountries">
                <multiselect
                  class="custom-multiselect"
                  :class="{ 'field-error': errors.deliveryCountries }"
                  v-model="company.DeliveryCountries"
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

            <div class="row mb-4">
              <div class="col-12 col-md mt-3 mt-lg-0">
                <label for="company-contact-person" class="form-label">Contact person</label>
                <h5 class="inactive-control">{{ company.ContactName }}</h5>
              </div>
              <div class="col-12 col-md mt-3 mt-lg-0">
                <label for="company-email" class="form-label">Contact email</label>
                <h5 class="inactive-control">{{ company.ContactEmail }}</h5>
              </div>
              <div class="col-12 col-md mt-3 mt-lg-0">
                <label for="company-contact-role" class="form-label">Role</label>
                <h5 class="inactive-control">{{ company.ContactPersonRole }}</h5>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="sumbit" class="btn btn-primary">Save</button>
          </div>
        </Form>
      </div>
    </div>
  </div>
</template>
<script>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
export default {
  name: 'EditCompanyModal',
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  emits: ['updateOrganisation'],
  props: {
    copyCompany: {
      type: Object,
      default() {
        return {}
      },
    },
  },
  data() {
    return {
      company: {
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
      },
      loaders: {},
      countries: [],
      companySizes: ['Micro (1–9)', 'Small (10–49)', 'Medium (50–249)', 'Large (250+)'],
      activitySectors: [],
      deliveryCountries: [],
    }
  },
  methods: {
    ShowDomains(){
      $('#editCompanyModal').modal('hide');
      this.$router.push({
        name:'domains'
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
    EditCompany() {
      this.$store.commit('SET_LOADER', true)
      const obj = {
        Id: this.company.Id,
        Name: this.company.Name,
        Domain: this.company.Domain,
        ActivitySectors: this.company.ActivitySectors.map((item) => item.Id),
        CompanySize: this.company.CompanySize,
        CountryId: this.company.Country.Id,
        DeliveryCountries: this.company.DeliveryCountries.map((item) => item.Id),
      }
      this.$axios
        .put(`/api/Organisation/updateOrganisation`, obj)
        .then((response) => {
          this.$emit('updateOrganisation')
          $('#editCompanyModal').modal('hide')
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotify('success', 'Company has been edited')
        })
        .catch(() => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  watch: {
    copyCompany(newVal, oldVal) {
      this.company = JSON.parse(JSON.stringify(newVal))
    },
  },
  computed: {
    schema() {
      return yup.object({
        companyName: yup
          .string()
          .required('This field is required'),
          // .min(3, 'Minimum 3 characters')
          // .max(500, 'Maximum 500 characters'),
        // domain: yup.string().required('This field is required').url('Invalid URL'),
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
      })
    },
  },
}
</script>
<style scoped>
.separator {
  height: 1px;
  width: 100%;
  background-color: var(--neutral-200);
  margin: 24px 0px;
}

.modal-dialog {
  max-width: 1000px;
}
.modal-content {
  border: none;
  padding: 24px;
  border-radius: 12px;
}
.modal-header {
  padding: 0px;
  border-bottom: 0px;
}
.modal-body {
  padding: 0px;
}

.modal-title {
  font-size: 32px;
  color: var(--neutral-900);
  font-weight: 600 !important;
  text-align: center;
  font-family: 'Roboto Condensed';
}
h5 {
  color: var(--neutral-900);
  font-size: 15px;
}
h6 {
  color: var(--neutral-700);
  font-size: 12px;
}

.btn {
  width: 206px;
}
</style>
