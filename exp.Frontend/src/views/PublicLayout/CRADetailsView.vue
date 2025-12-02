<template>
  <MenuSecondaryPage></MenuSecondaryPage>
  <div class="hero-bg">
    <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
  </div>
  <div class="text-position p-se-20-px">
    <div class="pt-104-px container-md">
      <div class="row">
        <div class="col-12 col-xl-6 z-index-1">
          <h1 class="title">Your Guide to CRA Compliance</h1>
        </div>
      </div>
      <div class="white-box my-3">
        <h3 class="fw-600 mb-3">Your Guide to CRA Compliance</h3>
        <p>
          The <b>EU Cyber Resilience Act (CRA)</b> is a new regulation that will affect nearly every
          company placing digital products or software on the EU market.
        </p>

        <p>
          This guide is here to help you — whether you’re a small startup, growing SME, or
          established provider — understand what CRA means in practice and how you can stay
          compliant without being overwhelmed.
        </p>

        <h3 class="fw-600 my-3">Introduction to the Cyber Resilience Act:</h3>
        <p>
          The Cyber Resilience Act (CRA) is an EU regulation aimed at strengthening the
          cybersecurity of digital products and software. Its goal is to ensure that hardware,
          software, and connected devices placed on the EU market meet essential cybersecurity
          requirements throughout their lifecycle.
        </p>
        <p>
          The CRA establishes clear obligations for manufacturers, importers, and distributors to
          integrate cybersecurity practices into the design, development, and maintenance of
          products. By doing so, it reduces risks for consumers and businesses while improving trust
          in digital technologies.
        </p>
        <p>
          In short:
          <b>if you sell or use digital products in the EU, CRA probably applies to you.</b>
        </p>
        <h3 class="fw-600 my-3">Key Steps to Stay Compliant</h3>
        <ul>
          <li>
            <strong>Identify your role under CRA</strong>
            <p>
              Determine whether you are a manufacturer, importer, or distributor — as obligations
              differ.
            </p>
          </li>
          <li>
            <strong>Integrate security by design and by default</strong>
            <p>
              Ensure cybersecurity is built into product development from the start, covering both
              hardware and software.
            </p>
          </li>
          <li>
            <strong>Maintain vulnerability management processes</strong>
            <p>
              Establish procedures for identifying, disclosing, and remediating vulnerabilities,
              including mandatory reporting of incidents to ENISA.
            </p>
          </li>
          <li>
            <strong>Provide security updates and support</strong>
            <p>
              Guarantee that products receive timely updates and patches throughout their expected
              lifecycle.
            </p>
          </li>
          <li>
            <strong>Ensure transparency and documentation</strong>
            <p>
              Maintain technical documentation, conformity assessments, and clear instructions for
              users regarding security features and safe usage.
            </p>
          </li>
          <li>
            <strong>Prepare for compliance assessments</strong> Depending on your product category,
            you may need third-party conformity assessments or internal checks to demonstrate
            compliance before placing products on the EU market.
          </li>
        </ul>
        <h3 class="fw-600 my-3">Why It Matters</h3>

        <p>
          Non-compliance with the CRA can lead to products being withdrawn from the EU market and
          significant penalties. More importantly, aligning with CRA ensures safer products,
          stronger customer trust, and long-term competitiveness in the digital economy. Whether you
          are a startup, SME, or established tech provider, this guide gives you clear direction and
          actionable steps to align with the Cyber Resilience Act.
        </p>
        <ul class="my-3">
          <li><b>Avoid fines and product bans.</b></li>
          <li>Build trust with your customers.</li>
          <li>Stay competitive in the EU market.</li>
        </ul>

        <p>
         Think of CRA as an opportunity: it doesn’t just protect your business from penalties, it helps you offer safer, more trusted products that customers actually want.
        </p>
        <p><b>Tip for SMEs: </b>Start small. Map your role, document your processes, and put a basic vulnerability management system in place. You can then build up to full compliance step by step.</p>
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
  margin-top: -100px;
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
    margin-top: -230px;
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

.hero-bg {
  background-image: url('@/assets/images/hero-section-bg-mobile.svg');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: right;
  height: 479px;
  position: relative;
  width: 100%;
  z-index: 1;
  position: fixed;
}

.hero-bg-curve {
  position: absolute;
  bottom: -20px;
  width: 100%;
  height: 78px;
}

@media (min-width: 768px) {
  .hero-bg {
    background-image: url('@/assets/images/bg-secondary-page.webp');
  }
}

@media (min-width: 992px) {
  .hero-bg {
    height: 629px;
  }
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
