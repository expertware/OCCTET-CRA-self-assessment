import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap'
import store from './store/vuex'
import utils from './utils/utils'
import accessCode from './localStorage/accessCode'
import moment from 'moment'

import recaptchaPlugin from './plugins/recaptcha'
import VueDOMPurifyHTML from 'vue-dompurify-html'

import Multiselect from 'vue-multiselect'
import 'vue-multiselect/dist/vue-multiselect.css'

import VueSweetalert2 from 'vue-sweetalert2'
import 'sweetalert2/dist/sweetalert2.min.css'
import VueApexCharts from 'vue3-apexcharts'

import VueDatePicker from '@vuepic/vue-datepicker'
import '@vuepic/vue-datepicker/dist/main.css'

import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import { $axios } from './axios/axios-config'

const app = createApp(App)

app.use(router)
app.use(store)
app.use(recaptchaPlugin)
app.use(VueDOMPurifyHTML, {
  default: {
    allowedTags: [
      'p',
      'br',
      'strong',
      'em',
      'code',
      'pre',
      'i',
      'ul',
      'ol',
      'li',
      'div',
      'h1',
      'h2',
      'h3',
      'h4',
      'h5',
      'h6',
      'title'
    ],
    allowedAttributes: {},
  },
})
app.component('Multiselect', Multiselect)
app.use(VueSweetalert2)
app.use(VueApexCharts)
app.component('VueDatePicker', VueDatePicker)

app.config.globalProperties.$utils = utils
app.config.globalProperties.$moment = moment
app.config.globalProperties.$accessCode = accessCode
app.config.globalProperties.$axios = {
  ...$axios,
}

app.mount('#app')
