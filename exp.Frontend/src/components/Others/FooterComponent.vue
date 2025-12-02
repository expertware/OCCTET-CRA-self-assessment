<template>
  <div class="footer">
    <div class="container-lg">
      <div class="row mb-20-px">
        <div class="col-12 col-sm-auto text-center text-sm-start">
          <router-link
            :to="{
              name: 'home',
            }"
          >
            <img class="logo-occtet w-auto mt-3" src="@/assets/images/logo-small.svg" alt="" />
          </router-link>
        </div>
        <div class="col-12 col-sm">
          <div
            class="d-flex wrap justify-content-center mt-3 justify-content-sm-end gap-3 gap-lg-5"
          >
            <img height="42px" src="@/assets/images/EU-logo1.svg" alt="logo-eu" />
            <img height="42px" src="@/assets/images/EU-logo2.svg" alt="logo-eccc" />
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col pe-5">
          <h6 class="color-white mt-2 mt-lg-4 mt-sm-0">
            OCCTET project has received funding from the Digital Europe Programme (DIGITAL), under
            grant agreement number: 101190474. The content does not necessarily reflect the views of
            the European Commission. The European Commission is not liable for any use that may be
            made of the information contained herein.
          </h6>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: '',
  data() {
    return {}
  },
}
</script>
<style scoped>
.footer {
  background-color: var(--neutral-900);
  min-height: 136px;
  padding: 20px 0px 20px 0px;
  width: 100%;
}

img:not(.logo-occtet) {
  filter: invert(99%) sepia(1%) saturate(521%) hue-rotate(71deg) brightness(114%) contrast(112%);
}
@media (max-width: 576px) {
  h6 {
    font-size: 12px;
    text-align: center;
  }
}
</style>
