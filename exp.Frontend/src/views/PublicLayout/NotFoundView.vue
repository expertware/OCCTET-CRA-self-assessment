<template>
  <MenuSecondaryPage></MenuSecondaryPage>

  <div class="hero-bg">
    <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
  </div>
  <div class="text-position">
    <h1 class="hero-section-title">Not Found...</h1>
  </div>
</template>
<script>
import MenuSecondaryPage from '@/components/Others/MenuSecondaryPageComponent.vue'
export default {
  name: '',
  components: {
    MenuSecondaryPage,
  },
  data() {
    return {}
  },
}
</script>
<style scoped>
.hero-bg {
  background-image: url('@/assets/images/hero-section-bg-mobile.svg');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: right;
  height: 479px;
  position: relative;
}

.hero-bg-curve {
  position: absolute;
  bottom: -20px;
  width: 100%;
  height: 78px;
}

.hero-section-title {
  font-size: 36px;
  color: #ffffff;
  line-height: 110.00000000000001%;
  margin-bottom: 22px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
}

.subtitle {
  color: #ffffff;
  opacity: 80%;
  margin-bottom: 40px;
  font-size: 15px;
}

@media (min-width: 768px) {
  .hero-bg {
    background-image: url('@/assets/images/bg-secondary-page.webp');
  }
}

@media (min-width: 992px) {
  .hero-section-title {
    font-size: 60px;
    text-transform: uppercase;
  }
  .subtitle {
    color: #ffffff;
    opacity: 80%;
    margin-bottom: 64px;
    font-size: 28px;
  }
  .hero-bg {
    height: 629px;
  }
}
.text-position {
  position: absolute;
  top: 40%;
  left: 50%;
  transform: translate(-50%, -50%);
  text-align: center;
}

@media (min-width: 1200px) {
  .hero-bg {
    height: 658px;
  }
  .hero-bg-curve {
    bottom: -91px;
    width: 100%;
    height: 271px;
  }
}

@media (min-width: 1400px) {
  .hero-bg-curve {
    bottom: -36px;
    width: 100%;
    height: 342px;
  }
}
</style>
