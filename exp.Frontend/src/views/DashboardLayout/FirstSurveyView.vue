<template>
  <div class="container-fluid">
    <div class="buttons-mobile">
      <button
        type="button"
        v-if="annexes.length > 0"
        data-bs-toggle="offcanvas"
        data-bs-target="#offCanvasMobileAnnexes"
        aria-controls="offCanvasMobileAnnexes"
        class="d-xxl-none"
      >
        <img src="@/assets/images/anexes-icon-black.svg" alt="annexes" />
      </button>
      <button
        type="button"
        data-bs-toggle="offcanvas"
        data-bs-target="#offCanvasMobileProgress"
        aria-controls="offCanvasMobileProgress"
        class="mt-3 d-lg-none"
      >
        <img src="@/assets/images/list-icon.svg" alt="progress" />
      </button>
    </div>
    <div
      class="progress offcanvas offcanvas-bottom"
      tabindex="-1"
      id="offCanvasMobileProgress"
      aria-labelledby="offCanvasMobileProgressLabel"
    >
      <div class="offcanvas-header">
        <button
          type="button"
          class="btn-close text-reset"
          data-bs-dismiss="offcanvas"
          aria-label="Close"
        ></button>
      </div>
      <div class="offcanvas-body small" ref="offcanvasBody">
        <div
          v-for="(chapter, index) in chapters"
          :key="chapter.Id"
          @click="ScrollToSection(index + 1, 'mobile')"
        >
          <div class="chapter" :class="{ active: `section-survey-${index + 1}` == activeSection }">
            <div class="d-flex flex-column align-items-center">
              <StatusSurveyComponent width="24" :status="chapter.Status"></StatusSurveyComponent>
              <div class="vertical-bar" v-if="index + 1 != chapters.length"></div>
            </div>

            <h6 class="color-N-900 pb-4">{{ chapter.Name }}</h6>
          </div>
        </div>
      </div>
    </div>
    <div
      class="annex-mobile offcanvas offcanvas-bottom"
      tabindex="-1"
      id="offCanvasMobileAnnexes"
      aria-labelledby="offCanvasMobileAnnexesLabel"
    >
      <div class="offcanvas-header">
        <h5 class="flex gap-2" id="offcanvasRightLabel">
          <img src="@/assets/images/survey-annexes.svg" alt="annexes" />Annexes
        </h5>
        <button
          type="button"
          class="btn-close text-reset"
          data-bs-dismiss="offcanvas"
          aria-label="Close"
        ></button>
      </div>
      <div class="offcanvas-body" ref="offCanvasMobileAnnexesBody">
        <AnnexesComponent :copyAnnexes="annexes" :activeAnnex="activeAnnex"></AnnexesComponent>
      </div>
    </div>
    <div class="w-100 d-flex">
      <div class="left-menu d-none d-lg-block">
        <div class="sticky-menu">
          
          <span class="survey-status">
            <StatusSurveyComponent width="24" :status="survey.Status"></StatusSurveyComponent>
            {{ survey.Status }}</span
          >
          <div
            v-for="(chapter, index) in chapters"
            :key="chapter.Id"
            @click="ScrollToSection(index + 1)"
          >
            <div
              class="chapter"
              :class="{ active: `section-survey-${index + 1}` == activeSection }"
            >
              <div class="d-flex flex-column align-items-center">
                <StatusSurveyComponent width="20" :status="chapter.Status"></StatusSurveyComponent>
                <div class="vertical-bar" v-if="index + 1 != chapters.length"></div>
              </div>

              <h6 class="color-N-900 pb-4">{{ chapter.Name }}</h6>
            </div>
          </div>
        </div>
      </div>
      <div class="w-100">
        <div class="d-lg-none sticky-menu-mobile">
        
          <span class="survey-status mb-0"
            ><StatusSurveyComponent width="24" :status="survey.Status"></StatusSurveyComponent
            >{{ survey.Status }}</span
          >
          <div class="d-flex justify-content-end gap-2">
            <button
              @click="FalseSaveSurvey()"
              v-if="!survey.Submited"
              type="button"
              class="btn btn-secondary btn-sm"
            >
              Save
            </button>
            <button
              v-if="!survey.Submited"
              type="button"
              class="btn btn-primary btn-sm px-4"
              @click="SubmitSurvey()"
              ref="submitButtonDesktop1"
            >
              Submit
            </button>
          </div>
        </div>
        <div class="container-lg p-se-20-px">
          <LoaderTopInfoSurveyComponent
            v-if="!loaders.isSurveyLoaded && survey.Id == null"
          ></LoaderTopInfoSurveyComponent>
          <div v-else>
            <div class="flex-between-center mb-3 mb-lg-4">
              <h2 class="color-N-900 fw-600 roboto-cond">
                {{ survey.Title }}
              </h2>
              <div class="flex gap-2">
                <button
                  @click="FalseSaveSurvey()"
                  v-if="!survey.Submited"
                  type="button"
                  class="btn btn-secondary btn-md d-none d-lg-block"
                >
                  Save
                </button>

                <button
                  v-if="!survey.Submited"
                  type="button"
                  class="btn btn-primary btn-md d-none d-lg-block"
                  @click="SubmitSurvey()"
                  ref="submitButtonDesktop"
                >
                  Submit
                </button>
              </div>
            </div>
            <h6 class="mb-3">
              {{ survey.Description }}
            </h6>
            <ResultMessageComponent
              v-if="survey.Result"
              :surveyType="survey.Type"
              :result="survey.Result"
              :report="this.report"
            ></ResultMessageComponent>
          </div>
        </div>
        <LoaderSurveyComponent
          v-if="!loaders.isSurveyLoaded && survey.Id == null"
        ></LoaderSurveyComponent>
        <div class="container-lg p-se-20-px">
          <h6 class="mt-5 mb-4 mb-lg-0 fs-13px" v-if="!survey.Submited">
            * Please note that all questions in this form are required. We ask you to answer each
            one carefully in order to help us gather complete and accurate information.
          </h6>
        </div>

        <div
          class="container-lg my-2 my-lg-3"
          :id="`section-survey-${indexSection + 1}`"
          :class="{ active: activeSection == `section-survey-${indexSection + 1}` }"
          v-for="(section, indexSection) in survey.Sections"
        >
          <button
            class="btn-expand"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="`#collapse${section.Id}`"
            aria-expanded="false"
            aria-controls="collapseExample"
          >
            <h3 class="title">{{ section.Title }}</h3>
            <img class="arrow-collapse" src="@/assets/images/collapse-up.svg" alt="" />
          </button>
          <div class="collapse show" :id="`collapse${section.Id}`">
            <div class="ps-3 border-start">
              <div v-for="(question, indexSectionQuestion) in section.Questions" :key="question.Id">
                <div v-if="question.Type === 'Text'">
                  <Text
                    :modelValue="question"
                    @update:modelValue="question = $event"
                    @getProgress="GetSectionsProgress"
                    :index="indexSectionQuestion + 1"
                    :disabledField="survey.Submited"
                  />
                  <div :id="`validation-id-${question.Id}`">
                    <ValidationComponent
                      :answer="question.Answer"
                      :submitted="isFormSubmitted"
                    ></ValidationComponent>
                  </div>
                </div>
                <div v-if="question.Type === 'Check'">
                  <Check
                    :modelValue="question"
                    @update:modelValue="question = $event"
                    @updateSurvey="UpdateSurveyAfterAnswer()"
                    @getProgress="GetSectionsProgress"
                    @openAnnex="OpenAnnex"
                    :index="indexSectionQuestion + 1"
                    :disabledField="survey.Submited"
                  />
                  <div :id="`validation-id-${question.Id}`">
                    <ValidationComponent
                      :answer="question.Answer"
                      :submitted="isFormSubmitted"
                    ></ValidationComponent>
                  </div>
                </div>
                <div v-if="question.Type === 'Score'">
                  <Score
                    :modelValue="question"
                    @update:modelValue="question = $event"
                    @updateSurvey="UpdateSurveyAfterAnswer()"
                    @getProgress="GetSectionsProgress"
                    :index="indexSectionQuestion + 1"
                    :disabledField="survey.Submited"
                  ></Score>
                  <div :id="`validation-id-${question.Id}`">
                    <ValidationComponent
                      :answer="question.Answer"
                      :submitted="isFormSubmitted"
                    ></ValidationComponent>
                  </div>
                </div>
                <template v-if="question.SubSections && question.SubSections.length">
                  <div class="ms-lg-5">
                    <template v-for="(subsection, indexSub) in question.SubSections">
                      <button
                        class="btn-expand"
                        type="button"
                        data-bs-toggle="collapse"
                        :data-bs-target="`#collapse${section.Id}-${subsection.Id}`"
                        aria-expanded="false"
                        aria-controls="collapseExample"
                      >
                        <h4 class="fw-600 mb-3 color-N-900">
                          <i>{{ subsection.Title }}</i>
                        </h4>
                        <img class="arrow-collapse" src="@/assets/images/collapse-up.svg" alt="" />
                      </button>

                      <div class="collapse show" :id="`collapse${section.Id}-${subsection.Id}`">
                        <div class="ps-3 border-start">
                          <div
                            v-for="(subQuestion, indexSubQuestion) in subsection.Questions"
                            :key="indexSubQuestion"
                          >
                            <div v-if="subQuestion.Type === 'Text'">
                              <Text
                                :modelValue="subQuestion"
                                @update:modelValue="subQuestion = $event"
                                @getProgress="GetSectionsProgress"
                                :index="indexSub + 1"
                                :disabledField="survey.Submited"
                              />
                              <div :id="`validation-id-${subQuestion.Id}`">
                                <ValidationComponent
                                  :answer="subQuestion.Answer"
                                  :submitted="isFormSubmitted"
                                ></ValidationComponent>
                              </div>
                            </div>
                            <div v-if="subQuestion.Type === 'Check'">
                              <Check
                                :modelValue="subQuestion"
                                @update:modelValue="subQuestion = $event"
                                @updateSurvey="UpdateSurveyAfterAnswer()"
                                @getProgress="GetSectionsProgress"
                                @openAnnex="OpenAnnex"
                                :index="indexSubQuestion + 1"
                                :disabledField="survey.Submited"
                              />
                              <div :id="`validation-id-${subQuestion.Id}`">
                                <ValidationComponent
                                  :answer="subQuestion.Answer"
                                  :submitted="isFormSubmitted"
                                ></ValidationComponent>
                              </div>
                            </div>
                            <div v-if="subQuestion.Type === 'Score'">
                              <Score
                                :modelValue="subQuestion"
                                @update:modelValue="subQuestion = $event"
                                @updateSurvey="UpdateSurveyAfterAnswer()"
                                @getProgress="GetSectionsProgress"
                                :index="indexSubQuestion + 1"
                                :disabledField="survey.Submited"
                              ></Score>
                              <div :id="`validation-id-${subQuestion.Id}`">
                                <ValidationComponent
                                  :answer="subQuestion.Answer"
                                  :submitted="isFormSubmitted"
                                ></ValidationComponent>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </template>
                  </div>
                </template>
              </div>
              <template v-if="section.SubSections">
                <div class="ms-lg-5">
                  <template v-for="(subs, indexSubSection) in section.SubSections">
                    <button
                      class="btn-expand"
                      type="button"
                      data-bs-toggle="collapse"
                      :data-bs-target="`#collapse${section.Id}-${subs.Id}`"
                      aria-expanded="false"
                      aria-controls="collapseExample"
                    >
                      <h4 class="fw-600 mb-3 color-N-900">
                        <i>{{ subs.Title }}</i>
                      </h4>
                      <img
                        width="20"
                        class="arrow-collapse"
                        src="@/assets/images/collapse-up.svg"
                        alt=""
                      />
                    </button>
                    <div class="collapse show" :id="`collapse${section.Id}-${subs.Id}`">
                      <div class="border-start ps-3">
                        <div v-for="(question, indexQuestion) in subs.Questions">
                          <div v-if="question.Type === 'Text'">
                            <Text
                              :modelValue="question"
                              @update:modelValue="question = $event"
                              @getProgress="GetSectionsProgress"
                              :index="indexQuestion + 1"
                              :disabledField="survey.Submited"
                            />
                            <div :id="`validation-id-${question.Id}`">
                              <ValidationComponent
                                :answer="question.Answer"
                                :submitted="isFormSubmitted"
                              ></ValidationComponent>
                            </div>
                          </div>
                          <div v-if="question.Type === 'Check'">
                            <Check
                              :modelValue="question"
                              @update:modelValue="question = $event"
                              @updateSurvey="UpdateSurveyAfterAnswer()"
                              @getProgress="GetSectionsProgress"
                              @openAnnex="OpenAnnex"
                              :index="indexQuestion + 1"
                              :disabledField="survey.Submited"
                            />
                            <div :id="`validation-id-${question.Id}`">
                              <ValidationComponent
                                :answer="question.Answer"
                                :submitted="isFormSubmitted"
                              ></ValidationComponent>
                            </div>
                          </div>
                          <div v-if="question.Type === 'Score'">
                            <Score
                              :modelValue="question"
                              @update:modelValue="question = $event"
                              @updateSurvey="UpdateSurveyAfterAnswer()"
                              @getProgress="GetSectionsProgress"
                              :index="indexQuestion + 1"
                              :disabledField="survey.Submited"
                            ></Score>
                            <div :id="`validation-id-${question.Id}`">
                              <ValidationComponent
                                :answer="question.Answer"
                                :submitted="isFormSubmitted"
                              ></ValidationComponent>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </template>
                </div>
              </template>
            </div>
          </div>
        </div>
        <div class="submit-bottom d-none d-lg-flex" v-if="!isTopSubmitInView && !survey.Submited">
          <button
            :disabled="survey.Submited == true"
            type="button"
            class="btn btn-primary btn-md d-none d-lg-block"
            @click="SubmitSurvey()"
          >
            Submit
          </button>
        </div>
      </div>

      <div class="left-menu d-flex-1700px justify-content-end pe-0">
        <button
          v-if="annexes.length > 0"
          class="btn annexes"
          type="button"
          data-bs-toggle="offcanvas"
          data-bs-target="#offCanvasAnnexDesktop"
          aria-controls="offCanvasAnnexDesktop"
        >
          <img src="@/assets/images/survey-annexes.svg" alt="annexes" />Annexes
        </button>
      </div>
    </div>
    <div
      class="annex-desktop offcanvas offcanvas-end"
      tabindex="-1"
      id="offCanvasAnnexDesktop"
      ref="offCanvasAnnexDesktop"
      aria-labelledby="offcanvasRightLabel"
    >
      <div class="offcanvas-header">
        <h5 class="flex gap-2" id="offcanvasRightLabel">
          <img src="@/assets/images/survey-annexes.svg" alt="annexes" />Annexes
        </h5>
        <button
          type="button"
          class="btn-close text-reset"
          data-bs-dismiss="offcanvas"
          aria-label="Close"
        >
          <img src="@/assets/images/survey-close-annexes.svg" alt="close annexes" />
        </button>
      </div>
      <div class="offcanvas-body" ref="offCanvasAnnexDesktopBody">
        <AnnexesComponent :copyAnnexes="annexes" :activeAnnex="activeAnnex"></AnnexesComponent>
      </div>
    </div>
  </div>
</template>
<script>
import Check from '../../components/Others/Survey/Check.vue'
import Text from '../../components/Others/Survey/Text.vue'
import AnnexesComponent from '@/components/Others/Survey/AnnexesComponent.vue'
import ValidationComponent from '@/components/Others/Survey/ValidationComponent.vue'
import Score from '@/components/Others/Survey/Score.vue'
import ResultMessageComponent from '@/components/Others/Survey/ResultMessageComponent.vue'
import LoaderSurveyComponent from '@/components/Others/loaders/LoaderSurveyComponent.vue'
import LoaderTopInfoSurveyComponent from '@/components/Others/loaders/LoaderTopInfoSurveyComponent.vue'
import { Offcanvas } from 'bootstrap'
import StatusSurveyComponent from '@/components/Others/Survey/StatusSurveyComponent.vue'
import MenuSurveysComponent from '@/components/Others/MenuSurveysComponent.vue'
export default {
  name: 'FirstSurvey',
  components: {
    Check,
    Text,
    Score,
    AnnexesComponent,
    ValidationComponent,
    ResultMessageComponent,
    LoaderSurveyComponent,
    LoaderTopInfoSurveyComponent,
    Offcanvas,
    StatusSurveyComponent,
    MenuSurveysComponent,
  },
  data() {
    return {
      loaders: {},
      chapters: [],
      activeSection: 'null',
      survey: {
        Id: null,
        OrganizationSurveyId: null,
        Description: '',
        Sections: [],
        Status: '',
        Submited: null,
        Title: '',
        KeyForUpdate: 0,
      },
      isFormSubmitted: false,
      isTopSubmitInView: false,
      observer: null,
      activeAnnex: null,
      annexes: [],
      report: null,
    }
  },
  methods: {
    OpenAnnex(annex, index) {
      if (window.innerWidth < 1400) {
        const offcanvasEl = document.getElementById('offCanvasMobileAnnexes')
        const instance = Offcanvas.getOrCreateInstance(offcanvasEl)
        instance.show()
        this.activeAnnex = {
          Name: annex,
          Index: index,
        }
      } else {
        const offcanvasEl = document.getElementById('offCanvasAnnexDesktop')
        const instance = Offcanvas.getOrCreateInstance(offcanvasEl)
        instance.show()
        this.activeAnnex = {
          Name: annex,
          Index: index,
        }
      }
    },
    UpdateSurveyAfterAnswer() {
      const searchParams = {
        OrganizationSurveyId: this.$route.params.surveyId,
      }
      this.$axios
        .get(`/api/Survey/getOrganizationPublicSurvey?${new URLSearchParams(searchParams)}`)
        .then((response) => {
          this.PrepareSurvey(response)
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GoBack() {
      this.$router.go(-1)
    },
    async GetSurvey(id) {
      this.loaders.isSurveyLoaded = false

      const searchParams = {
        OrganizationSurveyId: id,
      }

      await this.$axios
        .get(`/api/Survey/getOrganizationPublicSurvey?${new URLSearchParams(searchParams)}`)
        .then((response) => {
          this.loaders.isSurveyLoaded = true
          this.PrepareSurvey(response)
          this.GetReportData(id)
        })
        .catch((error) => {
          if (error) {
            this.$router.push({
              name: 'not-found',
            })
          }
          this.loaders.isSurveyLoaded = true
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    PrepareSurvey(response) {
      this.survey = response.data
      this.survey.Sections = this.survey.Sections.map((section) => ({
        ...section,
        Questions: section.Questions.map((question) => ({
          ...question,
          Answer:
            question.Type === 'Check' || question.Type === 'Score'
              ? (question.Answers?.find((item) => item.Check) ?? null)
              : question.Answer,
        })),
      }))

      this.survey.Sections = this.survey.Sections.map((section) => {
        return {
          ...section,
          SubSections: section.SubSection.filter((subs) => !subs.ParentId).map((subs) => ({
            ...subs,
            Questions: subs.Questions.map((question) => ({
              ...question,
              Answer:
                question.Type === 'Check' || question.Type === 'Score'
                  ? (question.Answers?.find((item) => item.Check) ?? null)
                  : question.Answer,
            })),
          })),
        }
      })

      this.survey.Sections = this.survey.Sections.map((section) => {
        const updatedQuestions = section.Questions.map((question) => ({
          ...question,
          SubSections: [],
        }))
        const subsections = section.SubSection
        if (subsections.length) {
          subsections.forEach((subsec) => {
            const questionIndex = updatedQuestions.findIndex((q) => q.Id === subsec.ParentId)
            if (questionIndex !== -1) updatedQuestions[questionIndex].SubSections.push(subsec)
          })
        }
        return {
          ...section,
          Questions: updatedQuestions,
        }
      })
      this.$nextTick(() => {
        this.SubmitButtonObserver()
      })
    },
    GetSectionsProgress() {
      const searchParams = {
        OrganizationSurveyId: this.$route.params.surveyId,
      }
      this.$axios
        .get(`/api/Survey/getPublicSectionProgress?${new URLSearchParams(searchParams)}`)
        .then((response) => {
          this.chapters = response.data
        })
        .catch(() => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    OnScrollSections() {
      const scrollPos = window.scrollY + window.innerHeight / 2
      for (let index = 0; index < this.chapters.length; index++) {
        // const chapter = this.chapters[index]
        const idSection = `section-survey-${index + 1}`
        const el = document.getElementById(idSection)

        if (el) {
          const top = el.offsetTop
          const bottom = top + el.offsetHeight
          if (scrollPos >= top && scrollPos <= bottom) {
            this.activeSection = `section-survey-${index + 1}`
            break
          }
        }
      }
    },
    ScrollToSection(index, mobile) {
      const idSection = `section-survey-` + index
      const element = document.getElementById(idSection)
      if (mobile) {
        const el = document.getElementById('offCanvasMobileProgress')
        const instance = Offcanvas.getOrCreateInstance(el)
        instance.hide()
      }
      if (element) {
        const rect = element.getBoundingClientRect()
        const offset = rect.top + window.pageYOffset - 120
        window.scrollTo({
          top: offset,
          behavior: 'smooth',
        })
      }
      if (idSection != this.activeSection) {
      }
    },
    ScrollToValidation(idValidation) {
      const idValidationComponent = `validation-id-` + idValidation
      const element = document.getElementById(idValidationComponent)
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
    ValidateSection(section) {
      for (const question of section.Questions) {
        if (!question.Answer || question.Answer === '') {
          this.ScrollToValidation(question.Id)
          return false
        }
        if (question.Type == 'Check') {
          if (question.Answer?.Answer == 'Other' && !question.Answer.Mentions) {
            this.ScrollToValidation(question.Id)
            return false
          }
        }

        if (question.SubSections && question.SubSections.length > 0) {
          for (const sub of question.SubSections) {
            for (const subQuestion of sub.Questions) {
              if (!subQuestion.Answer || subQuestion.Answer === '') {
                this.ScrollToValidation(subQuestion.Id)
                return false
              }
              if (subQuestion.Type == 'Check') {
                if (subQuestion.Answer?.Answer == 'Other' && !subQuestion.Answer.Mentions) {
                  this.ScrollToValidation(subQuestion.Id)
                  return false
                }
              }
            }
          }
        }
      }
      if (section.SubSections) {
        for (const subs of section.SubSections) {
          for (const question of subs.Questions) {
            if (!question.Answer || question.Answer === '') {
              this.ScrollToValidation(question.Id)
              return false
            }
            if (question.Type == 'Check') {
              if (question.Answer?.Answer == 'Other' && !question.Answer.Mentions) {
                this.ScrollToValidation(question.Id)
                return false
              }
            }
          }
        }
      }
      return true
    },
    ValidateAllSections(sections) {
      for (const section of sections) {
        if (!this.ValidateSection(section)) {
          return false
        }
      }
      return true
    },
    SubmitSurvey() {
      if (!this.survey.Submited) {
        this.isFormSubmitted = true
        const isFormValid = this.ValidateAllSections(this.survey.Sections)
        if (isFormValid) {
          const searchParams = {
            OrganisationSurveyId: this.$route.params.surveyId,
          }
          this.$store.commit('SET_LOADER', true)
          this.$axios
            .post(`/api/Survey/submitPublicSurvey?${new URLSearchParams(searchParams)}`)
            .then((response) => {
              this.$store.commit('SET_LOADER', false)
              this.GetSurvey(this.$route.params.surveyId)
              this.$utils.ToastNotify('success', 'Survey submitted!')
              window.scrollTo({ top: 0, behavior: 'smooth' })
            })
            .catch(() => {
              this.$store.commit('SET_LOADER', false)
              this.$utils.ToastNotifySomethingWentWrong()
            })
        }
      }
    },
    SubmitButtonObserver() {
      this.observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            this.isTopSubmitInView = entry.isIntersecting
          })
        },
        {
          threshold: 0.4,
        },
      )

      if (this.$refs.submitButtonDesktop) {
        this.observer.observe(this.$refs.submitButtonDesktop)
      }
    },
    GetAnnexes(device) {
      this.$axios
        .get(`/api/Survey/getSurveyAttachments?surveyId=${this.survey.Id}`)
        .then((response) => {
          this.annexes = response.data
        })
        .catch(() => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    GetReportData(id) {
      const searchParams = {
        organisationSurveyId: id,
      }
      const api =
        this.$store.state.user.UserName && !this.$store.state.user.UserRoles.includes('Guest')
          ? `/api/Administration/getScoreSurveyReports?organisationSurveyId=${id}`
          : `/api/Survey/getScorePublicSurveyReports?${new URLSearchParams(searchParams)}`
      if (this.survey.Type == 'score') {
        this.$axios
          .get(api)
          .then((response) => {
            if (response.data) {
              this.report = response.data
            }
          })
          .catch(() => {
            this.$utils.ToastNotifySomethingWentWrong()
          })
      }
    },
    FalseSaveSurvey() {
      this.$store.commit('SET_LOADER', true)
      setTimeout(() => {
        this.$store.commit('SET_LOADER', false)
      }, '1000')
    },
  },
  mounted() {
    window.addEventListener('scroll', this.OnScrollSections)
    this.$nextTick(() => {
      setTimeout(() => {
        this.OnScrollSections()
      }, 200)
    })
    this.SubmitButtonObserver()
    const offCanvasMobileProgress = document.getElementById('offCanvasMobileProgress')
    offCanvasMobileProgress.addEventListener('show.bs.offcanvas', () => {
      this.$refs.offcanvasBody.scrollTop = 0
    })
  },
  async created() {
    const id = this.$route.params.surveyId
    this.GetSectionsProgress()
    await this.GetSurvey(id)
    this.GetAnnexes()
  },
}
</script>
<style scoped>
.container-fluid {
  display: flex;
  justify-content: center;
  padding: 0px;
}

.title {
  font-size: 18px;
  font-weight: 600;
  color: var(--neutral-900);
  margin-bottom: 16px;
  padding-right: 60px;
}

.left-menu {
  width: 320px;
  min-width: 320px;
  min-height: 100vh;
  position: relative;
  padding: 40px;
  padding-top: 0px;
}
.sticky-menu {
  position: sticky;
  top: 122px;
}
.sticky-menu-mobile {
  position: sticky;
  top: 65px;
  left: 0px;
  background-color: var(--neutral-50);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 10px 12px;
  z-index: 3;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid var(--neutral-50);
}
.chapter h6 {
  font-size: 14px;
  line-height: 123%;
  font-family: 'Roboto Condensed';
  padding-top: 2px;
  cursor: pointer;
}
.vertical-bar {
  height: 100%;
  width: 1px;
  background-color: var(--success-500);
  margin-top: 2px;
}
.btn.plain {
  transition: all 0.3s;
}
.btn.plain:hover {
  transform: scale(1.05);
}
.d-flex-1700px {
  display: none;
  position: relative;
}
@media (min-width: 1400px) {
  .d-flex-1700px {
    display: flex;
    width: auto;
    min-width: 200px;
  }
}
@media (min-width: 1600px) {
  .d-flex-1700px {
    width: 320px;
    min-width: 320px;
  }
}
.chapter {
  min-height: 50px;
  display: flex;
  gap: 8px;
  padding-left: 17px;
}
.chapter.active h6 {
  font-weight: 600;
  color: var(--neutral-900) !important;
}
.chapter.not-started h6 {
  color: var(--neutral-500);
}

.survey-status {
  height: 42px;
  display: flex;
  align-items: center;
  gap: 12px;
  color: var(--neutral-900);
  background-color: var(--neutral-100);
  border-radius: 8px;
  padding: 9px 16px;
  font-size: 15px;
  font-weight: 600;
  margin-bottom: 40px;
}

/*--------------------canvas annex-desktop*/

.btn.annexes {
  background-color: #ffffff;
  height: 48px;
  padding: 12px;
  display: flex;
  align-items: center;
  gap: 10px;
  border-radius: 8px 0px 0px 8px;
  position: sticky;
  right: 0px;
  top: 175px;
}

.annex-desktop.offcanvas {
  margin-top: 12px;
  margin-bottom: 12px;
  border-bottom-left-radius: 16px;
  border-top-left-radius: 16px;
  border: none;
  width: 1400px;
}

.annex-desktop.offcanvas.show:not(.hiding),
.annex-desktop.offcanvas.showing {
  height: auto;
}

.progress.offcanvas-header {
  border-bottom: 1px solid var(--neutral-200);
}

.annex-desktop .btn-close {
  --bs-btn-close-bg: none;
  --bs-btn-close-opacity: 1;

  width: 44px;
  height: 44px;
  position: absolute;
  top: 50%;
  left: -29px;
  transform: translateY(-50%);

  background: transparent var(--bs-btn-close-bg) center / 1em auto no-repeat;
}
/*---------------------------------------------------annex mobile canvas*/
.annex-mobile.offcanvas {
  border-top-left-radius: 24px;
  border-top-right-radius: 24px;
  border-bottom-left-radius: 0px;
  border: none;
  width: 100%;
  margin: 0px;
  height: 85%;
  /*max-height: 559px;*/
}
.annex-mobile .offcanvas-header {
  border-bottom: 1px solid var(--neutral-200);
}
.annex-mobile.offcanvas .btn-close[data-v-1c776ccc] {
  --bs-btn-close-bg: url('@/assets/images/x-icon.svg');
  --bs-btn-close-opacity: 1;
  width: 20px;
  height: 20px;
  position: absolute;
  top: 19px;
  right: 22px;
  background: transparent var(--bs-btn-close-bg) center / 1em auto no-repeat;
  background-size: 20px;
}
/* -------------------------------------progress canvas*/
.progress.offcanvas {
  border-top-left-radius: 24px;
  border-top-right-radius: 24px;
  border-bottom-left-radius: 0px;
  border: none;
  width: 100%;
  margin: 0px;
  height: 80%;
}

.progress.offcanvas .offcanvas-header {
  border-bottom: none;
  padding: 0px;
  padding-bottom: 20px;
}

.progress.offcanvas .btn-close {
  --bs-btn-close-bg: url('@/assets/images/x-icon.svg');
  --bs-btn-close-opacity: 1;
  width: 20px;
  height: 20px;
  position: absolute;
  top: 19px;
  right: 22px;
  background: transparent var(--bs-btn-close-bg) center / 1em auto no-repeat;
  background-size: 20px;
}

/*------------------------------------------------collapse questions*/
.btn-expand {
  background-color: transparent;
  width: 100%;
  color: var(--neutral-900);
  border: none;
  position: relative;
  padding-right: 54px;
  text-align: start;
}
.btn-expand.collapsed .arrow-collapse {
  transform: rotate(180deg);
}
.btn-expand:active {
  border: none;
}

.arrow-collapse {
  position: absolute;
  right: 24px;
  top: 0px;
  transition: all 0.3s;
}

.btn-expand.collapsed .arrow-collapse {
  transform: rotate(180deg);
}
.buttons-mobile button {
  width: 44px;
  height: 44px;
  border-radius: 12px;
  border: 2px solid #ffffff;
  background-color: var(--neutral-50);
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0px 4px 12px 0px #00000029;
}
.buttons-mobile {
  z-index: 3;
  position: fixed;
  right: 16px;
  bottom: 31px;
}
.mt-40-px {
  margin-top: 24px;
}
.form-control {
  min-height: 40px;
}

@media (min-width: 992px) {
  .mt-40-px {
    margin-top: 40px;
  }
  .title {
    font-size: 24px;
    margin-bottom: 24px;
  }
  .survey-status {
    font-size: 18px;
  }
  .form-control {
    min-height: 48px;
  }
}

.offcanvas-body {
  overflow-y: hidden;
}

.top-menu {
  height: 65px;
  width: 100%;
  background-color: #14181f;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0px 40px;
  position: fixed;
  z-index: 12;
}

.logo-occtet {
  width: 133px;
}

img:not(.logo-occtet) {
  filter: invert(99%) sepia(1%) saturate(521%) hue-rotate(71deg) brightness(114%) contrast(112%);
}

.submit-bottom {
  position: sticky;
  bottom: 0px;
  height: 80px;
  background: url('@/assets/images/bottom-blur.svg');
  display: flex;
  justify-content: end;
  align-items: center;
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  z-index: 4;
}
.submit-bottom .btn-primary {
  margin-right: 12px;
}
</style>
