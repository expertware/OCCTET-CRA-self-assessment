<template>
  <div class="bg-white">
    <div class="bg-points">
      <div class="container position-relative">
        <div class="top-filters">
          <div class="row">
            <div class="col-12 col-sm-6 col-md mb-3 mb-md-0">
              <div class="custom-date-picker">
                <label class="form-label">Date interval</label>

                <VueDatePicker
                  v-model="filter.Interval"
                  format="dd/MM/yyyy"
                  auto-apply
                  utc
                  range
                  multi-calendars
                  :enable-time-picker="false"
                  placeholder="Select time interval"
                  @update:model-value="GetCompaniesData()"
                ></VueDatePicker>
              </div>
            </div>
            <div class="col-12 col-sm-6 col-md mb-3 mb-md-0">
              <div class="position-relative">
                <label for="search-company-size-companies" class="form-label">Company size</label>
                <multiselect
                  class="custom-multiselect"
                  v-model="filter.CompanySize"
                  :options="companySizes"
                  placeholder="Select size"
                  selected-label=""
                  :allow-empty="false"
                  deselect-label=""
                  select-label=""
                  @select="GetCompaniesData()"
                ></multiselect>

                <img
                  v-if="filter.CompanySize != null"
                  src="/src/assets/images/x-reset.svg"
                  alt=""
                  class="button-reset-multiselect"
                  @click="((filter.CompanySize = null), GetCompaniesData())"
                />
              </div>
            </div>
            <div class="col-12 col-sm-6 col-md mb-3 mb-md-0">
              <div class="position-relative">
                <label for="search-country-companies" class="form-label">Country</label>
                <multiselect
                  class="custom-multiselect"
                  v-model="filter.Country"
                  :options="countriesMultiselect"
                  placeholder="Search countries"
                  selected-label=""
                  deselect-label=""
                  select-label=""
                  label="Name"
                  :allow-empty="false"
                  track-by="Id"
                  :loading="loaders.loaderMultiselectCountries"
                  @select="GetCompaniesData()"
                  @open="GetCountriesMultiselect()"
                ></multiselect>
                <img
                  v-if="filter.Country != null"
                  src="/src/assets/images/x-reset.svg"
                  alt=""
                  class="button-reset-multiselect"
                  @click="((filter.Country = null), GetCompaniesData())"
                />
              </div>
            </div>
            <div class="col-12 col-sm-6 col-md">
              <div class="position-relative">
                <label for="search-country-companies" class="form-label">Sector</label>
                <multiselect
                  class="custom-multiselect"
                  v-model="filter.ActivitySector"
                  :options="activitySectors"
                  placeholder="Search sector"
                  selected-label=""
                  deselect-label=""
                  select-label=""
                  label="Name"
                  :allow-empty="false"
                  track-by="Id"
                  :loading="loaders.loaderMultiselectActivitySectors"
                  @select="GetCompaniesData()"
                  @open="GetActivitySectorsMultiselect()"
                ></multiselect>
                <img
                  v-if="filter.ActivitySector != null"
                  src="/src/assets/images/x-reset.svg"
                  alt=""
                  class="button-reset-multiselect"
                  @click="((filter.ActivitySector = null), GetCompaniesData())"
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="container-fluid">
        <div ref="chart" class="chart-container"></div>
      </div>
      <div class="container position-relative">
        <div class="row absolute-row">
          <div class="col-12 col-xl-6 mb-4 mb-xxl-0">
            <div class="white-box medium px-0">
              <h4 class="px-4 mb-12-px">Countries Summary</h4>
              <LoaderReportCountriesComponent
                v-if="!areCountriesLoaded && this.countries.length == 0"
              >
              </LoaderReportCountriesComponent>
              <div v-if="countries.length > 0" class="wrapper">
                <div
                  @click="HighlightCountry(item.name)"
                  v-for="(item, index) in countries"
                  class="item"
                  :class="{ selected: item.name == selectedCountryName }"
                >
                  <div>
                   
                    <img :src="ShowDynamicImageFlag('', item.iso)" alt="flag" class="img-flag" />
                    {{ item.name }}
                  </div>
                  <span class="value">{{ item.value }}</span>
                </div>
              </div>
              <div v-else class="w-100 flex gap-1 flex-column">
                <img src="@/assets/images/not-found.webp" alt="" />
                <h4>No result</h4>
              </div>
            </div>
          </div>
          <div class="col-12 col-xl-6 h-100">
            <div class="white-box medium pb-0">
              <h4 class="mb-12-px">Company size</h4>
              
              <DonutChartComponent
                :companySizes="donutChartData"
                v-if="isMapLoaded"
              ></DonutChartComponent>
            </div>
          </div>
          <div class="col-12 col-xl-4 h-100 mt-4">
            <div class="white-box small">
              <h4 class="fs-20px fw-600 color-N-900 mb-34-px">CRA Regulation Apply</h4>
              
              <BarChartComponent
                :answers="craRegulationApply"
                v-if="isMapLoaded"
              ></BarChartComponent>
              <h6 class="fs-13px color-N-400">
                * CRA Regulation Apply indicates whether an institution is subject to CRA
                requirements
              </h6>
            </div>
          </div>
          <div class="col-12 col-xl-4 h-100 mt-4">
            <div class="white-box small">
              <h4 class="fs-20px fw-600 color-N-900 mb-34-px">Awareness Survey</h4>
              
              <BarChartComponent :answers="awarenessSurvey" v-if="isMapLoaded"></BarChartComponent>
              <h6 class="fs-13px color-N-400">
                * Survey can be completed either without an account (anonymous user) or with an
                account
              </h6>
            </div>
          </div>
          <div class="col-12 col-xl-4 h-100 my-4">
            <div class="white-box small">
              <h4 class="fs-20px fw-600 color-N-900 mb-34-px">Awareness of The CRA</h4>
              
              <BarChartComponent :answers="awarenessCra" v-if="isMapLoaded"></BarChartComponent>
            </div>
          </div>
         
          <div class="col-12 mb-4">
            <BubbleChartComponent :bubbleData="bubbleChartData"></BubbleChartComponent>
          </div>
          <div class="col-12">
            <RadialChartComponent
              v-if="isMapLoaded"
              :report="maturityCategories"
            ></RadialChartComponent>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import moment from 'moment'
import * as echarts from 'echarts'
import statesData from '../../utils/customgeo.json'
import PieChartComponent from '@/components/Others/Reports/PieChartComponent.vue'
import BarChartComponent from '@/components/Others/Reports/BarChartComponent.vue'
import DonutChartComponent from '@/components/Others/Reports/DonutChartComponent.vue'
import LoaderPieChartComponent from '@/components/Loaders/LoaderPieChartComponent.vue'
import LoaderReportCountriesComponent from '@/components/Loaders/LoaderReportCountriesComponent.vue'
import LoaderBarChartComponent from '@/components/Loaders/LoaderBarChartComponent.vue'
import LoaderDonutComponent from '@/components/Loaders/LoaderDonutComponent.vue'
import RadialChartComponent from '@/components/Others/Reports/RadialChartComponent.vue'
import BubbleChartComponent from '@/components/Others/Reports/BubbleChartComponent.vue'
export default {
  name: 'MapChart',
  components: {
    PieChartComponent,
    BarChartComponent,
    DonutChartComponent,
    LoaderPieChartComponent,
    LoaderReportCountriesComponent,
    LoaderBarChartComponent,
    LoaderDonutComponent,
    RadialChartComponent,
    BubbleChartComponent,
  },
  data() {
    return {
      maxValueCountries: null,
      isMapLoaded: true,
      myChart: null,
      selectedCountryName: null,
      countries: [],
      filter: {
        Interval: null,
        CompanySize: null,
        Country: null,
        ActivitySector: null,
      },
      loaders: {},
      countriesMultiselect: [],
      companySizes: ['Micro (1–9)', 'Small (10–49)', 'Medium (50–249)', 'Large (250+)'],
      activitySectors: [],
      donutChartData: null,
      maturityCategories: {},
      craRegulationApply: null,
      awarenessSurvey: null,
      awarenessCra: null,
      bubbleChartData: null,
      areCountriesLoaded: false,
    }
  },
  mounted() {
    this.GetCompaniesData()
    window.addEventListener('resize', this.HandleResize)
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.HandleResize)
  },
  methods: {
    HandleResize() {
      if (this.myChart) {
        this.myChart.resize()
      }
    },
    ShowDynamicImageFlag(imagePath, defaultImg) {
      if (!imagePath) {
        return new URL(`/src/assets/images/countries-icons/${defaultImg}.webp`, import.meta.url)
          .href
      }
      return imagePath
    },
    async InitialiseWorldMapChart(countriesData, worldStates) {
      this.myChart = echarts.init(this.$refs.chart)
      this.myChart.showLoading()
      const worldJson = statesData
      echarts.registerMap('USA', worldJson)
      this.myChart.hideLoading()
      const option = {
        title: {
          text: 'Surveys data',
          subtext: 'Data from occtet',
          left: 'right',
          show: false,
        },
        tooltip: {
          trigger: 'item',
          borderColor: '#ffffff',
          borderWidth: 1,
          textStyle: {
            color: 'rgba(20, 24, 31, 1)',
            fontSize: 15,
          },
          extraCssText:
            'box-shadow: 0px 0px 10px rgba(0,0,0,0.3); padding: 10px; border-radius: 8px;',
          formatter: function (params) {
            const name = params.name
            const value = params.value
            const isoCode = worldStates[params.name]?.toLowerCase() || 'xx'
            const flagUrl = new URL(
              `/src/assets/images/countries-icons/${isoCode}.webp`,
              import.meta.url,
            ).href

            return `
      <div style="display: flex; align-items: center;">
        <img src="${flagUrl}" alt="${name} flag" style="width: 30px; height: 30px; margin-right: 10px; border-radius:50%; object-fit:cover; border:1px solid #B6C1CA" />
        <div>
          <strong class="color-P-900">${name}</strong>
           <span style="
          margin-left: 16px;
          background-color: #0066b3;
          color: white;
          border-radius: 8px;
          padding: 4px 10px;
          font-weight: bold;
        ">${value ? value.toLocaleString() : '0'}</span>
        </div>
      </div>
    `
          },
        },
        visualMap: {
          show: false,
          min: 0,
          max: this.maxValueCountries,
          left: 'right',
          text: ['High', 'Low'],
          calculable: true,
          inRange: {
            color: ['#57B5FE', '#0081C5', '#005581'],
          },
        },
        series: [
          {
            name: 'World map',
            type: 'map',
            roam: false,
            map: 'USA',
            selectedMode: false,
            emphasis: {
              label: {
                show: true,
              },
              itemStyle: {
                areaColor: '#83E5F7', //hover
              },
            },

            itemStyle: {
              areaColor: '#57B5FE', //default
              borderColor: '#ffffff',
              borderWidth: 2,
            },
            data: countriesData,
          },
        ],
      }

      this.myChart.setOption(option)
      this.isMapLoaded = true
    },
    HighlightCountry(name) {
      if (this.selectedCountryName && this.selectedCountryName !== name) {
        this.myChart.dispatchAction({
          type: 'downplay',
          name: this.selectedCountryName,
        })
      }
      this.myChart.dispatchAction({
        type: 'highlight',
        name: name,
      })
      this.selectedCountryName = name
    },
    GetCountriesMultiselect() {
      this.countriesMultiselect = []
      this.loaders.loaderMultiselectCountries = true
      this.$axios
        .get(`/api/Organisation/getCountries`)
        .then((response) => {
          this.loaders.loaderMultiselectCountries = false
          this.countriesMultiselect = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectCountries = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetActivitySectorsMultiselect() {
      this.activitySectors = []
      this.loaders.loaderMultiselectActivitySectors = true
      this.$axios
        .get(`/api/Organisation/getActivitySectors`)
        .then((response) => {
          this.loaders.loaderMultiselectActivitySectors = false
          this.activitySectors = response.data
        })
        .catch(() => {
          this.loaders.loaderMultiselectActivitySectors = false
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },

    GetCompaniesData() {
      this.$store.commit('SET_LOADER', true)
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.GetOrganisationsBySize()
      this.GetCraRegulationApply()
      this.GetAwarenessSurvey()
      this.GetAwarenessTheCra()
      this.GetBubbleChartData()
      this.GetMaturityCategories()
      this.$axios
        .post(`/api/Report/getOrganisationsOnCountries`, searchParams)
        .then((response) => {
          const dataCountries1 = response.data
          const mappedDataCountries = dataCountries1.map((item) => ({
            name: item.Name,
            value: item.Count,
          }))
          this.maxValueCountries = Math.max(...dataCountries1.map((a) => a.Count))

          const worldJson = statesData
          const nameToIsoMap = {}
          worldJson.features.forEach((f) => {
            const name = f.properties.name
            const iso_a2 = f.properties.iso_a2.toLowerCase()
            nameToIsoMap[name] = iso_a2
          })

          const countriesWithIso = mappedDataCountries.map((country) => ({
            ...country,
            iso: nameToIsoMap[country.name]?.toLowerCase() || null,
          }))
          this.InitialiseWorldMapChart(mappedDataCountries, nameToIsoMap)
          this.countries = countriesWithIso
          this.areCountriesLoaded = true

          this.$store.commit('SET_LOADER', false)
        })
        .catch((error) => {
          this.$store.commit('SET_LOADER', false)
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetMaturityCategories() {
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.$axios
        .post(`/api/Report/getMaturityCategories`, searchParams)
        .then((response) => {
          this.maturityCategories = response.data
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetOrganisationsBySize() {
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.$axios
        .post(`/api/Report/getOrganizationsBySize`, searchParams)
        .then((response) => {
          this.donutChartData = response.data
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetCraRegulationApply() {
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.$axios
        .post(`/api/Report/getOrganisationCRARegulationApply`, searchParams)
        .then((response) => {
          this.craRegulationApply = response.data
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetAwarenessSurvey() {
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.$axios
        .post(`/api/Report/getAwarenessSurveyReport`, searchParams)
        .then((response) => {
          this.awarenessSurvey = response.data
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetAwarenessTheCra() {
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.$axios
        .post(`/api/Report/getAwarenessofTheCRA`, searchParams)
        .then((response) => {
          this.awarenessCra = response.data
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetBubbleChartData() {
      const searchParams = {
        ...(this.filter.Interval && {
          StartDate: this.filter.Interval[0],
        }),
        ...(this.filter.Interval && {
          EndDate: this.filter.Interval[1],
        }),
        ...(this.filter.Country && { CountryId: this.filter.Country.Id }),
        ...(this.filter.CompanySize && { CompanySize: this.filter.CompanySize }),
        ...(this.filter.ActivitySector && { ActivityDomainId: this.filter.ActivitySector.Id }),
      }
      this.$axios
        .post(`/api/Report/getOrganizationsRgistersOnTime`, searchParams)
        .then((response) => {
          const data = response.data
          this.bubbleChartData = data.map((item) => {
            const [day, month, year] = item.Name.split('/')
            const date = new Date(`${year}-${month}-${day}`)
            return [item.Count, moment(date).format('YYYY-MM-DD')]
          })
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
}
</script>

<style scoped>
.white-box {
  box-shadow: 0px 12px 64px 0px #64879e29;
  background: linear-gradient(
    180deg,
    rgba(255, 255, 255, 0.9025) 0%,
    rgba(255, 255, 255, 0.76) 100%
  );
  backdrop-filter: blur(7px);
  padding: 24px;
  border: 1px solid white;
}

.white-box.small {
  height: 220px;
}

.white-box.medium {
  height: 256px;
}

.white-box.question {
  padding: 24px 40px 24px 40px;
}

.white-box .wrapper {
  max-height: 189px;
  overflow-x: auto;
  scrollbar-color: #0168a3 transparent;
  scrollbar-width: thin;
}

.item {
  min-height: 38px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  cursor: pointer;
  font-size: 15px;
  color: var(--neutral-900);
  padding: 0px 24px;
  transition: all 0.3s;
  max-height: 256px;
  overflow-y: auto;
}

.item:hover {
  background-color: var(--neutral-50);
}

.item.selected {
  background: linear-gradient(90deg, rgba(94, 192, 255, 0.4) 0%, rgba(94, 192, 255, 0) 100%);
}

.value {
  background: var(--primary-200);
  color: var(--primary-600);
  padding: 2px 6px;
  height: 22px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 4px;
  font-size: 13px;
  font-weight: 700;
}

.item.selected .value {
  background: var(--primary-600);
  color: #ffffff;
}

.row {
  background-color: transparent;
}

.bg-white {
  background-color: var(--neutral-50) !important;
  overflow-x: hidden;
}

.bg-points {
  min-height: 250vh;
  /*background-image: url('@/assets/images/bg-points.png');*/
}

h4 {
  font-size: 20px;
  font-weight: 600;
  color: var(--neutral-900);
}

.top-filters {
  padding: 24px;
  border-radius: 16px;
  box-shadow: 0px 12px 64px 0px rgba(100, 135, 158, 0.16);
  background: linear-gradient(
    180deg,
    rgba(255, 255, 255, 0.9025) 0%,
    rgba(255, 255, 255, 0.76) 100%
  );
  backdrop-filter: blur(7px);
  position: relative;
  z-index: 4;
}

.chart-container {
  width: auto;
  min-height: 50vh;
}

.img-flag {
  width: 20px;
  height: 20px;
  margin-right: 10px;
  border-radius: 50%;
  object-fit: cover;
  border: 1px solid #b6c1ca;
}

@media (min-width: 1400px) {
  .absolute-row {
    position: absolute;
    width: 100%;
    z-index: 2;
    bottom: -1200px;
  }

  .chart-container {
    width: auto;
    min-height: 100vh;
  }

  .top-filters {
    position: fixed;
    top: 90px;
    right: inherit;
    width: inherit;
    max-width: 1295px;
    z-index: 4;
  }

  .container.position-relative {
    width: 100%;
    display: flex;
    justify-content: center;
  }
}
</style>
