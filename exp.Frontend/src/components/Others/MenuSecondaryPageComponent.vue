<template>
  <div class="top-menu px-5">
    <router-link
      :to="{
        name: 'home',
      }"
    >
      <img src="@/assets/images/logo-small.svg" alt="logo-occtet" />
    </router-link>

    <div class="container"></div>
  </div>
  <div class="mobile-menu">
    <router-link
      :to="{
        name: 'home',
      }"
    >
      <img src="@/assets/images/logo-small.svg" alt="logo-occtet" />
    </router-link>
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
.top-menu {
  height: 88px;
  width: 100%;
  display: none;
  align-items: center;
  position: absolute;
  top: 0;
  z-index: 10;
  background-color: transparent;
  transition:
    background-color 0.3s ease,
    position 0.3s ease;
}

.mobile-menu {
  height: 68px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: absolute;
  top: 0px;
  z-index: 1;
  padding-left: 20px;
  padding-right: 20px;
}

@media (min-width: 992px) {
  .top-menu {
    display: flex;
  }

  .mobile-menu {
    display: none;
  }
}
</style>
