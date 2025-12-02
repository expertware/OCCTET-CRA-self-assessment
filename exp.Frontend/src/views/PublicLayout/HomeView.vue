<template>
  <MenuComponent @scroll="ScrollToSection" @scroll-mobile="ScrollToSectionMobile"></MenuComponent>
  <HeroSectionComponent @scroll="ScrollToSection" id="home"></HeroSectionComponent>

  <div class="bg-light-2">
    <AboutComponent></AboutComponent>
    <div id="register">
      <RegisterCompanyComponent></RegisterCompanyComponent>
    </div>
  </div>
  <div class="bg-grey">
    <SelfAssesmentComponent></SelfAssesmentComponent>
    <SurveyTypesComponent @scroll="ScrollToSection" id="surveys"></SurveyTypesComponent
    ><WhyRegisterComponent id="whyRegister"></WhyRegisterComponent>
    <FirstRegistrationComponent @scroll="ScrollToSection"></FirstRegistrationComponent>
    <FrequentlyAskedComponent id="faq"></FrequentlyAskedComponent>
    <ContactUsComponent id="contact"></ContactUsComponent>
    <PartnersComponent></PartnersComponent>
  </div>
</template>
<script>
import MenuComponent from '@/components/Others/MenuComponent.vue'
import HeroSectionComponent from '@/components/Others/HomePage/HeroSectionComponent.vue'
import AboutComponent from '@/components/Others/HomePage/AboutComponent.vue'
import RegisterCompanyComponent from '@/components/Others/HomePage/RegisterCompanyComponent.vue'
import SelfAssesmentComponent from '@/components/Others/HomePage/EuCyberResilienceComponent.vue'
import SurveyTypesComponent from '@/components/Others/HomePage/SurveyTypesComponent.vue'
import PartnersComponent from '@/components/Others/HomePage/PartnersComponent.vue'
import FirstRegistrationComponent from '@/components/Others/HomePage/FirstRegistrationComponent.vue'
import FrequentlyAskedComponent from '@/components/Others/HomePage/FrequentlyAskedComponent.vue'
import WhyRegisterComponent from '@/components/Others/HomePage/WhyRegisterComponent.vue'
import ContactUsComponent from '@/components/Others/HomePage/ContactUsComponent.vue'

export default {
  name: '',
  components: {
    MenuComponent,
    HeroSectionComponent,
    AboutComponent,
    RegisterCompanyComponent,
    SurveyTypesComponent,
    PartnersComponent,
    FirstRegistrationComponent,
    WhyRegisterComponent,
    FrequentlyAskedComponent,
    SelfAssesmentComponent,
    ContactUsComponent,
  },
  data() {
    return {
      codeOrganisation: null,
      organisationVat: null,
      emailRequest: null,
    }
  },
  methods: {
    ScrollToSection(id) {
      const element = document.getElementById(id)
      if (element) {
        const rect = element.getBoundingClientRect()
        const offset = rect.top + window.pageYOffset
        const centerY = offset - window.innerHeight / 2 + element.offsetHeight / 2
        window.scrollTo({
          top: centerY,
          behavior: 'smooth',
        })
      }
    },
    ScrollToSectionMobile(id) {
      const element = document.getElementById(id)
      if (element) {
        const rect = element.getBoundingClientRect()
        const offset = rect.top + window.pageYOffset - 120
        
        window.scrollTo({
          top: offset,
          behavior: 'smooth',
        })
      }
    },
  },
}
</script>
<style scoped>
.bg-light-2 {
  background-image: url('@/assets/images/bg-light-2.png');
  background-size: cover;
  background-repeat: repeat;
  min-height: 1786px;
}
.bg-grey {
  background-color: #f6f7f9;
}
</style>
