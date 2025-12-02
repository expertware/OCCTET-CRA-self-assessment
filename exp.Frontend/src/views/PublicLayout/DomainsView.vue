<template>
  <MenuSecondaryPage></MenuSecondaryPage>
  <div>
    <div class="hero-bg">
      <div class="pt-104-px container-md">
        <div class="row">
          <div class="col-12 col-xl-6 z-index-1">
            <h1 class="title">Activity sectors</h1>
            <h3 class="subtitle">
              The purpose of this survey is to understand the challenges and needs SMEs face in
              complying with the EU Cyber Resilience Act (CRA). The CRA mandates cybersecurity
              requirements for products with digital elements, including software and hardware. Your
              responses will help develop tools and resources to simplify compliance.
            </h3>
            <div>
              <div class="col-12 col-sm position-relative">
                <input
                  v-model="filter.SearchText"
                  type="text"
                  class="input-opaque w-100 ps-5"
                  placeholder="Search"
                  aria-label="Search"
                  aria-describedby="Search"
                  @keydown.enter="GetDomains()"
                />
                <button type="button" class="btn arrow" @click="GetDomains()"></button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <img class="hero-bg-curve" src="@/assets/images/hero-bg-curve.webp" alt="" />
    </div>
    <div class="container-md margin">
      <div v-if="loaders.areDomainsLoaded && domains.length == 0" class="white-box">
        <NotFoundComponent text="Domains not found" class="custom-not-found"></NotFoundComponent>
      </div>

      <div class="white-box mt-3" v-for="(domain, index) in domains">
        <p>
          <button
            class="btn-expand"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="`#collapse${index}`"
            aria-expanded="false"
            aria-controls="collapseExample"
          >
            {{ domain.Name }}
            <img class="arrow-collapse" src="@/assets/images/collapse-up.svg" alt="" />
          </button>
        </p>
        <div class="collapse show" :id="`collapse${index}`">
          <div class="card card-body">
            <div class="" v-for="(subsector, index) in domain.SubSector">
              <h4 class="color-N-900 fw-600 mb-12-px">{{ subsector.Name }}</h4>
              <h6 class="mb-4">{{ subsector.Description }}</h6>
            </div>
          </div>
        </div>
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
        SearchText: '',
      },
      domains: [
        
      ],
      loaders: {},
    }
  },
  methods: {
    GetDomains() {
      this.loaders.areDomainsLoaded = false
      this.$axios
        .get(`/api/Organisation/getActivitySectorsDetails?searchText=${this.filter.SearchText}`)
        .then((response) => {
          this.domains = response.data
          this.loaders.areDomainsLoaded = true
        })
        .catch(() => {
          this.loaders.areDomainsLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
  },
  created() {
    this.GetDomains()
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
  width: 45px;
  background-repeat: no-repeat;
  transition: all 0.3s;
}
.btn.arrow {
  border-left: none;
  
}
.btn.arrow::before {
  content: '';
  position: absolute;
  background-image: none;
}
.input-opaque {
  padding: 15px 15px 15px 60px !important;
}
.container-md.margin {
  margin-top: -100px;
  margin-bottom: 36px;
}

.white-box {
  position: relative;
  z-index: 2;
}
.card {
  border: none;
  border-radius: 0px;
  border-bottom-right-radius: 16px;
  border-bottom-left-radius: 16px;
}
.card-body {
  padding: 0px 16px;
}
.btn-expand {
  background-color: #ffffff;
  font-size: 24px;
  font-weight: 600;
  text-transform: uppercase;
  width: 100%;
  text-align: start;
  padding: 24px 16px;
  border-radius: 16px;
  color: var(--neutral-900);
  border: none;
}

.arrow-collapse {
  position: absolute;
  right: 24px;
  top: 30px;
  transition: all 0.3s;
}

.btn-expand.collapsed .arrow-collapse {
  transform: rotate(180deg);
}
.btn-expand:active {
  border: none;
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
