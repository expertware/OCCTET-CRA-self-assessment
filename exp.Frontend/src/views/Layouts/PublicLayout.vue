<template>
  <div class="min-height public-layout">
    <RouterView></RouterView>
    <FooterComponent></FooterComponent>
  </div>
</template>
<script>
import "../../assets/publicLayout.css"
import FooterComponent from '@/components/Others/FooterComponent.vue'
export default {
  components: {
    FooterComponent,
  },
  created(){

  }
}
</script>

<style scoped>
.min-height {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  justify-content: space-between;
  background-color: #e2f2ff;
}


</style>
