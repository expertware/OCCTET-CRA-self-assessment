<template>
  <div class="container-lg p-se-20-px why-register-section">
    <div class="row">
      <div class="col-12 col-md-6">
        <div class="lottie-container" ref="lottieContainer" id="lottie-container"></div>
      </div>
      <div class="col-12 col-md-6 text-wrapper">
        <h1 class="title text-center text-md-start">Why register</h1>
        <div class="list-item">
          <img src="@/assets/images/check-icon.svg" alt="" />
          <h4>
            Access the survey for Cyber Resilience Act (CRA) compliance.
          </h4>
        </div>
        <div class="list-item">
          <img src="@/assets/images/check-icon.svg" alt="" />
          <h4>
           View your company's maturity score.
          </h4>
        </div>
        <div class="list-item">
          <img src="@/assets/images/check-icon.svg" alt="" />
          <h4>
            Compare your results with other companies in the industry
          </h4>
        </div>
        <div class="list-item">
          <img src="@/assets/images/check-icon.svg" alt="" />
          <h4>
        Receive tailored recommendations for improving your cybersecurity posture.
          </h4>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import lottie from 'lottie-web'
const animationPath = new URL('@/assets/images/lottie-animation.json', import.meta.url).href
export default {
  name: '',
  data() {
    return {}
  },
   mounted() {
    lottie.loadAnimation({
      container: this.$refs.lottieContainer,
      renderer: 'svg',
      loop: true,
      autoplay: true,
      path: animationPath,
    })
  },
}
</script>
<style scoped>
.why-register-section {
  margin-bottom: 100px;
  margin-top: 0px;
}
.text-wrapper {
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 0px 20px;
}
.title {
  color: var(--neutral-900);
  font-size: 36px;
  margin-bottom: 24px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
}

.list-item {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 40px;
}
.list-item:last-of-type {
  margin-bottom: 0px;
}

.lottie-container{
  height: 500px;
}

@media(min-width:768px){
  .why-register-section {
  margin-bottom: 90px;
}

.text-wrapper {

  padding: 0px 60px;
}
}

@media (min-width: 1200px) {
  .title {
    font-size: 48px;
    margin-bottom: 40px;
  }
  .why-register-section {
  margin-bottom: 110px;
}
  
}
</style>
