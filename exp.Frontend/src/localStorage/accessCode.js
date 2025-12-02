import router from '@/router'
let accessCode = {
  getAccessCode() {
    return JSON.parse(localStorage.getItem('accessCode'))
  },

  setAccessCode(code) {
    localStorage.setItem('accessCode', JSON.stringify(code))
  },

  removeData() {
    localStorage.removeItem('surveyId')
  },
}

export default accessCode
