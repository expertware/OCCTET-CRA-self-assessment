<template>
  <div class="top-menu">
    <router-link
      :to="{
        name: 'home',
      }"
    >
      <img class="logo-occtet" src="@/assets/images/logo-small.svg" alt="logo-occtet" />
    </router-link>
    <div class="container-lg d-flex align-items-center justify-content-end justify-content-md-start h-100">
      <button v-if="$route.name!='first-survey'" type="button" class="btn btn-dark btn-md" @click="OpenModalEditCompany()">Company details</button>
    </div>

    <div class="gap-5 d-none d-md-flex align-items-center justify-content-center logos">
      <img height="32px" src="@/assets/images/EU-logo1.svg" alt="logo-eu" />
      <img height="32px" src="@/assets/images/EU-logo2.svg" alt="logo-eccc" />
    </div>
  </div>
  <EditCompanyModalComponent
    :copyCompany="organisationToEdit"
  ></EditCompanyModalComponent>
</template>
<script>
import EditCompanyModalComponent from '../Modals/EditCompanyModalComponent.vue';
export default {
  name: '',
  components:{
    EditCompanyModalComponent,
  },
  data() {
    return {
      organisationToEdit:null
    }
  },
  methods:{
     OpenModalEditCompany() {
      this.$store.commit('SET_LOADER', true)
     
      this.$axios
        .get(`/api/Organisation/getOrganisation`)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.organisationToEdit = response.data
          $('#editCompanyModal').modal('show');
        })
        .catch(() => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  }
}
</script>
<style scoped>
.top-menu {
  height: 65px;
  width: 100%;
  background-color: #14181f;
  padding: 0px 20px;
  position: fixed;
  z-index: 12;
  display: flex;
    align-items: center;
    justify-content: space-between;
}

.logo-occtet {
  width: 133px;
}

img:not(.logo-occtet) {
  filter: invert(99%) sepia(1%) saturate(521%) hue-rotate(71deg) brightness(114%) contrast(112%);
}
@media(min-width:768px){
  .top-menu {
  
  padding: 0px 4%;
 
}
}
@media (min-width: 1650px) {
  .top-menu {
    display: block;
  }
  .logo-occtet {
    position: absolute;
    top: 13px;
    left: 40px;
  }
  .logos {
  position: absolute;
  right: 40px;
  top: 18px;
}

}
</style>
