<template>
  <div class="container-lg mt-4 p-se-20-px">
   
      <h2 class="color-N-900 fw-600 roboto-cond mb-4">Companies</h2>
    
    <div class="white-box mb-3">
      <div class="row gutter-12-px">
        <div class="col-12 col-md-6 col-lg-4 col-xl">
          <label for="search-text-companies" class="form-label">Search companies by name or vat</label>
          <div class="input-group flex-nowrap">
            <span class="input-group-text"
              ><img src="@/assets/images/search.svg" alt="search" /></span
            ><input
              v-model="filter.SearchText"
              class="form-control"
              type="text"
              id="search-text-companies"
              placeholder="Search companies"
              aria-describedby="search-company"
              autocomplete="off"
              @keydown.enter="GetCompanies()"
            />
          </div>
        </div>
        <div class="col-12 col-md-6 col-lg-4 col-xl mt-3 mt-md-0 ">
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
              track-by="Id"
              :allow-empty="false"
              :loading="loaders.loaderMultiselectCountries"
              @select="GetCompanies()"
              @open="GetCountries()"
            ></multiselect>
            <img
              v-if="filter.Country != null"
              src="/src/assets/images/x-reset.svg"
              alt=""
              class="button-reset-multiselect"
              @click="((filter.Country = null), GetCompanies())"
            />
          </div>
        </div>
        <div class="col-12 col-md-6 col-lg-4 col-xl mt-3 mt-md-0">
          <div class="position-relative">
            <label for="search-company-size-companies" class="form-label">Company size</label>
            <multiselect
              class="custom-multiselect"
              v-model="filter.CompanySize"
              :options="companySizes"
              placeholder="Select size"
              :allow-empty="false"
              selected-label=""
              deselect-label=""
              select-label=""
              @select="GetCompanies()"
            ></multiselect>

            <img
              v-if="filter.CompanySize != null"
              src="/src/assets/images/x-reset.svg"
              alt=""
              class="button-reset-multiselect"
              @click="((filter.CompanySize = null), GetCompanies())"
            />
          </div>
        </div>
        <div class="col-auto mt-3 mt-xl-0">
          <div class="h-100 d-flex align-items-end gap-2">
            <button @click="GetCompanies()" type="button" class="btn action">
              <img src="@/assets/images/filter.svg" alt="" />
            </button>
            <button @click="ResetFilters()" type="button" class="btn action">
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
                  ><OrderByComponent
                    value="RegistrationDate"
                    :orderBy="filter.OrderBy"
                  ></OrderByComponent
                  >Registration date</span
                >
              </th>
              <th>Country</th>
              <th>Size</th>
              <th width="280"></th>
              <th></th>
              <th width="1"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(company, index) in companies.Items" :key="index">
              <td></td>
              <td class="fw-600 color-N-900">
                <router-link :to="{ name: 'surveys-admin', params: { companyId: company.Id, companyName: company.Name } }">
                  {{ company.Name }}</router-link
                >
              </td>
              <td>{{ company.Vat }}</td>
              <td>{{ $utils.FormatDateDMY(company.CreatedAt) }}</td>
              <td>{{ company.Country }}</td>
              <td>{{ company.CompanySize }}</td>

              <td>
                <div class="d-flex gap-2 justify-content-end pe-3">
                  <button
                    @click="DeleteCompany(company.Id, company.Name)"
                    type="button"
                    class="btn btn-danger btn-sm"
                  >
                    Delete
                  </button>
                  <button
                    @click="ResendCode(company.Id)"
                    type="button"
                    class="btn btn-secondary btn-sm"
                  >
                    Resend code
                  </button>
                </div>
              </td>
              <td>
                <router-link :to="{ name: 'surveys-admin', params: { companyId: company.Id, companyName: company.Name } }"
                  ><img src="@/assets/images/arrow-pagination.png" alt=""
                /></router-link>
              </td>

              <td></td>
            </tr>
          </tbody>
        </table>
      </div>

      <LoaderTableComponent v-if="!loaders.areCompaniesLoaded"></LoaderTableComponent>
      <NotFoundComponent
        text="No company found"
        v-if="loaders.areCompaniesLoaded && companies.Items.length == 0"
      ></NotFoundComponent>
      <div
        class=" py-4 mx-4 d-flex align-items-center justify-content-center justify-content-sm-between wrap"
      >
        <h6 v-if="companies.Count > 0" class="color-grey fw-600 mb-3 mb-sm-0">
          Showing
          {{ filter.PageSize > companies.Count ? companies.Count : companies.Items.length }}
          of {{ companies.Count }}
          {{ companies.Count !== 1 ? 'entries' : 'entry' }}
        </h6>
        <div class="d-flex align-items-center gap-4">
          <PaginationComponent
            @pagechanged="GetCompanies"
            :totalPages="companies.NumberOfPages"
            :currentPage="filter.PageNumber"
          ></PaginationComponent>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import PaginationComponent from '@/components/Others/PaginationComponent.vue'
import OrderByComponent from '@/components/Others/OrderByComponent.vue'
import NotFoundComponent from '@/components/Others/NotFoundComponent.vue'
import LoaderTableComponent from '@/components/Others/loaders/LoaderTableComponent.vue'
export default {
  name: 'Companies',
  components: {
    PaginationComponent,
    NotFoundComponent,
    OrderByComponent,
    LoaderTableComponent,
  },
  data() {
    return {
      filter: {
        PageSize: 10,
        PageNumber: 1,
        OrderBy: 'Name',
        SearchText: '',
        Country: null,
        CompanySize: null,
        CRAStatus: null,
      },
      loaders: {},
      companies: {
        NumberOfPages: 1,
        Count: null,
        Items: [],
      },
      countries: [],
      companySizes: ['Micro (1–9)', 'Small (10–49)', 'Medium (50–249)', 'Large (250+)'],
      craStatuses: [
        { Name: 'Yes', Value: true },
        { Name: 'No', Value: false },
      ],
    }
  },
  methods: {
    ResendCode(companyId) {
      this.$store.commit('SET_LOADER', true)
      this.$axios
        .post(`/api/Administration/resendCode?organisationId=${companyId}`)
        .then((response) => {
          this.GetCompanies()
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotify('success', 'The code was resent')
        })
        .catch((error) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    DeleteCompany(companyId, name) {
      this.$swal
        .fire({
          title: `Are you sure you want to delete the company ${name}?`,
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
              .delete(`/api/Administration/delete?organisationId=${companyId}`)
              .then((response) => {
                this.GetCompanies()
                this.$store.commit('SET_LOADER', false)
                this.$utils.ToastNotify('success', 'Organisation has been deleted')
              })
              .catch((error) => {
                this.$store.commit('SET_LOADER', false)
                this.$utils.ToastNotifySomethingWentWrong()
              })
          }
        })
    },
    ResetFilters() {
      this.filter.SearchText = ''
      this.filter.Country = null
      this.filter.CompanySize = null
      this.filter.CRAStatus = null
      this.GetCompanies()
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
      this.GetCompanies()
    },
    GetCountries() {
      this.countries = []
      this.loaders.loaderMultiselectCountries = true
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

    GetCompanies(page) {
      this.loaders.areCompaniesLoaded = false
      this.filter.PageNumber = 1
      if (page) {
        this.filter.PageNumber = page
      }
      this.companies = {
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
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
      }
      this.$axios
        .post(`/api/Administration/getOrganisations`, body)
        .then((response) => {
          this.companies = response.data
          this.loaders.areCompaniesLoaded = true
        })
        .catch(() => {
          this.loaders.areCompaniesLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  created() {
    this.GetCompanies()
  },
}
</script>
<style scoped>
a {
  color: var(--neutral-900);
  transition: all 0.3s
}
a:hover {
  color: var(--neutral-700);
}
</style>
