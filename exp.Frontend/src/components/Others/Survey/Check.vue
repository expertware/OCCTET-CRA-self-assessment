<template>
  <div class="inner-box mb-3">
    <div class="flex-between-center">
      <h4 class="color-N-900 fw-600 mb-3">
        <span v-if="index != ''">{{ index }}. </span> {{ modelValue.Title }}
      </h4>
      <div v-if="$route.name!='first-survey'" @click="$emit('AIgenerate')" class="ai-icon"></div>
    </div>

    <div class="d-flex gap-3 mb-4" v-if="modelValue.Attachments.length > 0">
      <button
        type="button"
        v-for="(atch, index) in modelValue.Attachments"
        @click="$emit('openAnnex', atch.Name, index)"
        class="btn btn-annex"
      >
        {{ atch.Name }}
      </button>
    </div>
    <h6 v-if="modelValue.Description != ''" class="color-grey fs-12px">
      <i>{{ modelValue.Description }}</i>
    </h6>
    <div class="row" v-if="disabledField">
      <div
        class="col-12"
        :class="{ 'col-lg-6 col-xxl': !LongQuestion(modelValue.Answers) }"
        v-for="(item, indexAns) in modelValue.Answers"
      >
        <div
          v-if="item.Answer != 'Other'"
          class="inactive-control d-flex gap-3 align-items-start mt-2"
        >
          <img v-if="!item.Check" src="@/assets/images/icon-check.svg" alt="" /><img
            v-else
            src="@/assets/images/check-blue.svg"
            alt=""
          />{{ item.Answer }}
        </div>

        <div v-else class="inactive-control gap-3 mt-2">
          <img v-if="!item.Check" src="@/assets/images/icon-check.svg" alt="" /><img
            v-else
            src="@/assets/images/check-blue.svg"
            alt=""
          />{{ item.Check == true ? item.Mentions : item.Answer }}
        </div>
      </div>
    </div>
    <div class="row" v-else>
      <div
        class="col-12"
        :class="{ 'col-lg-6 col-xxl': !LongQuestion(modelValue.Answers) }"
        v-for="(item, indexAns) in modelValue.Answers"
      >
        <label
          class="form-check w-100 mt-2"
          :class="{ 'blue-bg': item.Check }"
          v-if="item.Answer != 'Other'"
          :for="`${modelValue.Id}${indexAns}`"
        >
          <input
            class="form-check-input"
            type="radio"
            :value="item"
            v-model="modelValue.Answer"
            @click="SendAnswer(item)"
            :id="`${modelValue.Id}${indexAns}`"
          />

          <span class="form-check-label ps-2">
            {{ item.Answer }}
          </span>
        </label>

        <label class="form-check w-100 mt-2" :class="{ 'blue-bg': item.Check }" v-else>
          <input
            class="form-check-input"
            type="radio"
            :value="item"
            v-model="modelValue.Answer"
            @click="SendAnswer(item)"
            :id="`${modelValue.Id}${indexAns}`"
          />
          <span class="form-check-label w-100 ps-2 d-flex flex-column gap-2">
            <input
              :class="{ 'blue-bg': item.Check }"
              v-model.lazy="item.Mentions"
              @focus="SelectValueOnFocus(item)"
              @change="SendAnswer(item)"
              type="text"
              class="complete-others"
              placeholder="Other (please specify)..."
              @click="SendAnswer(item)"
              @input="UpdateAnswerWhenMention()"
              :id="`${modelValue.Id}${indexAns}-mention`"
              autocomplete="off"
            />
          </span>
        </label>
      </div>
    </div>
  </div>

  <div v-for="(answer, index) in modelValue.Answers">
    <YellowMessageComponent
      v-if="answer.Prompt"
      :promptText="answer.Prompt"
    ></YellowMessageComponent>
  </div>
</template>
<script>
import YellowMessageComponent from './YellowMessageComponent.vue'
export default {
  inheritAttrs: false,
  name: 'CheckComponent',
  components: {
    YellowMessageComponent,
  },
  emits: ['updateSurvey', 'getProgress', 'update:modelValue', 'openAnnex', 'AIgenerate'],
  props: {
    disabledField: {
      type: Boolean,
      default() {
        false
      },
    },
    name: {
      type: String,
      default: '',
    },
    index: {
      type: Number,
      default: 0,
    },
    modelValue: {
      type: Object,
      default: {
        Id: null,
        Title: '',
        Description: null,
        Answers: [],
        Answer: '',
        Type: '',
        ParentId: null,
      },
    },
  },
  data() {
    return {
      copyAnswer: null,
    }
  },

  methods: {
    SelectValueOnFocus(value) {
    },
    LongQuestion(answers) {
      if (answers) {
        const hasLongAnswer = answers.some((a) => a.Answer.length > 30)
        return hasLongAnswer
      }
    },
    UpdateAnswerWhenMention() {
      this.$emit('update:modelValue', this.modelValue)
    },
    SendAnswer(selectedItem) {
      const previousAnswer = this.modelValue.Answers.find((item) => item.Check == true)
      const previousAnswers = this.modelValue.Answers
    
      const obj = {
        QuestionId: this.modelValue.Id,
        OrganizationSurveyId: this.$route.params.surveyId,
        Answer: selectedItem.Answer,
        Mentions: selectedItem.Mentions ?? null,
      }
      const url = this.$route.name=='first-survey'
        ? `/api/Survey/addPublicAnswersQuestion`
        : `/api/Survey/addAnswersQuestion`
      this.$axios
        .post(url, obj)
        .then((response) => {
          this.modelValue.Answers.forEach((item) => {
            item.Check = item === selectedItem
            if (selectedItem.Answer !== 'Other' && item.Answer === 'Other') {
              item.Mentions = null
            }
          })
          this.$emit('updateSurvey')
          this.$emit('getProgress')
        })
        .catch((error) => {
          this.modelValue.Answers = previousAnswers
          this.modelValue.Answer = previousAnswer
          this.modelValue.Answers.forEach((item) => {
            item.Check = item === previousAnswer
            if (previousAnswer.Answer !== 'Other' && item.Answer === 'Other') {
              item.Mentions = null
            }
          })
         
        })
    },
  },
  watch: {
    modelValue: {
      handler(val) {
        if (this.copyAnswer == null) {
          this.copyAnswer = JSON.parse(JSON.stringify(val.Answer))
        }
      },
      immediate: true,
      deep: true,
    },
  },
}
</script>
<style scoped>
.form-check {
  min-height: 40px;
  border: 1px solid var(--neutral-200);
  padding: 6px;
  padding-left: 35px;
  border-radius: 8px;
  width: 100%;
  cursor: pointer;
  display: flex;
  align-items: center;
}

.form-check-input {
  background-image: url('@/assets/images/icon-check.svg');
  width: 24px;
  height: 24px;
  margin-top: -1px;
  border: none;
  cursor: pointer;
}
.form-check-input:checked[type='radio'] {
  --bs-form-check-bg-image: none;
  background-image: url('@/assets/images/check-blue.svg');
}

.form-check-input:checked {
  background-color: transparent;
  border-color: transparent;
}

.form-check-input:focus {
  outline: 0;
  box-shadow: none;
}
.form-check-label {
  margin-left: 10px;
  cursor: pointer;
}

.inner-box {
  padding: 16px;
  border: 1px solid var(--neutral-100);
  border-radius: 16px;
  background-color: #ffffff;
  box-shadow: 0px 12px 64px 0px #64879e29;
}
.row {
  --bs-gutter-x: 12px;
}
.complete-others {
  border: none;
  font-size: 15px;
  width: 100%;
  color: var(--neutral-700);
}
.complete-others:focus-visible {
  outline: none;
}
.complete-others::placeholder {
  font-style: italic;
}

.blue-bg {
  background-color: #f0f8ff;
}

@media (min-width: 992px) {
  .form-check {
    min-height: 48px;
    padding: 12px;
    padding-left: 35px;
  }

  .inner-box {
    padding: 24px;
  }
}
</style>
