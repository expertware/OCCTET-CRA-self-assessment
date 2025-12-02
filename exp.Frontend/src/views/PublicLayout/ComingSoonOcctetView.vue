<template>
  <div class="hero-bg">
    <div class="hero-text">
      <h1 class="section-title">
        COMING SOON: <br />
        <p class="pt-4">OCCTET – Self Assessment Portal</p>
      </h1>
      <h3 class="subtitle">
        Evaluate your Business Cyber Resilience. Free, instant, and aligned with the EU Cyber
        Resilience Act.
      </h3>
      <h5 class="color-white">
        OCCTET is a free, instant self-assessment platform designed to help organizations evaluate
        their Business Cyber Resilience Readiness. Get a clear overview of your company’s strengths
        and areas for improvement, while staying aligned with the EU Cyber Resilience Act.
      </h5>
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
.hero-bg {
  background-image: url('@/assets/images/blue-bg-coming-soon.webp');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: right;
  height: 100vh;
  position: relative;
  display: flex;
  justify-content: center;
}
.hero-bg::before {
  content: '';
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
}

.hero-text {
  position: relative;
  z-index: 1; 
  text-align: center;
  padding-top: 200px;
  max-width: auto;
  padding-left: 20px;
  padding-right: 20px;

}

.section-title {
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

@media (min-width: 992px) {
  .section-title {
    font-size: 60px;
    text-transform: uppercase;
  }
  .subtitle {
    color: #ffffff;
    opacity: 80%;
    margin-bottom: 64px;
    font-size: 28px;
  }
  .hero-text {
  position: relative;
  z-index: 1; 
  text-align: center;
  padding-top: 200px;
  max-width: 1000px;

}
}

</style>
