import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/Layouts/PublicLayout.vue'
import token from '@/services/token.service'
import store from '../store/vuex'
import setBreadcrumbPage from './setBreadcrumbPages'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
      children: [
        {
          path: '/:pathMatch(.*)*',
          name: 'NoFoundPage',
          component: () => import('../views/PublicLayout/NotFoundView.vue'),
          meta: {
            breadCrumb(route) {
              return [
                {
                  text: 'Not found',
                  to: { name: 'NoFoundPage' },
                },
              ]
            },
          },
        },
        {
          path: '/',
          name: 'home',
          component: () => import('../views/PublicLayout/HomeView.vue'),
        },
        {
          path: '/confirm-email',
          name: 'confirm-email',
          component: () => import('../views/PublicLayout/ConfirmEmailView.vue'),
        },
        {
          path: '/accept-request',
          name: 'accept-request',
          component: () => import('../views/PublicLayout/AcceptAccessRequestView.vue'),
        },
        {
          path: '/coming-soon',
          name: 'coming-soon',
          component: () => import('../views/PublicLayout/ComingSoonView.vue'),
        },

        {
          path: '/not-found',
          name: 'not-found',
          component: () => import('../views/PublicLayout/NotFoundView.vue'),
        },
        {
          path: '/domains',
          name: 'domains',
          component: () => import('../views/PublicLayout/DomainsView.vue'),
        },
        {
          path: '/craDetails',
          name: 'craDetails',
          component: () => import('../views/PublicLayout/CRADetailsView.vue'),
        },
        {
          path: '/gdpr',
          name: 'gdpr',
          component: () => import('../views/PublicLayout/GDPRView.vue'),
        },
      ],
    },
    {
      path: '/auth',
      name: 'auth',
      component: () => import('../views/Layouts/AuthLayout.vue'),
      children: [
        {
          path: '/login',
          name: 'login',
          component: () => import('../views/AuthLayout/LoginView.vue'),
        },
        
        {
          path: '/forgot-password/:email?',
          name: 'forgot-password',
          component: () => import('../views/AuthLayout/SendLinkResetPassView.vue'),
        },
        {
          path: '/reset-password',
          name: 'reset-password',
          component: () => import('../views/AuthLayout/ResetPasswordView.vue'),
        },
      ],
    },
    {
      path: '/second',
      name: 'second',
      component: () => import('../views/Layouts/DashboardLayout.vue'),
      children: [
        {
          path: '/first-survey/:surveyId/:surveyName/',
          name: 'first-survey',
          component: () => import('../views/DashboardLayout/FirstSurveyView.vue'),
          meta: {
            breadCrumb(route) {
              return [
                {
                  text: route.params.surveyName,
                  to: {
                    name: 'first-survey',
                    params: {
                      surveyId: route.params.surveyId,
                      surveyName: route.params.surveyName,
                    },
                  },
                },
              ]
            },
          },
        },

        {
          path: '/survey/:surveyId/:surveyName/:organisationId?/:companyName?/:categorySurveyId',
          name: 'survey',
          component: () => import('../views/DashboardLayout/SurveyView.vue'),
          meta: {
            roles: ['Admin'],
            requiresAuth: true,
            requiresAccessCode: true,
            breadCrumb(route) {
              return setBreadcrumbPage.SurveyBreadcrumb(route)
            },
          },
        },

        {
          path: '/surveys',
          name: 'surveys',
          component: () => import('../views/DashboardLayout/SurveysView.vue'),
          meta: {
            requiresAccessCode: true,
            breadCrumb(route) {
              const surveyName = route.params.surveyName
              return [
                {
                  text: 'Surveys',
                  to: { name: 'surveys' },
                },
              ]
            },
          },
        },
        {
          path: '/surveys-admin/:companyId?/:companyName?',
          name: 'surveys-admin',
          component: () => import('../views/DashboardLayout/SurveysAdminView.vue'),
          meta: {
            roles: ['Admin'],
            requiresAuth: true,
            breadCrumb(route) {
              return [
                {
                  text: 'Companies',
                  to: { name: 'companies' },
                },
                {
                  text: route.params.companyName,
                  to: {
                    name: 'surveys-admin',
                    params: {
                      ...(route.params.companyId && { companyId: route.params.companyId }),
                      ...(route.params.companyName && { companyName: route.params.companyName }),
                    },
                  },
                },
              ]
            },
          },
        },
        {
          path: '/surveys-by-category/:surveyId/:categorySurvey/:organisationId?/:companyName?',
          name: 'surveys-by-category',
          component: () => import('../views/DashboardLayout/SurveysByCategoryView.vue'),
          meta: {
            roles: ['Admin'],
            requiresAuth: true,
            requiresAccessCode: true,
            breadCrumb(route) {
              return setBreadcrumbPage.SurveysByCategoryBreadcrumb(route)
            },
          },
        },
        {
          path: '/users',
          name: 'users',
          component: () => import('../views/DashboardLayout/UsersView.vue'),
          meta: {
            roles: ['Admin'],
            requiresAuth: true,
            breadCrumb(route) {
              return [
                {
                  text: 'Users',
                  to: { name: 'users' },
                },
              ]
            },
          },
        },
        {
          path: '/companies',
          name: 'companies',
          component: () => import('../views/DashboardLayout/CompaniesView.vue'),
          meta: {
            roles: ['Admin'],
            requiresAuth: true,
            breadCrumb(route) {
              return [
                {
                  text: 'Companies',
                  to: { name: 'companies' },
                },
              ]
            },
          },
        },
        {
          path: '/reports',
          name: 'reports',
          component: () => import('../views/DashboardLayout/ReportsView.vue'),
          meta: {
            roles: ['Admin', 'Reports Viewer'],
            requiresAuth: true,
            breadCrumb(route) {
              return [
                {
                  text: 'Reports',
                  to: { name: 'reports' },
                },
              ]
            },
          },
        },
        {
          path: '/requests',
          name: 'requests',
          component: () => import('../views/DashboardLayout/CompanyRequestsView.vue'),
          meta: {
            roles: ['Admin', 'Request Approver'],
            requiresAuth: true,
            breadCrumb(route) {
              return [
                {
                  text: 'Companies requests',
                  to: { name: 'requests' },
                },
              ]
            },
          },
        },
      ],
    },
    {
      path: '/coming-soon-occtet',
      name: 'coming-soon-occtet',
      component: () => import('../views/PublicLayout/ComingSoonOcctetView.vue'),
    },
  ],
  scrollBehavior() {
    return new Promise((resolve) => {
      resolve({ left: 0, top: 0 })
    })
  },
})

router.beforeEach(async (to, from, next) => {
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)
  const requiresAccessCode = to.matched.some((record) => record.meta.requiresAccessCode)
  token.getJwtToken()
  if (token.getJwtToken() != null) {
    await store.dispatch('getLoggedUser')
  }
  const user = store.getters.getUser
  if ((!user.UserRoles || user.UserRoles?.includes('Guest')) && to.name === 'requests') {
    return next({ path: '/login' })
  }

  if (!user.UserRoles?.includes('Guest') && requiresAccessCode && !requiresAuth) {
    return next({ path: '/' })
  }
  if (requiresAuth) {
   
    if (user?.UserRoles?.includes('Guest')) {
      const requiresAccessCode = to.matched.some((record) => record.meta.requiresAccessCode)
      if (requiresAccessCode && !user?.UserRoles.includes('Guest')) {
        return next({
          path: '/',
        })
      }
      if (requiresAccessCode && user?.UserRoles.includes('Guest')) {
        return next() // if guest auth, next
      }
      if (!requiresAccessCode) {
        return next({ path: '/' })
      }
      return next({
        path: '/',
      })
    }
    const matchedRouteWithRoles = to.matched.find((record) => record.meta?.roles)

    if (matchedRouteWithRoles) {
      const allowedRoles = matchedRouteWithRoles.meta.roles
      const userRoles = user.UserRoles || []

      const hasRole = userRoles.some((role) => allowedRoles.includes(role))
      if (!hasRole) {
        if (requiresAuth && requiresAccessCode) {
          // redirect to home
          return next({ path: '/' })
        }
        if (requiresAuth && !requiresAccessCode) {
          // redirect to login
          return next({ path: '/login' })
        }
        // default
        return next({ path: '/' })
      }
    }
    return next() //if user exists, next
  }
  return next()
})

export default router
