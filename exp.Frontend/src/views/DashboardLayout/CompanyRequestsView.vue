<template>
  <div class="container-lg mt-4 p-se-20-px">
    <h2 class="color-N-900 fw-600 roboto-cond mb-4">Companies requests</h2>

    <div class="white-box mb-3">
      <div class="row gutter-12-px">
        <div class="col-12 col-md-6 col-lg-4 col-xl-3">
          <label for="search-text-requests" class="form-label">Search</label>
          <div class="input-group flex-nowrap">
            <span class="input-group-text"
              ><img src="@/assets/images/search.svg" alt="search" /></span
            ><input
              v-model="filter.SearchText"
              class="form-control"
              type="text"
              id="search-text-requests"
              placeholder="Search requests"
              aria-describedby="search-user"
              autocomplete="off"
              @keydown.enter="GetRequests()"
            />
          </div>
        </div>
        <div class="col-12 col-md-6 col-lg-4 col-xl-3 mt-3 mt-md-0">
          <div class="position-relative">
            <label for="search-country-companies" class="form-label">Country</label>
            <multiselect
              class="custom-multiselect"
              v-model="filter.Country"
              :options="countries"
              placeholder="Search countries"
              selected-label=""
              deselect-label=""
              select-label=""
              label="Name"
              :allow-empty="false"
              track-by="Id"
              :loading="loaders.loaderMultiselectCountries"
              @select="GetRequests()"
              @open="GetCountries()"
            ></multiselect>
            <img
              v-if="filter.Country != null"
              src="/src/assets/images/x-reset.svg"
              alt=""
              class="button-reset-multiselect"
              @click="((filter.Country = null), GetRequests())"
            />
          </div>
        </div>
        <div class="col"></div>
        <div class="col-auto mt-3 mt-lg-0">
          <div class="h-100 d-flex align-items-end gap-2">
            <button type="button" @click="GetRequests()" class="btn action">
              <img src="@/assets/images/filter.svg" alt="" />
            </button>
            <button type="button" @click="ResetFilters()" class="btn action">
              <img src="@/assets/images/delete.svg" alt="" />
            </button>
          </div>
        </div>
      </div>
    </div>
    <div class="white-box px-2 pt-2 mb-5">
      <div class="table-responsive">
        <table class="custom table">
          <thead>
            <tr>
              <th width="8"></th>
              <th>
                <span
                  @click="OrderBy('Name')"
                  class="pointer flex-align-center"
                  :class="{
                    'active-order-by': filter.OrderBy.includes('Name'),
                  }"
                >
                  <OrderByComponent value="Name" :orderBy="filter.OrderBy"></OrderByComponent
                  >Company name
                </span>
              </th>
              <th>VAT</th>
              <th>
                <span
                  @click="OrderBy('RegistrationDate')"
                  class="pointer flex-align-center"
                  :class="{
                    'active-order-by': filter.OrderBy.includes('RegistrationDate'),
                  }"
                >
                  <OrderByComponent
                    value="RegistrationDate"
                    :orderBy="filter.OrderBy"
                  ></OrderByComponent
                  >Request date
                </span>
              </th>

              <th>Country</th>
              <th>Domain URL</th>
              <th>Contact info</th>

              <th></th>
              <th></th>
              <th width="1"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(request, index) in requests.Items" :key="index">
              <td></td>
              <td class="fw-600 color-N-900">{{ request.Name }}</td>
              <td>{{ request.Vat }}</td>
              <td>{{ $utils.FormatDateDMY(request.RequestDate) }}</td>
              <td>{{ request.Country }}</td>
              <td>
                <a :href="request.UrlDomain" target="_blank">{{
                  request.UrlDomain.includes('://') ? Split(request.UrlDomain) : request.UrlDomain
                }}</a>
              </td>
              <td>
                <div>
                  {{ request.ContactEmail }}<br />
                  <h6 class="fs-13px fw-600 mt-2">Name: {{ request.ContactName }}</h6>
                </div>
              </td>
              <td>
                <div class="d-flex gap-2 justify-content-end pe-3">
                  <button
                    type="button"
                    @click="AnswerCompany(request.Id, false)"
                    class="btn btn-danger btn-sm"
                  >
                    Reject
                  </button>
                  <button
                    type="button"
                    @click="AnswerCompany(request.Id, true)"
                    class="btn btn-primary btn-sm"
                  >
                    Approve
                  </button>
                </div>
              </td>
              <td>
                <button
                  @click="OpenModalSeeCompanyRequests(request.Id)"
                  type="button"
                  class="btn p-0"
                >
                  <img src="@/assets/images/arrow-pagination.png" alt="" />
                </button>
              </td>
              <td></td>
            </tr>
          </tbody>
        </table>
      </div>

      <LoaderTableComponent v-if="!loaders.areRequestsLoaded"></LoaderTableComponent>
      <NotFoundComponent
        text="No company found"
        v-if="loaders.areRequestsLoaded && requests.Items.length == 0"
      ></NotFoundComponent>
      <div
        class="py-4 mx-4 d-flex align-items-center justify-content-center justify-content-sm-between wrap"
      >
        <h6 v-if="requests.Count > 0" class="color-grey fw-600 mb-3 mb-sm-0">
          Showing
          {{ filter.PageSize > requests.Count ? requests.Count : requests.Items.length }}
          of {{ requests.Count }}
          {{ requests.Count !== 1 ? 'entries' : 'entry' }}
        </h6>
        <div class="d-flex align-items-center gap-4">
          <PaginationComponent
            @pagechanged="GetRequests"
            :totalPages="requests.NumberOfPages"
            :currentPage="filter.PageNumber"
          ></PaginationComponent>
        </div>
      </div>
    </div>
  </div>
  <SeeCompanyRequestModalComponent :request="selectedRequest"></SeeCompanyRequestModalComponent>
</template>
<script>
import PaginationComponent from '@/components/Others/PaginationComponent.vue'
import OrderByComponent from '@/components/Others/OrderByComponent.vue'
import NotFoundComponent from '@/components/Others/NotFoundComponent.vue'
import LoaderTableComponent from '@/components/Others/loaders/LoaderTableComponent.vue'
import SeeCompanyRequestModalComponent from '@/components/Modals/SeeCompanyRequestModalComponent.vue'
export default {
  name: 'Companies',
  components: {
    PaginationComponent,
    NotFoundComponent,
    OrderByComponent,
    LoaderTableComponent,
    SeeCompanyRequestModalComponent,
  },
  data() {
    return {
      filter: {
        PageSize: 6,
        PageNumber: 1,
        OrderBy: 'RegistrationDate_desc',
        SearchText: '',
        Country: null,
      },
      loaders: {},
      requests: {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      },
      countries: [],
      selectedRequest: null,
    }
  },
  methods: {
    OpenModalSeeCompanyRequests(id) {
      this.$store.commit('SET_LOADER', true)
      this.$axios
        .get(`/api/Administration/getRequestOrgansation?organisationId=${id}`)
        .then((response) => {
          this.$store.commit('SET_LOADER', false)
          this.selectedRequest = response.data
          $('#seeCompanyRequest').modal('show')
        })
        .catch(() => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    Split(text) {
      const domain = text.split('://')
      return domain[1]
    },
    ResetFilters() {
      this.filter.SearchText = null
      this.filter.Country = null
      this.GetRequests()
    },
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
      this.GetRequests()
    },
    GetCountries() {
      this.countries = []
      this.loaders.loaderMultiselectCountries = true
      //add method
      this.$axios
        .get(`/api/Organisation/getCountries`)
        .then((response) => {
          this.loaders.loaderMultiselectCountries = false
          this.countries = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectCountries = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetRequests(page) {
      this.loaders.areRequestsLoaded = false
      this.filter.PageNumber = 1
      if (page) {
        this.filter.PageNumber = page
      }
      this.requests = {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      }
      const body = {
        PageSize: this.filter.PageSize,
        PageNumber: this.filter.PageNumber,
        OrderBy: this.filter.OrderBy,
        ...(this.filter.SearchText && { SearchText: this.filter.SearchText }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
      }
      this.$axios
        .post(`/api/Administration/getRequestsOrganisations`, body)
        .then((response) => {
          this.requests = response.data
          this.loaders.areRequestsLoaded = true
        })
        .catch(() => {
          this.loaders.areRequestsLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    AnswerCompany(id, value) {
      const answer = {
        organisationId: id,
        IsAprrove: value,
      }

      this.$swal
        .fire({
          title: value == true ? 'Are you sure you want to accept this request?' : 'Are you sure you want to reject this request?',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#1B7DF5',
          cancelButtonColor: '#DF0C3D',
          confirmButtonText: 'Yes',
        })
        .then((result) => {
          if (result.isConfirmed) {
            this.$store.commit('SET_LOADER', true)
            this.$axios
              .post(`/api/Administration/respondRequest`, answer)
              .then((response) => {
                if (value == true) {
                  this.$utils.ToastNotify('success', 'The company request has been aproved')
                } else {
                  this.$utils.ToastNotify('success', 'The company request has been rejected')
                }
                      this.$store.commit('SET_LOADER', false)
                this.GetRequests()
              })
              .catch(() => {
                      this.$store.commit('SET_LOADER', false)
                this.$utils.ToastNotifySomethingWentWrong()
              })
          }
        })
    },
  },
  created() {
    this.GetRequests()
  },
}
</script>
<style scoped>
.table-responsive .table {
  min-width: 1240px;
}</style>
