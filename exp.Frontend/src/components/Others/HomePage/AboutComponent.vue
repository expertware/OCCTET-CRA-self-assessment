<template>
  <div class="about-section container-lg p-se-20-px" id="about">
    <div class="row">
      <div class="col-12 col-lg-5 p-se-20-px">
        <h1 class="title">Welcome to the OCCTET Self-Assessment</h1>
        <h4>
          <p>
            The Open CyberSecurity Compliance Toolkit (OCCTET) is designed to help Small and Medium
            Enterprises (SMEs) evaluate their readiness for the EU Cyber Resilience Act (CRA) —
            especially when using Free and Open Source Software (FOSS) in digital products.
          </p>
          <p class="mt-3">
            Funded by the EU Digital Europe Programme, OCCTET offers practical, open-source tools
            that guide you through every step of compliance — from identifying OSS components to
            producing clear, actionable reports.
          </p>
          <p class="mt-3">
            By completing this self-assessment, you’ll: <br />
            <b>a.</b> Get an overview of your current compliance status. <br />
            <b>b.</b> Discover gaps and improvement areas.<br />
            <b>c.</b> Learn which OCCTET tools can help you move towards full CRA compliance.
          </p>

          <p class="mt-3">The toolkit includes:</p>
          <ul class="mt-2">
            <li>Compliance Checklist</li>
            <li>Conformity Assessment Specifications</li>
            <li>Automated Evaluation Method and Tool</li>
            <li>Federated OSS Assessment Database – Share and access component data</li>
            <li>Dependency Analysis Tools Inventory – Track OSS usage.</li>
            <li>Reporting Tool – Summarize your compliance status.</li>
          </ul>
          <p class="mt-3">
            Let’s begin your assessment and build a stronger cybersecurity posture together.
          </p>
        </h4>
      </div>
      <div class="d-none d-lg-block col-lg-7 carousel-overflow pe-0">
        <AboutCarouselComponent></AboutCarouselComponent>
      </div>
    </div>
  </div>
</template>
<script>
import AboutCarouselComponent from './AboutCarouselComponent.vue'
export default {
  name: '',
  components: {
    AboutCarouselComponent,
  },
  data() {
    return {}
  },
}
</script>
<style scoped>
.about-section {
  padding-top: 190px;
}

.title {
  color: var(--neutral-900);
  font-size: 36px;
  margin-bottom: 24px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
}
.carousel-overflow {
  border-top-right-radius: 24px;
  overflow: hidden;
  position: relative;
}

@media (min-width: 992px) {
  .title {
    margin-bottom: 40px;
    font-size: 48px;
  }
}

@media (min-width: 1200px) {
  .about-section {
    padding-top: 84px;
  }
}
</style>
