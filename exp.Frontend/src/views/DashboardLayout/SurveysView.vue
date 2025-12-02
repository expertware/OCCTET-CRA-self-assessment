<template>
  <div class="blue-bg">
    <div class="container-text m-auto">
      <h1 class="title pt-80-px">
        CRA Compliance: <br />Is your company aligned with the requirements?
      </h1>

      <div class="custom dropdown mt-40-px mx-3 mx-xl-0" ref="dropdownInputAi">
        <input
          data-bs-toggle="dropdown"
          aria-expanded="false"
          placeholder="Oxy is Ready to Help You"
          v-model="filter.SearchText"
          @keydown.enter="SendQuestionToOxy()"
          type="text"
        />
        <img class="search" src="@/assets/images/dropdown-search-lupe.svg" alt="" />
        <img
          @click="ToggleDropdown()"
          class="arrow"
          src="@/assets/images/dropdown-search-arrow.svg"
          alt=""
        />

        <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1" role="menu">
          <h6 class="dropdown-category">Recommendations</h6>
          <div
            @click="OpenModalSeeQuestion(option)"
            class="question"
            v-for="option in options.recommendations"
            :key="option"
          >
            <div>
              <img src="@/assets/images/dropdown-star.svg" alt="" />
              {{ option.Name }}
            </div>
            <img src="@/assets/images/dropdown-arrow.svg" alt="" />
          </div>
          <hr />
          
        </ul>
      </div>
    </div>
    <div class="container-lg px-4 px-md-5 white-box position-relative">
      <img class="blue-form-desktop" src="@/assets/images/surveys-blue-form.svg" alt="" />
      <img class="blue-form-mobile" src="@/assets/images/blue-form-mobile.png" alt="" />
      <div class="row w-100">
        <div class="col-12 col-md-6 z-index-1 text-center text-md-start">
          <h3>Your Guide to CRA Compliance</h3>
          <h4 class="mt-2">A quick introduction to CRA and the steps needed to stay compliant.</h4>
          <router-link
            :to="{ name: 'craDetails' }"
            class="btn btn-primary btn-md mt-4 fit-content mx-auto mx-md-0"
            >View details</router-link
          >
        </div>
        <div class="col-12 col-md-6 text-center">
          <img class="girl-image" src="@/assets/images/surveys-girl-image.svg" alt="" />
        </div>
      </div>
    </div>
  </div>
  <div class="container-lg mt-4 p-se-20-px">
    <div class="row flex">
      <div class="col-12 col-xl-6 ps-xl-5">
        <StepsComponent activeStep="step 1"></StepsComponent>
        <h1 class="title">Survey for SMEs on CRA Compliance</h1>
        <h4>
          <p>
            This survey is intended for SMEs and FOSS contributors to help determine whether their
            products or activities fall within the scope of the EU Cyber Resilience Act (CRA). By
            completing it, you’ll gain insight into your responsibilities and contribute to
            identifying areas where further guidance or tools are needed.
          </p>
        </h4>
      </div>
      <div
        class="col-12 col-xl-6 d-flex justify-content-center align-items-md-center h-100 mt-5 mt-xl-0 col-min-height"
      >
        <div class="position-relative">
          <img
            class="img-section d-md-none"
            src="@/assets/images/surveys-step-mobile-11.webp"
            alt=""
          />
          <img
            class="img-section d-none d-md-block"
            src="@/assets/images/surveys-step-11.webp"
            alt=""
          />
          <div class="absolute-btns">
            <div class="row">
              <div class="col-6 d-flex flex-column justify-content-end">
                <div v-if="FindSurvey(3)?.NumberOfFinishedSurveys > 0">
                  <h6 class="fw-600 mb-12-px color-N-700">
                    {{ FindSurvey(3)?.NumberOfFinishedSurveys }} completed
                  </h6>
                  <router-link
                    :to="{
                      name: 'surveys-by-category',
                      params: {
                        surveyId: FindSurvey(3)?.Id,
                        categorySurvey: FindSurvey(3)?.Name,
                      },
                    }"
                    class="btn btn-secondary btn-md w-100"
                    type="button"
                    >See history</router-link
                  >
                </div>
              </div>
              <div class="col-6 d-flex flex-column justify-content-end h-100 mt-3">
                <h6 class="fw-600 mb-12-px color-N-700">
                  {{
                    FindSurvey(3)?.ProgressedSurveyId ? 'Pending completion' : 'Start new survey'
                  }}
                </h6>
                <router-link
                  v-if="FindSurvey(3)?.ProgressedSurveyId"
                  :to="{
                    name: 'survey',
                    params: {
                      surveyId: FindSurvey(3)?.ProgressedSurveyId,
                      surveyName: FindSurvey(3)?.Name,
                      categorySurveyId: 3,
                    },
                  }"
                  class="btn btn-primary btn-md w-100"
                  type="button"
                >
                  Continue
                </router-link>
                <button
                  v-else
                  @click="StartSurvey(FindSurvey(3)?.Id, FindSurvey(3)?.Name, 3)"
                  class="btn btn-primary btn-md w-100"
                  type="button"
                >
                  Start
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <hr />
  </div>

  <div class="container-lg mt-4 p-se-20-px">
    <div class="row flex">
      <div
        class="col-12 col-xl-6 d-flex justify-content-center align-items-md-center h-100 mt-5 mt-xl-0 col-min-height order-last order-xl-first"
      >
        <div class="position-relative">
          <img
            class="img-section d-md-none"
            src="@/assets/images/surveys-step-2-mobile.webp"
            alt=""
          />
          <img
            class="img-section d-none d-md-block"
            src="@/assets/images/surveys-step-2.webp"
            alt=""
          />
          <div class="absolute-btns">
            <div class="row">
              <div class="col-6 d-flex flex-column justify-content-end">
                <div v-if="FindSurvey(7)?.NumberOfFinishedSurveys > 0">
                  <h6 class="fw-600 mb-12-px color-N-700">
                    {{ FindSurvey(7)?.NumberOfFinishedSurveys }} completed
                  </h6>
                  <router-link
                    :to="{
                      name: 'surveys-by-category',
                      params: {
                        surveyId: FindSurvey(7)?.Id,
                        categorySurvey: FindSurvey(7)?.Name,
                      },
                    }"
                    class="btn btn-secondary btn-md w-100"
                    type="button"
                    >See history</router-link
                  >
                </div>
              </div>
              <div class="col-6 d-flex flex-column justify-content-end h-100 mt-3">
                <h6 class="fw-600 mb-12-px color-N-700">
                  {{
                    FindSurvey(7)?.ProgressedSurveyId ? 'Pending completion' : 'Start new survey'
                  }}
                </h6>
                <router-link
                  v-if="FindSurvey(7)?.ProgressedSurveyId"
                  :to="{
                    name: 'survey',
                    params: {
                      surveyId: FindSurvey(7)?.ProgressedSurveyId,
                      surveyName: FindSurvey(7)?.Name,
                      categorySurveyId: 7,
                    },
                  }"
                  class="btn btn-primary btn-md w-100"
                  type="button"
                >
                  Continue
                </router-link>
                <button
                  v-else
                  @click="StartSurvey(FindSurvey(7)?.Id, FindSurvey(7)?.Name, 7)"
                  class="btn btn-primary btn-md w-100"
                  type="button"
                >
                  Start
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-12 col-xl-6 ps-xl-5">
        <StepsComponent activeStep="step 2"></StepsComponent>
        <h1 class="title">Self-Qualification Questionnaire</h1>
        <h4>
          <p>
            The purpose of this questionnaire is to assist SMEs in establishing the applicability of
            the CRA, assessing the criticality of their products or services, and accessing
            structured guidance on compliance and maturity assessment processes.
          </p>
        </h4>
      </div>
    </div>
    <hr />
  </div>

  <div class="container-lg mt-4 p-se-20-px">
    <div class="row flex">
      <div class="col-12 col-xl-6 ps-xl-5">
        <StepsComponent activeStep="step 3"></StepsComponent>
        <h1 class="title">SMEs CRA Maturity Assesment</h1>
        <h4>
          <p>
            This maturity assessment helps SMEs evaluate their technical readiness in relation to
            the Cyber Resilience Act (CRA). It identifies strengths and weaknesses in current
            practices, highlights areas that require improvement, and provides tailored guidance to
            support continuous development toward compliance.
          </p>
        </h4>
      </div>
      <div
        class="col-12 col-xl-6 d-flex justify-content-center align-items-md-center h-100 mt-5 mt-xl-0 col-min-height"
      >
        <div class="position-relative">
          <img
            class="img-section d-md-none"
            src="@/assets/images/surveys-step-3-mobile.webp"
            alt=""
          />
          <img
            class="img-section d-none d-md-block"
            src="@/assets/images/surveys-step-3.webp"
            alt=""
          />
          <div class="absolute-btns">
            <div class="row">
              <div class="col-6 d-flex flex-column justify-content-end">
                <div v-if="FindSurvey(8)?.NumberOfFinishedSurveys > 0">
                  <h6 class="fw-600 mb-12-px color-N-700">
                    {{ FindSurvey(8)?.NumberOfFinishedSurveys }} completed
                  </h6>
                  <router-link
                    :to="{
                      name: 'surveys-by-category',
                      params: {
                        surveyId: FindSurvey(8)?.Id,
                        categorySurvey: FindSurvey(8)?.Name,
                      },
                    }"
                    class="btn btn-secondary btn-md w-100"
                    type="button"
                    >See history</router-link
                  >
                </div>
              </div>
              <div class="col-6 d-flex flex-column justify-content-end h-100 mt-3">
                <h6 class="fw-600 mb-12-px color-N-700">
                  {{
                    FindSurvey(8)?.ProgressedSurveyId ? 'Pending completion' : 'Start new survey'
                  }}
                </h6>
                <router-link
                  v-if="FindSurvey(8)?.ProgressedSurveyId"
                  :to="{
                    name: 'survey',
                    params: {
                      surveyId: FindSurvey(8)?.ProgressedSurveyId,
                      surveyName: FindSurvey(8)?.Name,
                      categorySurveyId: 8,
                    },
                  }"
                  class="btn btn-primary btn-md w-100"
                  type="button"
                >
                  Continue
                </router-link>
                <button
                  v-else
                  @click="StartSurvey(FindSurvey(8)?.Id, FindSurvey(8)?.Name, 8)"
                  class="btn btn-primary btn-md w-100"
                  type="button"
                >
                  Start
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <SeeQuestionAnswerModalComponent
    @reset="ResetOcctetAnswer()"
    :option="selectedAnswer"
    :oxyResponse="oxyResponse"
    :question="selectedQuestion"
  ></SeeQuestionAnswerModalComponent>
</template>
<script>
import StepsComponent from '@/components/Others/Survey/StepsComponent.vue'
import SeeQuestionAnswerModalComponent from '@/components/Modals/SeeQuestionAnswerModalComponent.vue'
import { Dropdown } from 'bootstrap'
export default {
  name: 'Surveys',
  components: {
    StepsComponent,
    SeeQuestionAnswerModalComponent,
  },
  data() {
    return {
      oxyResponse: false,
      filter: {
        PageSize: 3,
        PageNumber: 1,
        OrderBy: '',
        SearchText: '',
      },
      loaders: {},
      organisationToEdit: {},
      surveys: {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      },
      selectedQuestion: null,
      selectedAnswer: null,
      abortController: null,

      options: {
        recommendations: [
          {
            Name: 'Is Your Company Aligned with the Requirements',
            Text: `<body>
  <h4 class="mb-2">What is it?</h4>
  <p class="mb-2">The EU regulation that sets cybersecurity requirements for products with digital elements (hardware and software) placed on the EU market.</p>

  <h4 class="mb-2">Main Objectives</h4>
  <ul class="mb-2">
    <li>Increasing the security of digital products throughout their entire lifecycle.</li>
    <li>Protecting consumers against cyber threats.</li>
    <li>Imposing clear rules for manufacturers and economic operators.</li>
    <li>Supervision and enforcement of compliance.</li>
  </ul>

  <h4 class="mb-2">Scope</h4>
  <ul class="mb-2">
    <li>Products with digital elements that can connect to networks or other devices.</li>
    <li>Includes consumer electronics, software, industrial systems, critical infrastructures.</li>
    <li>Manufacturers, importers, and distributors in the EU or selling to the EU market.</li>
  </ul>

  <h4 class="mb-2">Manufacturers' Obligations</h4>
  <ul class="mb-2">
    <li>Cyber risk assessment.</li>
    <li>Secure-by-design development and engineering.</li>
    <li>Technical documentation and EU declaration of conformity.</li>
    <li>Managing and reporting vulnerabilities and incidents.</li>
    <li>Providing security updates for at least 5 years.</li>
  </ul>

  <h4 class="mb-2">Product Classification</h4>
  <ol>
    <li>Standard products – self-assessment.</li>
    <li>Important products (Class I) – self-assessment + EU declaration.</li>
    <li>Critical products (Class II) – assessment by a notified body.</li>
  </ol>
  <p class="mb-2">Compliant products receive the CE marking.</p>

  <h4 class="mb-2">Key Dates</h4>
  <ul class="mb-2">
    <li>December 10, 2024: entry into force.</li>
    <li>December 11, 2027: full application of obligations.</li>
  </ul>

  <h4 class="mb-2">Sanctions</h4>
  <ul class="mb-2">
    <li>Fines up to €15 million or 2.5% of global annual turnover for manufacturers.</li>
    <li>Smaller fines for other economic operators.</li>
  </ul>

  <h4 class="mb-2">Exceptions</h4>
  <ul class="mb-2">
    <li>Medical devices, vehicles, aviation and maritime products, and defense-related products.</li>
  </ul>
</body>
`,
          },
          {
            Name: 'Why is it important to know if my product falls under the CRA',
            Text: `<body>
  <section class="mb-2">
    <h4 class="mb-2">Legal Compliance</h4>
    <p>If your product is subject to the CRA and you don’t comply, you risk <strong>heavy fines</strong> (up to €15 million or 2.5% of global turnover).</p>
  </section>

  <section class="mb-2">
    <h4 class="mb-2">Security by Design Requirements</h4>
    <p>You must ensure <strong>mandatory cybersecurity measures</strong> are built into your product from the start—design, development, updates, and beyond.</p>
  </section>

  <section class="mb-2">
    <h4 class="mb-2">Time to Prepare</h4>
    <p>The CRA allows time until <strong>December 2027</strong> for full compliance. Early identification gives you a head start to update processes and plan accordingly.</p>
  </section>

  <section class="mb-2">
    <h4 class="mb-2">Market Access</h4>
    <p>Non-compliant products may be <strong>barred from the EU market</strong>. Meeting CRA requirements ensures your product can be legally sold with the <strong>CE mark</strong>.</p>
  </section>

  <section class="mb-2">
    <h4 class="mb-2">Product Classification</h4>
    <p>Different rules apply for <strong>standard, important, or critical</strong> products. Misclassification can lead to delays or missed obligations.</p>
  </section>

  <section class="mb-2">
    <h4 class="mb-2">Supply Chain Impact</h4>
    <p>Partners and customers may request <strong>proof of CRA compliance</strong>. It’s essential for maintaining trust and collaboration in the digital supply chain.</p>
  </section>

</body>`,
          },
          {
            Name: 'Who should check if they are subject to the CRA?',
            Text: `<body>
  <section class="mb-2">
    <ul>
      <li><strong>Manufacturers</strong> of products with digital elements sold or used in the EU.</li>
      <li><strong>Importers</strong> who bring hardware or software products into the EU market.</li>
      <li><strong>Distributors</strong> that handle or resell digital products within the EU.</li>
      <li><strong>Software developers</strong> offering downloadable apps, SaaS tools, or firmware updates.</li>
      <li><strong>IoT device producers</strong>, including smart home, health, and industrial equipment makers.</li>
      <li><strong>Companies outside the EU</strong> that market digital products to EU customers (B2B or B2C).</li>
      <li><strong>Startups</strong> and SMEs launching connected or embedded systems in the EU.</li>
    </ul>
    <p>If your product includes connectivity or software functionality and is sold in the EU, it is your responsibility to verify CRA applicability.</p>
  </section>

</body>
`,
          },

          {
            Name: 'Who can complete the CRA surveys?',
            Text: `<body>
  <section class="mb-2">
    <h4 class="mb-2">Recommended Roles:</h4>
    <ul>
      <li><strong>IT Manager or CISO</strong> – for technical and cybersecurity-related questions.</li>
      <li><strong>Product Manager or Software Developer</strong> – for questions about digital features and product architecture.</li>
      <li><strong>Compliance Officer</strong> – for legal, regulatory, and CE marking responsibilities.</li>
      <li><strong>General Manager / CEO</strong> – especially in SMEs where roles may overlap.</li>
    </ul>
  </section>

  <section>
    <h4 class="mb-2">What Matters Most:</h4>
    <ul>
      <li>The person must have <strong>sufficient knowledge</strong> about the product and security posture.</li>
      <li>They should be <strong>internally authorized</strong> to respond on behalf of the company.</li>
      <li>No external certification is required, but answers should be <strong>accurate, verifiable, and accountable</strong>.</li>
    </ul>
  </section>

</body>`,
          },
          {
            Name: 'Can I complete the maturity assessment myself, or do I need an authorized person?',
            Text: `<body>

  <section>
    <p class="mb-2">If your product is a <strong>default product</strong> (i.e., not classified as important or critical), you can complete the maturity assessment yourself.</p>

    <p class="mb-2">However, if your product is classified as <strong>important (Class I or II)</strong> or <strong>critical</strong>, the assessment must be completed by an <strong>authorized person</strong> with expertise in cybersecurity, compliance, or product architecture.</p>

    <p>If you're unsure about your product’s classification, please take <strong>Survey 2: "Self-assessment – Qualification"</strong> to determine the correct category.</p>
  </section>

</body>`,
          },
        ],
      },
    }
  },
  methods: {
    ResetOcctetAnswer() {
      if (this.abortController) {
        this.abortController.abort()
      }
      this.selectedQuestion = null;
      this.selectedAnswer = null;
    },
    ToggleDropdown() {
      const inputEl = this.$refs.dropdownInputAi
      if (inputEl) {
        const dropdown = Dropdown.getOrCreateInstance(inputEl)
        dropdown.toggle()
      }
    },
    FindSurvey(id) {
      return this.surveys.Items.find((survey) => survey.Id == id)
    },
    StartSurvey(surveyId, surveyName, categorySurveyId) {
      this.$axios
        .post(`/api/Survey/startSurvey/${surveyId}`)
        .then((response) => {
          const surveyId = response.data
          this.$router.push({
            name: 'survey',
            params: {
              surveyId: surveyId,
              surveyName: surveyName,
              categorySurveyId: categorySurveyId,
            },
          })
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    OpenModalEditCompany() {
      this.$store.commit('SET_LOADER', true)
      const url = this.$route.params.companyId
        ? `/api/Administration/getOrgansation?organisationId=${this.$route.params.companyId}`
        : `/api/Organisation/getOrganisation`
      this.$axios
        .get(url)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.organisationToEdit = response.data
          this.$route.params.companyId
            ? $('#editCompanyAdminModal').modal('show')
            : $('#editCompanyModal').modal('show')
        })
        .catch(() => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetSurveys(page) {
      this.loaders.areSurveysLoaded = false
      this.filter.PageNumber = 1
      if (page) {
        this.filter.PageNumber = page
      }
      this.surveys = {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      }
      const body = {
        PageSize: this.filter.PageSize,
        PageNumber: this.filter.PageNumber,
        OrderBy: '',
      }
      this.$axios
        .post('/api/Survey/getOrganizationSurveys', body)
        .then((response) => {
          this.surveys = response.data
          this.loaders.areSurveysLoaded = true
        })
        .catch(() => {
          this.loaders.areSurveysLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    OpenModalSeeQuestion(option) {
      this.oxyResponse = false
      this.selectedAnswer = option.Text
      this.selectedQuestion = option.Name
      $('#seeQuestionAnswer').modal('show')
    },
    async SendQuestionToOxy() {
      if (!this.filter.SearchText) return
      this.oxyResponse = true
      this.selectedQuestion = this.filter.SearchText
      $('#seeQuestionAnswer').modal('show')
      if (this.abortController) {
        this.abortController.abort()
      }
      this.abortController = new AbortController()

      const obj = {
        Prompt:
          'Can you answer to this question related to cyber security  : ' +
          this.filter.SearchText +
          ' in a nice html and css format only with h3, h4, p and ul with li with spaces between sections and not send ```html\n',
      }

      try {
        const response = await this.$axios.post(`/api/Assistent/sendMessage`, obj, {
          signal: this.abortController.signal,
        })
        this.selectedAnswer = response.data
      } catch (error) {
        if (error.name === 'CanceledError' || error.name === 'AbortError') {
        } else if (error.status == 429) {
          this.selectedAnswer = 'Oxi can’t answer right now, please come back in a few minutes.'
        } else {
          this.$utils.ToastNotifySomethingWentWrong()
        }
      }
    },
  },
  created() {
    this.GetSurveys()
  },
}
</script>
<style scoped>
.blue-bg {
  background-image: url('@/assets/images/surveys-blue-bg.webp');
  min-height: 600px;
  margin-top: -20px;
}

/*----------------------------------------------------*/
.absolute-btns {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  bottom: 30px;
  width: 100%;
}

.title {
  color: var(--neutral-900);
  font-size: 36px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
  margin-bottom: 8px;
  margin-top: 40px;
}

@media (min-width: 1200px) {
  .title {
    font-size: 48px;
    margin-bottom: 16px;
  }
}

.col-min-height {
  min-height: 380px;
}

.img-section {
  width: 100%;
}

@media (min-width: 530px) {
}

@media (min-width: 576px) {
  .absolute-btns {
    bottom: 40px;
  }
}

@media (min-width: 768px) {
  .absolute-btns {
    bottom: 134px;
  }

  .img-section {
    width: auto;
  }

  .absolute-btns {
    padding: 0px 120px;
  }
}

@media (min-width: 768px) {
  .absolute-btns {
    bottom: 60px;
  }

  .img-section {
    width: 100%;
  }
}

@media (min-width: 1400px) {
  .absolute-btns {
    padding: 0px 120px;
    bottom: 75px;
  }
}

.pt-80-px {
  padding-top: 80px;
}

.container-text {
  text-align: center;
  max-width: 916px;
}

.custom.dropdown input {
  height: 48px;
  border: none;
  border-radius: 8px;
  box-shadow: 0px 8px 16px 0px rgba(157, 157, 157, 0.16);
  width: 100%;
  padding-left: 46px;
}

.custom.dropdown input:focus-visible {
  outline: none;
  box-shadow: 0px 8px 16px 0px rgba(157, 157, 157, 0.5);
}

.custom.dropdown .search {
  position: absolute;
  left: 12px;
  top: 12px;
}

.arrow {
  width: 24px !important;
  height: 24px !important;
  position: absolute;
  right: 12px;
  top: 12px;
  cursor: pointer;
}

.dropdown-menu {
  width: 100%;
  border: none;
  padding: 16px 12px;
  --bs-dropdown-zindex: 11;
  box-shadow: 0px 16px 40px 0px rgba(53, 53, 53, 0.16);
}

.dropdown-category {
  font-size: 13px;
  text-transform: uppercase;
  margin-bottom: 8px;
}

.question {
  font-size: 15px;
  color: var(--neutral-700);
  display: flex;
  align-items: start;
  justify-content: space-between;
  gap: 10px;
  padding: 8px;
  margin-bottom: 8px;
  cursor: pointer;
}

.question:hover {
  background-color: rgba(248, 249, 253, 1);
  border-radius: 8px;
}

.custom.dropdown hr {
  color: var(--neutral-100);
  opacity: 1;
  margin-bottom: 16px;
}

.white-box {
  margin-top: 60px;
  min-height: 218px;
  box-shadow: 0px 12px 64px 0px rgba(100, 135, 158, 0.16);
  padding: 40px 60px;
  display: flex;
  align-items: center;
  position: relative;
  overflow-x: clip;
}

h3 {
  font-size: 24px;
  font-weight: 600;
  color: var(--neutral-900);
}

.blue-form-desktop {
  position: absolute;
  height: 100%;
  right: 0px;
  width: auto;
}

.blue-form-mobile {
  position: absolute;
  height: 100%;
  right: 0px;
  width: auto;
}

.girl-image {
  position: relative;
}

@media (min-width: 768px) {
  .girl-image {
    position: absolute;
    right: 78px;
    top: -13px;
  }
}
</style>
