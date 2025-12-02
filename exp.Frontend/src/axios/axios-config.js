import axios from 'axios'
import router from '../router'
import tokenService from '../services/token.service'
import accessCode from '@/localStorage/accessCode'

const baseUrl = import.meta.env.VITE_BASE_URL
const axiosConfig = axios.create({
  baseURL: baseUrl,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
})

let isRefreshing = false
let subscribers = []

function onRefreshed(token) {
  subscribers.forEach((callback) => callback(token))
  subscribers = []
}

function addSubscriber(callback) {
  subscribers.push(callback)
}

axiosConfig.interceptors.request.use(
  async (config) => {
    const token = tokenService.getJwtToken()
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`
    }
    return config
  },
  (error) => Promise.reject(error),
)

axiosConfig.interceptors.response.use(
  async (response) => {
    return response
  },
  async (error) => {
    const originalConfig = error.config
    const excludedUrls = [`/api/Auth/login`, `/api/Auth/validateOTPFor2Factor`];
    if (originalConfig &&  !excludedUrls.includes(originalConfig.url) && error.response) {
      if (error.response.status == 401) {
        if (!isRefreshing) {
          isRefreshing = true
          const tokens = {
            AccessToken: tokenService.getJwtToken(),
            RefreshToken: tokenService.getRefreshToken(),
          }
          return await axios
            .post(baseUrl + `/api/Auth/refresh-token`, tokens)
            .then((response) => {
              //--clear local storage
              tokenService.setJwtToken(response.data.AccessToken)
              tokenService.setRefreshToken(response.data.RefreshToken)
              isRefreshing = false
              onRefreshed(response.data.AccessToken)
              return axiosConfig(originalConfig)
              // //we continue the original request with the new token
             
            })
            .catch((error) => {
              //remove old tokens
              if (axios.isCancel(error)) {
                
                isRefreshing = false
                return Promise.reject(error)
              }
              tokenService.removeAuthTokens()
              //if refreshToken is expired, user needs to login again
              accessCode.removeData()
              router.push({
                name: 'home',
              })

             
              isRefreshing = false
              return Promise.reject(_error)
            })
        }
        return new Promise((resolve) => {
          addSubscriber((token) => {
            originalConfig.headers['Authorization'] = 'Bearer ' + token
            resolve(axiosConfig(originalConfig))
          })
        })
      }
    }
    return Promise.reject(error)
  },
)

export const $axios = axiosConfig
