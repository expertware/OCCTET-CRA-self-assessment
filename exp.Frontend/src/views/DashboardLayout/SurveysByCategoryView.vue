<template>
  <div class="container-fluid">
    <div class="container mt-4">
      <div class="container p-0">
        <div class="flex-between-center mb-4">
          <h2 class="color-N-900 fw-600 roboto-cond">{{ surveys.Name }}</h2>
          <button
            type="button"
            @click="StartSurvey()"
            v-if="
              !surveys.Surveys?.find((item) => item.Status === 'Progress') &&
              this.$store.state.user.UserRoles.includes('Guest')
            "
            class="btn btn-info btn-md"
          >
            Start
          </button>
        </div>
        <h6 class="mb-40-px">
          {{ surveys.Description }}
        </h6>
      </div>

      <div class="white-box px-2 pt-2">
        <div class="table-responsive">
          <table class="custom table">
            <thead>
              <tr>
                <th width="25"></th>
                <th>
                  <span
                    @click="OrderBy('start')"
                    class="pointer flex-align-center"
                    :class="{
                      'active-order-by': filter.OrderBy.includes('start'),
                    }"
                  >
                    <OrderByComponent value="start" :orderBy="filter.OrderBy"></OrderByComponent
                    >Started date
                  </span>
                </th>
                <th>Updated date</th>
                <th width="20%">Status</th>
                <th>Result</th>
                <th></th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(survey, index) in surveys.Surveys" :key="index">
                <td></td>
                <td class="fw-600 color-N-900">
                  {{ this.$utils.FormatDateDMY(survey.StartDate) }}
                </td>
                <td>{{ this.$utils.FormatDateDMY(survey.UpdateDate) }}</td>
                <td>
                  <div class="flex-align-center gap-2">
                    <img
                      width="20"
                      v-if="survey.Status == 'Progress'"
                      src="@/assets/images/survey-menu-in-progress.svg"
                      alt=""
                    />
                    <img
                      width="20"
                      v-if="survey.Status == 'Finished'"
                      src="@/assets/images/survey-menu-checked.svg"
                      alt=""
                    />
                    <h6 class="ms-2">{{ survey.Status }}</h6>
                  </div>
                </td>
                <td>{{ survey.Result ? survey.Result : '-' }}</td>
                <td></td>
                <td>
                  <span class="d-flex justify-content-end"
                    ><router-link
                      class="btn btn-md"
                      :class="survey.Status == 'Progress' ? 'btn-primary' : 'btn-secondary'"
                      :to="{
                        name: 'survey',
                        params: {
                          surveyId: survey.OrganisationSurveyId,
                          surveyName: surveys.Name,
                          ...($route.params.organisationId && {
                            organisationId: $route.params.organisationId,
                          }),
                          ...($route.params.companyName && {
                            companyName: $route.params.companyName,
                          }),
                          
                            categorySurveyId: $route.params.surveyId,
                          
                        },
                      }"
                    >
                      {{ survey.Status == 'Progress' ? 'Continue' : 'View' }}</router-link
                    >
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <LoaderTableComponent v-if="!loaders.areSurveysLoaded"></LoaderTableComponent>
      </div>
      <div></div>
    </div>
  </div>
</template>
<script>
import LoaderSurveysComponent from '@/components/Others/loaders/LoaderSurveysComponent.vue'
import NotFoundComponent from '@/components/Others/NotFoundComponent.vue'
import OrderByComponent from '@/components/Others/OrderByComponent.vue'
import LoaderTableComponent from '@/components/Others/loaders/LoaderTableComponent.vue'
export default {
  name: 'Surveys',
  components: {
    LoaderSurveysComponent,
    NotFoundComponent,
    OrderByComponent,
    LoaderTableComponent,
  },
  data() {
    return {
      filter: {
        OrderBy: 'start',
      },
      loaders: {},
      surveys: {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      },
    }
  },
  methods: {
    OrderBy(orderBy) {
      const value = this.filter.OrderBy.split('_')
      if (value[0] === orderBy) {
        if (!this.filter.OrderBy.includes('_desc')) {
          this.filter.OrderBy = `${orderBy}_desc`
        } else {
          this.filter.OrderBy = orderBy
        }
      } else {
        this.filter.OrderBy = `${orderBy}_desc`
      }
      if (this.filter.OrderBy == 'start') {
        this.surveys.Surveys = this.surveys.Surveys.sort(
          (a, b) => new Date(a.StartDate) - new Date(b.StartDate),
        )
      } else if (this.filter.OrderBy == 'start_desc') {
        this.surveys.Surveys = this.surveys.Surveys.sort(
          (a, b) => new Date(b.StartDate) - new Date(a.StartDate),
        )
      }
    },
    GetSurveys() {
      this.loaders.areSurveysLoaded = false

      const searchParams = {
        SurveyId: this.$route.params.surveyId,
        ...(this.$route.params.organisationId && {
          OrganisationId: this.$route.params.organisationId,
        }),
      }
      const api = this.$route.params.organisationId
        ? `/api/Administration/getHistorySurvey?${new URLSearchParams(searchParams)}`
        : `/api/Survey/getHistorySurvey?${new URLSearchParams(searchParams)}`
      this.$axios
        .get(api)
        .then((response) => {
          this.surveys = response.data
          this.OrderBy(this.filter.OrderBy)
          this.loaders.areSurveysLoaded = true
        })
        .catch(() => {
          this.loaders.areSurveysLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    StartSurvey() {
      this.$axios
        .post(`/api/Survey/startSurvey/${this.surveys.Id}`)
        .then((response) => {
          const surveyId = response.data
          this.$router.push({
            name: 'survey',
            params: {
              surveyId: surveyId,
              surveyName: this.surveys.Name,
              ...(this.$route.params.organisationId && {
                organisationId: this.$route.params.organisationId,
              }),
              ...(this.$route.params.companyName && {
                companyName: this.$route.params.companyName,
              }),
              
                categorySurveyId: this.$route.params.surveyId
             
            },
          })
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },

  created() {
    this.GetSurveys()
  },
}
</script>
<style scoped>
.white-box {
  padding: 40px 40px 20px 40px;
  margin-bottom: 40px;
}

hr {
  color: #d9d9d9;
  opacity: 1;
  margin-bottom: 24px;
}
.btn {
  width: 190px;
}
</style>
