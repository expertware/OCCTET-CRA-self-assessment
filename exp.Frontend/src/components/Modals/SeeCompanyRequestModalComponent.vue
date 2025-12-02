<template>
  <div
    class="modal fade"
    id="seeCompanyRequest"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Company request</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="separator"></div>
          <div class="row">
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label">Company name</label>
              <div class="inactive-control">{{ request?.Name }}</div>
            </div>
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label mt-3 mt-md-0">Company number/ VAT</label>
              <div class="inactive-control">{{ request?.Vat }}</div>
            </div>
          </div>
          <div class="row">
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label mt-3">Domain</label>
              <div class="inactive-control">{{ request?.Domain }}</div>
            </div>
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label  mt-3">Company size</label>
              <div class="inactive-control">{{ request?.CompanySize }}</div>
            </div>
          </div>
          <div class="row">
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label  mt-3">Country</label>
              <div class="inactive-control">{{ request?.Country.Name }}</div>
            </div>
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label  mt-3">Delivery countries</label>
              <div class="inactive-control flex-wrap">
                <div v-for="(country, index) in request?.DeliveryCountries">{{
                  country.Name
                }}{{index+1<request?.DeliveryCountries.length ? ', &nbsp ': ' '}} </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-12">
              <label for="company-name" class="form-label  mt-3">Activity sectors</label>
              <div class="inactive-control flex-wrap">
                <div v-for="(domain, index) in request?.ActivitySectors">{{ domain.Name }} {{index+1<request?.ActivitySectors.length ? ', &nbsp ': ' '}} </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label  mt-3">Contact name</label>
              <div class="inactive-control">{{ request?.ContactName }}</div>
            </div>
            <div class="col-12 col-md-6">
              <label for="company-name" class="form-label  mt-3">Contact email</label>
              <div class="inactive-control">{{ request?.ContactEmail }}</div>
            </div>
          </div>
          <div class="row">
            <div class="col-12">
              <label for="company-name" class="form-label  mt-3">Contact role</label>
              <div class="inactive-control">{{ request?.ContactPersonRole }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: 'SeeCompanyRequestModal',
  props: {
    request: {
      type: Object,
      default() {
        return {
          Name:null,
          Vat:null,
        }
      },
    },
  },
}
</script>
<style scoped>
.separator {
  height: 1px;
  width: 100%;
  background-color: var(--neutral-200);
  margin: 12px 0px 24px 0px;
}

.modal-dialog {
  max-width: 900px;
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
  padding-bottom: 24px;
}

.modal-footer {
  padding-top: 24px;
}
.modal-title {
  font-size: 24px;
  color: var(--neutral-900);
  font-weight: 600 !important;
  text-align: center;
  font-family: 'Roboto';
}
h5 {
  color: var(--neutral-900);
  font-size: 15px;
}
</style>
