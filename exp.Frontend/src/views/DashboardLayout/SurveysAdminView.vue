<template>
  <div class="container mt-4 p-se-20-px">
    <div class="flex-between-center mb-4">
      <h2 class="color-N-900 fw-600 roboto-cond">Details</h2>
      <button type="button" class="btn btn-info btn-md" @click="OpenModalEditCompany()">
        Edit
      </button>
    </div>
    <div class="white-box">
      <LoaderOrganisationComponent
        v-if="!loaders.isOrganisationLoaded"
      ></LoaderOrganisationComponent>
      <div class="row" v-else>
        <div class="col-12 col-sm-6 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Name</h6>
            <h6 class="details-data">{{ organisation.Name }}</h6>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Vat</h6>
            <h6 class="details-data">{{ organisation.Vat }}</h6>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Location</h6>
            <h6 class="details-data">{{ organisation.Country?.Name }}</h6>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Activity Sector</h6>
            <h6 class="details-data" v-for="(sector, index) in organisation.ActivitySectors">
              {{ sector.Name }}
            </h6>
          </div>
        </div>
        <div class="col-12 col-sm-6 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Company size</h6>
            <h6 class="details-data">{{ organisation.CompanySize }}</h6>
          </div>
        </div>
        <div class="col-12 col-md-4 mb-3 mb-sm-4">
          <h6 class="details-title mb-1">Countries where you sale</h6>
          <div class="flex-between-center">
            <div class="flex-between-center wrap">
              <span v-for="(country, index) in organisation.DeliveryCountries">
                <div v-if="organisation.DeliveryCountries?.length > 3 && seeAllDeliveryCountries">
                  <span class="details-data">
                    {{ country?.Name
                    }}<span>{{
                      index == organisation.DeliveryCountries?.length - 1 ? '' : ',&nbsp '
                    }}</span></span
                  >
                </div>
                <span
                  v-else-if="organisation.DeliveryCountries?.length > 3 && !seeAllDeliveryCountries"
                >
                  <span class="details-data" v-if="index < 3">
                    {{ country?.Name }}<span>{{ index == 2 ? '...' : ',&nbsp ' }}</span></span
                  >
                </span>
                <div v-else-if="organisation.DeliveryCountries?.length <= 3">
                  <span class="details-data">
                    {{ country?.Name
                    }}<span>{{
                      index == organisation.DeliveryCountries?.length - 1 ? '' : ',&nbsp'
                    }}</span></span
                  >
                </div>
              </span>
            </div>
            <button
              @click="seeAllDeliveryCountries = !seeAllDeliveryCountries"
              v-if="organisation.DeliveryCountries?.length > 3"
              class="btn plain"
              type="button"
            >
              {{ seeAllDeliveryCountries ? 'See less' : 'See all' }}
            </button>
          </div>
        </div>

        <div class="col-12 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Contact person</h6>
            <h6 class="details-data">{{ organisation.ContactName }}</h6>
          </div>
        </div>
        <div class="col-12 col-md-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Contact email</h6>
            <h6 class="details-data">{{ organisation.ContactEmail }}</h6>
          </div>
        </div>
        <div class="col-12 col-lg-4 mb-3 mb-sm-4">
          <div>
            <h6 class="details-title">Website URL</h6>
            <h6 class="details-data">{{ organisation.Domain }}</h6>
          </div>
        </div>
      </div>
    </div>
    <div>
      <h2 class="color-N-900 fw-600 roboto-cond mb-4">Surveys</h2>
      <div class="row">
        <div class="col-12 col-md-6 col-xl-4 mb-3" v-for="(survey, index) in surveys.Items">
          <div class="card survey h-100">
            <div class="card-body">
              <div class="d-flex gap-4 align-items-start mb-4">
                <img src="@/assets/images/survey-folder.svg" alt="" />
                <div>
                  <h3 class="card-title mb-12-px">{{ survey.Name }}</h3>
                  <h6 class="card-text" :title="survey.Description">
                    {{ survey.Description }}
                  </h6>
                  <span class="label-survey">{{ survey.UpdateAt }}</span>
                </div>
              </div>
              <div>
                <hr />
                <div class="row">
                  <div class="col-12 col-sm-6 mb-3 mb-sm-0 d-flex flex-column justify-content-end">
                    <div v-if="survey.NumberOfFinishedSurveys > 0">
                      <h6 class="fw-600 mb-12-px color-N-700">
                        {{ survey.NumberOfFinishedSurveys }} completed
                      </h6>
                      <router-link
                        :to="{
                          name: 'surveys-by-category',
                          params: {
                            surveyId: survey.Id,
                            categorySurvey: survey.Name,
                            organisationId: organisation.Id,
                            ...(this.$route.params.companyName && {
                              companyName: this.$route.params.companyName,
                            }),
                          },
                        }"
                        class="btn btn-secondary btn-md w-100"
                        type="button"
                        >See history</router-link
                      >
                    </div>
                    <div v-else>
                      <h6 class="fw-600 mb-12-px color-N-700">0 completions</h6>
                    </div>
                  </div>
                  <div class="col-12 col-sm-6 d-flex flex-column justify-content-end"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="col-12 col-md-6 col-xl-4 mb-3" v-for="item in 3">
          <LoaderSurveysComponent v-if="!loaders.areSurveysLoaded"></LoaderSurveysComponent>
        </div>
        <NotFoundComponent
          text="No survey found"
          v-if="loaders.areSurveysLoaded && surveys.Items.length == 0"
        ></NotFoundComponent>
      </div>
      <PaginationComponent
        @pagechanged="GetSurveys"
        :totalPages="surveys.NumberOfPages"
        :currentPage="filter.PageNumber"
      ></PaginationComponent>
    </div>
  </div>
  <EditCompanyAdminModalComponent
    @updateOrganisation="GetOrganisation()"
    :copyCompany="organisationToEdit"
  ></EditCompanyAdminModalComponent>
</template>
<script>
import PaginationComponent from '@/components/Others/PaginationComponent.vue'
import LoaderSurveysComponent from '@/components/Others/loaders/LoaderSurveysComponent.vue'
import LoaderOrganisationComponent from '@/components/Others/loaders/LoaderOrganisationComponent.vue'
import NotFoundComponent from '@/components/Others/NotFoundComponent.vue'
import EditCompanyAdminModalComponent from '@/components/Modals/EditCompanyAdminModalComponent.vue'

export default {
  name: 'Surveys',
  components: {
    PaginationComponent,
    LoaderSurveysComponent,
    LoaderOrganisationComponent,
    NotFoundComponent,
    EditCompanyAdminModalComponent,
  },
  data() {
    return {
      filter: {
        PageSize: 3,
        PageNumber: 1,
        OrderBy: '',
      },
      seeAllDeliveryCountries: false,
      loaders: {},
      organisation: {},
      organisationToEdit: {},
      surveys: {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      },
    }
  },
  methods: {
    OpenModalEditCompany() {
      this.$store.commit('SET_LOADER', true)
      this.$axios
        .get(`/api/Administration/getOrgansation?organisationId=${this.$route.params.companyId}`)
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
        ...(this.$store.state.user.UserName &&
          !this.$store.state.user.UserRoles.includes('Guest') && {
            OrganisationId: Number(this.$route.params.companyId),
          }),
      }
      this.$axios
        .post(`/api/Administration/getOrganizationSurveys`, body)
        .then((response) => {
          this.surveys = response.data
          this.loaders.areSurveysLoaded = true
        })
        .catch(() => {
          this.loaders.areSurveysLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetOrganisation() {
      this.loaders.isOrganisationLoaded = false
      this.$axios
        .get(`/api/Administration/getOrgansation?organisationId=${this.$route.params.companyId}`)
        .then((response) => {
          this.organisation = response.data
          this.loaders.isOrganisationLoaded = true
        })
        .catch((error) => {
          if (error.response.data.message == 'Organization not found') {
            this.$router.push({
              name: 'not-found',
            })
          }
          this.loaders.isOrganisationLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  created() {
    this.GetSurveys()
    this.GetOrganisation()
  },
}
</script>
<style scoped>
.white-box {
  padding: 20px;
  margin-bottom: 40px;
}
.details-title {
  font-size: 13px;
  text-transform: uppercase;
}
.details-data {
  margin-top: 8px;
  font-size: 18px;
  font-weight: 600;
  color: var(--primary-700);
}

.card.survey {
  border: 1px solid var(--neutral-100);
  border-radius: 16px;
}
.survey .card-body {
  padding: 24px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.card-title {
  font-size: 18px;
  color: var(--neutral-900);
  font-weight: 600;
}
.card-text {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
  font-size: 13px;
  color: var(--neutral-700);
}

span.label-survey {
  font-size: 12px;
  color: var(--neutral-900);
  font-style: italic;
}

hr {
  color: #d9d9d9;
  opacity: 1;
  margin-bottom: 24px;
}
.btn.plain {
  min-width: 80px;
  color: var(--primary-400);
}

@media (min-width: 576px) {
  .white-box {
    padding: 40px 40px 20px 40px;
    margin-bottom: 40px;
  }
}
</style>
