<template>
  <MenuSecondaryPage></MenuSecondaryPage>
  <div>
    <div class="hero-bg">
      <div class="pt-104-px container-md">
        <div class="row">
          <div class="col-12 col-xl-6 z-index-1">
            <h1 class="title">Privacy Policy</h1>
            <h3 class="subtitle">
              The Open CyberSecurity Compliance Toolkit (OCCTET) project values your privacy and is
              committed to protecting your personal data in accordance with the General Data
              Protection Regulation (GDPR) and applicable EU privacy laws.
            </h3>
          </div>
        </div>
      </div>
      <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
    </div>
    <div class="container-md margin">
      <div class="white-box mt-3">
        
        <h3 class="fw-600 mb-3">1. Who we are</h3>
        <p>
          OCCTET is an EU-funded initiative under the Digital Europe Programme, aimed at helping
          Small and Medium Enterprises (SMEs) comply with the EU Cyber Resilience Act (CRA). <br />
          <strong>Project Coordinator</strong>: Eclipse Foundation AISBL <br />
          <strong>Contact</strong>: occtet-eu@eclipse.org
        </p>
        <h3 class="fw-600 my-3">2. What data we collect:</h3>
        When you interact with our platform, we may collect:
        <ul>
          <li>Company details (name, VAT number, website, sector, country)</li>
          <li>Contact person details (name, role, email)</li>
          <li>
            Assessment data (survey responses, compliance scores, improvement recommendations)
          </li>
          <li>Technical data (IP address, browser type, and cookies for site functionality)</li>
        </ul>
        <h3 class="fw-600 my-3">3. Why we collect your data</h3>
        We process your personal data to:
        <ul>
          <li>Provide access to CRA self-assessment tools and surveys</li>
          <li>Generate compliance reports and recommendations for your organization</li>
          <li>Securely store and manage your access code for future assessments</li>
          <li>Improve our platform and ensure accurate benchmarking across SMEs</li>
          <li>Comply with legal obligations under EU regulations</li>
        </ul>
        <h3 class="fw-600 my-3">4. Legal basis for processing</h3>
        We process your data based on:
        <ul>
          <li>
            Your consent <a href="https://gdpr-info.eu/art-6-gdpr/">(Article 6(1)(a) GDPR)</a> when
            you register and agree to this policy
          </li>
          <li>
            Performance of a task in the public interest
            <a href="https://gdpr-info.eu/art-6-gdpr/">(Article 6(1)(e) GDPR)</a> in promoting
            cybersecurity compliance
          </li>
        </ul>
        <h3 class="fw-600 my-3">5. How we store and protect your data</h3>
        Your data is stored on secure EU-based servers with appropriate technical and organizational
        safeguards, including encryption and restricted access. <br />
        We retain your personal data only for as long as necessary to fulfil the purposes outlined
        in this policy or as required by law.
        <h3 class="fw-600 my-3">6. Data sharing</h3>
        We do not sell or share your personal data with third parties for marketing purposes. <br> Your
        data may be shared only with OCCTET project partners involved in delivering the platform’s
        functionality, under strict GDPR-compliant agreements.
        <h3 class="fw-600 my-3">7. Your rights under GDPR</h3>
        You have the right to:
        <ul>
          <li>
            Access your personal data
          </li>
          <li>
            Correct inaccurate or incomplete data
          </li>
          <li>Request deletion of your data</li>
          <li>Restrict or object to data processing</li>
          <li>Withdraw your consent at any time</li>
          <li>File a complaint with your national data protection authority</li>
          To exercise these rights, contact us at: occtet-eu@eclipse.org
        </ul>
        <h3 class="fw-600 my-3">8. Cookies</h3>
        Our site uses cookies to ensure proper functionality, improve user experience, and collect anonymized analytics. You can control cookie settings through your browser preferences.
        <h3 class="fw-600 my-3">Contact us</h3>
        If you have questions about this Privacy Policy or how we process your data, please email us at occtet-eu@eclipse.org.
      </div>
    </div>
  </div>
</template>
<script>
import MenuSecondaryPage from '@/components/Others/MenuSecondaryPageComponent.vue'
import NotFoundComponent from '@/components/Others/NotFoundComponent.vue'
export default {
  name: '',
  components: {
    MenuSecondaryPage,
    NotFoundComponent,
  },
  data() {
    return {
      filter: {
        Content: '',
      },

      loaders: {},
    }
  },
}
</script>
<style scoped>
.btn.arrow {
  position: absolute;
  left: 14px;
  top: 8px;
  background-image: url('@/assets/images/magnifier-icon.svg');
  background-size: contain;
  height: 32px;
  width: 56px;
  background-repeat: no-repeat;
  transition: all 0.3s;
}

.container-md.margin {
  margin-top: -163px;
  margin-bottom: 36px;
}

.white-box {
  position: relative;
  z-index: 2;
  color: var(--neutral-900);
  font-size: 15px;
  padding: 40px;
}

.pt-104-px {
  padding-top: 104px;
}
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

.title {
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

.custom-not-found {
  z-index: 5;
  position: relative;
  margin-top: 100px !important;
}

@media (min-width: 768px) {
  .hero-bg {
    background-image: url('@/assets/images/bg-secondary-page.webp');
  }
}

@media (min-width: 992px) {
  .container-md.margin {
    margin-top: -290px;
  }
  .title {
    font-size: 60px;
    text-transform: uppercase;
  }
  .subtitle {
    color: #ffffff;
    opacity: 80%;
    margin-bottom: 64px;
    font-size: 13px;
  }
  .hero-bg {
    height: 629px;
  }
}
@media (min-width: 1200px) {
  .hero-bg {
    height: 587px;
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
