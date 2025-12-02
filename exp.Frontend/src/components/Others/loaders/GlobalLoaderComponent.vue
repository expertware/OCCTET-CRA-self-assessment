<template>
  <div class="page-loader-background">
    <div class="blue-bg">
      <img src="@/assets/images/global-loader.gif" alt="" />
    </div>
  </div>
</template>
<script>
export default {
  name: 'LoaderComponent',
  data() {
    return {}
  },
}
</script>
<style scoped>
.page-loader-background {
  width: 100vw;
  height: 100vh;
  position: fixed;
  background: linear-gradient(180deg, #e0f2fe 0%, #f6f7f9 100.98%);

  top: 0;
  left: 0;
  z-index: 4000;
  display: flex;
  align-items: center;
  justify-content: center;
}
.blue-bg {
  width: 100%;
  height: 100%;
  background-image: url('@/assets/images/global-loader-bg.webp');
  background-repeat: no-repeat;
  background-position: center;
  display: flex;
  justify-content: center;
  align-items: center;
}
@media (max-width: 600px) {
  img {
    width: 100%;
  }
}
</style>
