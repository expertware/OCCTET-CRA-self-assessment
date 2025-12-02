<template>
  <div class="container-lg eu-resilience-section p-se-20-px">
    <div class="row">
      <div class="col-12 col-md-6 d-flex align-items-center">
        <div class="position-relative">
          <img class="img-form" src="@/assets/images/self-assessment-form.png" alt="" />
          <img class="blur" src="@/assets/images/self-assessment-blur.png" alt="" />
          <button class="btn btn-primary" type="button" @click="StartSurvey()">Start now</button>
        </div>
      </div>
      <div class="col-12 col-md-6 col-lg">
        <h1 class="title">Is your business ready for the EU Cyber Resilience Act?</h1>
        <div class="accordion accordion-flush" id="accordionFlushExample2">
          <div class="accordion-item" v-for="(item, index) in items" :key="item.Id">
            <h2 class="accordion-header" :id="`heading-cyber-${item.Id}`">
              <button
                class="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                :data-bs-target="`#collapse-faq-${item.Id}`"
                aria-expanded="false"
                :aria-controls="`collapse-faq-${item.Id}`"
              >
                {{ item.Question }}
              </button>
            </h2>
            <div
              :id="`collapse-faq-${item.Id}`"
              class="accordion-collapse collapse"
              :aria-labelledby="`heading-cyber-${item.Id}`"
              data-bs-parent="#accordionFlushExample2"
            >
              <div class="accordion-body">
                {{ item.Response }}
              </div>
            </div>
          </div>
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
          Id: 456,
          Question: 'Assess whether the CRA regulation applies to your company',
          Response:
            'Use this tool to quickly assess whether the CRA (Corporate Reporting Act) regulation applies to your company. Stay informed about your compliance obligations and take the necessary steps to meet regulatory requirements efficiently',
        },
        {
          Id: 457,
          Question: 'Test your knowledge of the CRA requirements',
          Response:
            'Test your knowledge of the CRA requirements with this quick quiz and ensure you’re up to date with the latest compliance standards.',
        },
        {
          Id: 458,
          Question: 'Help shape tools and resources designed to support SMEs',
          Response:
            'It only takes a few minutes — and could bring real value to your organization and others across Europe. rewrite',
        },
      ],
    }
  },
  methods: {
    StartSurvey() {

      const surveyId = localStorage.getItem('surveyId')
      if (!surveyId) {
        const firstSurveyId = import.meta.env.VITE_FIRST_SURVEY_ID
        this.$axios
          .post(`/api/Survey/startPublicSurvey/${firstSurveyId}`)
          .then((response) => {
            const surveyId = response.data
            localStorage.setItem('surveyId', JSON.stringify(surveyId))
            this.$router.push({
              name: 'first-survey',
              params: {
                surveyId: surveyId,
                surveyName:"CRA awareness"
              },
            })
          })
          .catch(() => {
            this.$utils.ToastNotifySomethingWentWrong()
          })
      } else {
        this.$router.push({
          name: 'first-survey',
          params: {
            surveyId: surveyId,
            surveyName:"CRA awareness"
          },
        })
      }
    },
  },
}
</script>
<style scoped>
.eu-resilience-section {
  padding-top: 60px;
  padding-bottom: 60px;
}
.title {
  color: var(--neutral-900);
  font-size: 36px;
  font-weight: 600 !important;
  font-family: 'Roboto Condensed';
  margin-bottom: 8px;
  margin-top: -17px;
  text-align: center;
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
  min-height: 80px;
  background-color: transparent;
  color: var(--neutral-900);
  font-weight: 600;
  font-size: 20px;
  padding: 16px 24px;
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
.img-form {
  width: 100%;
}

.blur {
  position: absolute;
  left: 0px;
  bottom: 34px;
  width: 100%;
}

.btn-primary {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  bottom: 120px;
}

@media (min-width: 768px) {
  .title {
    margin-top: 51px;
  }
}

@media (min-width: 1200px) {
  .title {
    text-align: start;
    margin-bottom: 20px;
    font-size: 48px;
  }
  .eu-resilience-section {
  padding-top: 90px;
  
}
}
</style>
