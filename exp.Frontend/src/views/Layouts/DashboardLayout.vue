<template>
  <div class="min-height admin-layout">
    <div>
      <MenuAdminComponent
        v-if="shouldShowMenuAdmin"
      ></MenuAdminComponent>
      <MenuSurveysComponent v-else></MenuSurveysComponent>
      <div class="container-lg margin-top-85px p-se-20-px">
        <BreadCrumbComponent  v-if="!['surveys', 'reports'].includes($route.name)"></BreadCrumbComponent>
      </div>
      <RouterView></RouterView>
    </div>
    <!-- <FooterComponent></FooterComponent> -->
  </div>
</template>
<script>
import MenuSurveysComponent from '@/components/Others/MenuSurveysComponent.vue'
import FooterComponent from '@/components/Others/FooterComponent.vue'
import '../../assets/administrationLayout.css'
import BreadCrumbComponent from '@/components/Others/BreadCrumbComponent.vue'
import MenuAdminComponent from '@/components/Others/MenuAdminComponent.vue'
export default {
  name: '',
  components: {
    MenuSurveysComponent,
    FooterComponent,
    BreadCrumbComponent,
    MenuAdminComponent,
  },
  data() {
    return {}
  },
  computed: {
    shouldShowMenuAdmin() {
      return this.$store.state.user.UserName && !this.$store.state.user.UserRoles.includes('Guest')
    },
  },
}
</script>
<style scoped>
.min-height {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  justify-content: space-between;
  background-color: var(--neutral-50);
}

.margin-top-85px {
  margin-top: 85px;
}
</style>
