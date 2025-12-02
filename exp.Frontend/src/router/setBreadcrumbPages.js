import store from '@/store/vuex'

export default {
  SurveyBreadcrumb(route) {
    const surveyName = route.params.surveyName
    const crumbs = []
    switch (true) {
      case !store.state.user.UserRoles.includes('Guest'):
        crumbs.push(
          {
            text: 'Companies',
            to: { name: 'companies' },
          },
          {
            text: route.params.companyName,
            to: {
              name: route.params.organisationId ? `surveys-admin` : 'surveys',
              params: {
                ...(route.params.organisationId && {
                  companyId: route.params.organisationId,
                }),
                ...(route.params.companyName && { companyName: route.params.companyName }),
              },
            },
          },
          {
            text: route.params.surveyName + ' Surveys',
            to: {
              name: 'surveys-by-category',
              params: {
                surveyId: route.params.categorySurveyId,
                categorySurvey: route.params.surveyName,
                ...(route.params.organisationId && {
                  organisationId: route.params.organisationId,
                  ...(route.params.companyName && {
                    companyName: route.params.companyName,
                  }),
                }),
              },
            },
          },
          {
            text: surveyName,
            to: {
              name: 'survey',
              params: {
                surveyId: route.params.surveyId,
                surveyName: surveyName,
                ...(route.params.organisationId && {
                  organisationId: route.params.organisationId,
                }),
                ...(route.params.companyName && { companyName: route.params.companyName }),
                ...(route.params.categorySurveyId && {
                  categorySurveyId: route.params.categorySurveyId,
                }),
              },
            },
          },
        )
        return crumbs
      case store.state.user.UserRoles.includes('Guest'):
        crumbs.push(
          {
            text: 'Surveys',
            to: {
              name: 'surveys',
              params: {},
            },
          },
          {
            text: route.params.surveyName + ' Surveys',
            to: {
              name: 'surveys-by-category',
              params: {
                surveyId: route.params.categorySurveyId,
                categorySurvey: route.params.surveyName,
              },
            },
          },
          {
            text: surveyName,
            to: {
              name: 'survey',
              params: {
                surveyId: route.params.surveyId,
                surveyName: surveyName,
                categorySurveyId: route.params.categorySurveyId,
              },
            },
          },
        )
        return crumbs
    }
  },
  SurveysByCategoryBreadcrumb(route) {
    const categorySurvey = route.params.categorySurvey
    const crumbs = []
  if (route.params.organisationId) {
                crumbs.push(
                  {
                    text: 'Companies',
                    to: {
                      name: `surveys-admin`,
                      params: {
                        ...(route.params.organisationId && {
                          companyId: route.params.organisationId,
                        }),
                        ...(route.params.companyName && { companyName: route.params.companyName }),
                      },
                    },
                  },
                  {
                    text: route.params.companyName,
                    to: {
                      name: 'surveys-admin',
                      params: {
                        ...(route.params.organisationId && {
                          companyId: route.params.organisationId,
                        }),
                        ...(route.params.companyName && { companyName: route.params.companyName }),
                      },
                    },
                  },
                  {
                    text: categorySurvey + ' Surveys',
                    to: {
                      name: 'surveys-by-category',
                      params: {
                        surveyId: route.params.surveyId,
                        categorySurvey: categorySurvey,
                        ...(route.params.organisationId && {
                          organisationId: route.params.organisationId,
                          ...(route.params.companyName && {
                            companyName: route.params.companyName,
                          }),
                        }),
                      },
                    },
                  },
                )
              } else {
                crumbs.push(
                  {
                    text: 'Surveys',
                    to: {
                      name: 'surveys',
                      params: {},
                    },
                  },
                  {
                    text: categorySurvey + ' Surveys',
                    to: {
                      name: 'surveys-by-category',
                      params: {
                        surveyId: route.params.surveyId,
                        categorySurvey: categorySurvey,
                      },
                    },
                  },
                )
              }
              return crumbs
  },
}
