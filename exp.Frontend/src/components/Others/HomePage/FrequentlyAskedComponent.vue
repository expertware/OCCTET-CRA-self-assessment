<template>
  <div class="container-lg p-se-20-px frequently-asked-section">
    <h1 class="title">Frequently asked questions</h1>

    <div class="accordion accordion-flush" id="FAQ-accordion">
      <div class="accordion-item" v-for="(item, index) in items" :key="item.Id">
        <h2 class="accordion-header" :id="`heading--${item.Id}`">
          <button
            class="accordion-button collapsed"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="`#collapse${item.Id}`"
            aria-expanded="false"
            :aria-controls="`collapse${item.Id}`"
          >
            {{ item.Question }}
          </button>
        </h2>
        <div
          :id="`collapse${item.Id}`"
          class="accordion-collapse collapse"
          :aria-labelledby="`heading--${item.Id}`"
          data-bs-parent="#FAQ-accordion"
        >
          <div class="accordion-body"><div v-html="item.Response"></div></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: '',
  data() {
    return {
      items: [
        {
          Id: 111,
          Question: 'Why OCCTET?',
          Response:
            'Many SMEs face challenges in meeting cybersecurity compliance requirements due to limited resources, restricted technical expertise, and unclear regulatory guidance. OCCTET addresses these challenges by providing practical, open-source tools that make compliance with the EU Cyber Resilience Act (CRA) simpler, faster, and more cost-effective. By supporting open-source collaboration, OCCTET ensures that its compliance toolkit is transparent, adaptable, and community-driven — benefitting not only SMEs, but also policymakers, industry stakeholders, and the broader cybersecurity ecosystem. With OCCTET, SMEs can focus on innovation and growth, confident that their products meet security and compliance standards. Stay informed: Follow the project’s progress, news, and updates at <a target=_blank"" href=https://occtet.eu/>www.occtet.eu.</a> ',
        },
        {
          Id: 222,
          Question: 'What is the CRA?',
          Response:
            'The Cyber Resilience Act (CRA) is a European Union regulation aimed at ensuring that digital products — including both hardware and software — are secure by design, secure by default, and secure throughout their entire lifecycle. It requires that products connecting to the internet or other digital systems are developed, maintained, and updated with strong cybersecurity measures. The CRA sets clear requirements for manufacturers, importers, and distributors to protect users and reduce vulnerabilities across the EU’s digital ecosystem.',
        },
        {
          Id: 333,
          Question: 'How does this help SMEs?',
          Response:
            'Our platform simplifies the CRA compliance process by offering a structured, easy-to-use toolkit. SMEs can assess their current maturity level, identify areas of non-compliance, receive tailored guidance, and compare their progress with other similar organizations. This reduces legal and cybersecurity risks and helps prepare for future regulatory requirements.',
        },
        {
          Id: 444,
          Question: 'Why is the CRA important?',
          Response:
            'The Cyber Resilience Act (CRA) is not just a best practice — it is a legal requirement for digital products sold or used within the EU. It mandates that products be secure by design and secure by default throughout their lifecycle. For SMEs, CRA compliance is essential because:  <ul>    <li>  It reduces the risk of costly cyber incidents and product vulnerabilities</li>  <li>It avoids significant penalties and sanctions for non-compliance, which can include product recalls or market withdrawal</li>    <li> It builds trust with customers, partners, and regulators</li>    <li>It strengthens market competitiveness by demonstrating robust security practices</li>  </ul> Failing to comply with the CRA can result in serious financial, legal, and reputational consequences. Early adoption ensures your business stays ahead of regulations — and avoids the risk of being locked out of the EU market. ',
        },
        {
          Id: 555,
          Question: 'Accessing Your Survey Results',
          Response:
            "Once you complete your registration, you will receive a unique access code linked to your organization. This code allows you to: <ul>    <li> Log in securely at any time</li>  <li>Access the self-assessment surveys</li>    <li> View and update your CRA compliance results</li>    <li>Compare your maturity score with other SMEs</li>  </ul> Please keep your code safe, as it is required to access your results and continue your CRA compliance journey. If you've already registered, you can enter your code <a href='#home'>here</a>",
        },
      ],
    }
  },
}
</script>
<style scoped>
.frequently-asked-section {
  padding-top: 95px;
}
.title {
  color: var(--neutral-900);
  font-size: 36px;
  margin-bottom: 40px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
  text-align: center;
  margin-bottom: 24px;
}

.accordion-item {
  background-color: transparent;
}

.accordion-body {
  padding: 0px 24px 24px 24px;
  color: #525e6f;
  font-size: 15px;
}

.accordion-button {
  height: 80px;
  background-color: transparent;
  color: var(--neutral-900);
  font-weight: 600;
  font-size: 20px;
  padding: 0px 24px;
}
.accordion-button:not(.collapsed) {
  color: var(--neutral-900);
  background-color: transparent;
  box-shadow: none;
}
.accordion-button:focus {
  z-index: 3;
  outline: 0;
  box-shadow: none;
}

.accordion-button::after {
  flex-shrink: 0;
  width: 32px;
  height: 32px;
  margin-left: auto;
  content: '';
  background-image: url('@/assets/images/icon-plus.svg');
  background-repeat: no-repeat;
  background-size: 100%;
  transition: var(--bs-accordion-btn-icon-transition);
}

.accordion-button:not(.collapsed)::after {
  background-image: url('@/assets/images/icon-minus.svg');
  transform: var(--bs-accordion-btn-icon-transform);
}

@media (min-width: 768px) {
  .frequently-asked-section {
    padding-top: 120px;
  }
}
@media (min-width: 1200px) {
  .title {
    font-size: 48px;
    margin-bottom: 68px;
  }
}
</style>
