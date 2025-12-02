<template>
  <div class="min-height">
    <RouterView></RouterView>
  </div>
</template>
<script>
import "../../assets/publicLayout.css"
export default {
  components: {

  },
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
