<template>
  <div class="inner-box mb-3">
    <div class="flex-between-center mb-3">
      <h4 class="color-N-900 fw-600 ">
        <span v-if="index != ''">{{ index }}. </span> {{ modelValue.Title }}
      </h4>
      <div v-if="$route.name!='first-survey'" @click="$emit('AIgenerate')" class="ai-icon"></div>
    </div>
    <div class="d-flex gap-3 mb-4" v-if="modelValue.Attachments.length > 0">
      <button
        data
        v-for="(atch, index) in modelValue.Attachments"
        @click="$emit('openAnnex', atch.Name, index)"
        class="btn btn-annex"
        type="button"
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
        <div class="inactive-control d-flex gap-3 align-items-start mt-2">
          <img v-if="!item.Check" src="@/assets/images/icon-check.svg" alt="" /><img
            v-else
            src="@/assets/images/check-blue.svg"
            alt=""
          />{{ item.Answer }} - {{ item.Comment }}
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
          <span class="form-check-label ps-2"> {{ item.Answer }} - {{ item.Comment }} </span>
        </label>
      </div>
    </div>
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
  emits: ['updateSurvey', 'getProgress', 'update:modelValue', 'AIgenerate', 'openAnnex'],
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
        Title: 'Threat Modeling and Risk Assesment',
        Description:
          'Ensuring that the development team systematically identifies and mitigates potential threats during the design phase (e.g., using methodologies like STRIDE or PASTA).',
        Answers: [
          {
            Answer: '1',
            Prompt: 'bjjbjb',
            Mentions: 'hvsuidvndfv',
            Comment: 'no formal process',
            Check: 'true',
          },
          {
            Answer: '2',
            Prompt: 'bjjbjb',
            Mentions: 'hvsuidvndfv',
            Comment: 'existing  process, coverage <25%',
            Check: 'false',
          },
          {
            Answer: '3',
            Prompt: 'bjjbjb',
            Mentions: 'hvsuidvndfv',
            Comment: '  existing  process , coverage <50%',
            Check: 'false',
          },
          {
            Answer: '4',
            Prompt: 'bjjbjb',
            Mentions: 'hvsuidvndfv',
            Comment: 'existing  process, coverage <75%',
            Check: 'false',
          },
          {
            Answer: '5',
            Prompt: 'bjjbjb',
            Mentions: 'hvsuidvndfv',
            Comment: 'existing  process, coverage between 75 to 100%  or Not Applicable',
            Check: 'false',
          },
        ],
        Answer: {
          Answer: '5',
          Prompt: 'bjjbjb',
          Mentions: 'hvsuidvndfv',
          Comment: 'existing  process, coverage between 75 to 100%  or Not Applicable',
          Check: 'false',
        },
        Type: 'Score',
        ParentId: null,
      },
    },
  },

  methods: {
    SelectValueOnFocus(value) {
      //this works when user is selectimg 'Other' by clicking on 'Mention' input
      if (this.modelValue.Answer !== value) {
        this.modelValue.Answer = value
      }
    },
    LongQuestion(answers) {
      if (answers) {
        const hasLongAnswer = answers.some((a) => a.Comment.length > 30)
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
          })
          this.$emit('update:modelValue', { ...this.modelValue })
          this.$emit('updateSurvey')
          this.$emit('getProgress')
        })
        .catch((error) => {
          this.modelValue.Answers = previousAnswers
          this.modelValue.Answer = previousAnswer
          this.modelValue.Answers.forEach((item) => {
            item.Check = item === previousAnswer
          })
        })
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
  .inner-box {
    padding: 24px;
  }
  .form-check {
    min-height: 48px;
    padding: 12px;
    padding-left: 35px;
  }
}
</style>
