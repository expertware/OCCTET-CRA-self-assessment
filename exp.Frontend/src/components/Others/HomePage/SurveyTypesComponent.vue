<template>
  <div class="overflow-hidden">
    <div class="container-lg survey-types-section p-se-20-px">
      <h1 class="title">Start CRA Check</h1>

      <div class="row g-lg-5">
        <div
          class="col-12 col-md-6 col-lg-4 mb-3 mb-lg-0"
          v-for="(item, index) in items"
          :key="index"
        >
          <div class="card h-lg-100">
            <div class="card-body">
              <div>
                <h5 class="card-title">{{ item.Name }}</h5>
                <p class="card-text">
                  {{ item.Description }}
                </p>
              </div>
              <div>
                <span class="green mt-4 mb-40-px">{{ item.Label }}</span>

                <button
                  v-if="item.Button == 'Go to register'"
                  class="btn btn-secondary w-100"
                  @click="$emit('scroll', 'register')"
                  type="button"
                >
                  {{ item.Button }}
                </button>

                <button @click="StartSurvey()" v-else class="btn btn-secondary w-100" type="button">
                  {{ item.Button }}
                </button>
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
  emits: ['scroll'],
  data() {
    return {
      items: [
        {
          Name: 'CRA awareness',
          Description:
            "This survey is designed for SMEs and FOSS contributors to help assess whether their products or activities fall under the scope of the EU Cyber Resilience Act (CRA). By answering a few questions, you'll gain clarity on your obligations and help us identify where additional guidance and tools are needed.",
          Label: 'Start right now',
          Button: 'Start now',
        },
        {
          Name: 'Self assessment -qualification',
          Description:
            'This questionnaire helps SMEs determine whether the CRA regulation applies to them, assess the criticality of their products or services, and access practical guidance on conformity and maturity assessments.',
          Label: 'Must be registered',
          Button: 'Go to register',
        },
        {
          Name: 'Self assessment -CRA maturity',
          Description:
            'This maturity assessment helps SMEs evaluate their technical readiness, identify areas for improvement, and benchmark themselves against similar organizations.',
          Label: 'Must be registered',
          Button: 'Go to register',
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
  .survey-types-section {
  padding-bottom: 0px;
}
.title {
  font-size: 36px;
  font-family: 'Roboto Condensed';
  font-weight: 600 !important;
  color: var(--neutral-900);
  text-align: center;
  margin-bottom: 24px;
}
.card {
  border: none;
  border-radius: 16px;
  padding: 16px;
}

.card-title {
  font-size: 28px;
  font-weight: 600 !important;
  margin-bottom: 24px;
}
.card-text {
  font-size: 18px;
  color: var(--neutral-700);
}

.card-body {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}
@media (min-width: 768px) {
  .h-lg-100 {
    height: 100%;
  }
    .survey-types-section {
  padding-bottom: 50px;
}
}

@media (min-width: 1200px) {
  .title {
    font-size: 48px;
    margin-bottom: 56px;
  }
    .survey-types-section {
  padding-bottom: 90px;
}
  

}
</style>
