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
        type="button"
        data
        v-for="(atch, index) in modelValue.Attachments"
        @click="$emit('openAnnex', atch.Name, index)"
        class="btn btn-annex"
      >
        {{ atch.Name }}
      </button>
    </div>
    <h6 v-if="modelValue.Description != ''" class="color-grey fs-12px mb-2">
      <i>{{ modelValue.Description }}</i>
    </h6>
    <div v-if="disabledField">
      <div class="inactive-control">{{ modelValue.Answer }}</div>
    </div>
    
    <textarea v-else
      :class="{ 'blue-bg': modelValue.Answer }"
      @focusout="SendAnswer()"
      rows="1"
      class="textarea form-control"
      aria-describedby="role-in-company"
      autocomplete="off"
      placeholder="Write something..."
      v-model="modelValue.Answer"></textarea>
  </div>
</template>
<script>
export default {
  name: 'TextComponent',
  emits: ['getProgress', 'update:modelValue', 'AIgenerate', 'openAnnex'],
  props: {
    modelValue: {
      type: Object,
      default: {},
    },
    index: '',
    disabledField: {
      type: Boolean,
      default() {
        false
      },
    },
  },
  methods: {
    SendAnswer() {
      const obj = {
        QuestionId: this.modelValue.Id,
        OrganizationSurveyId: this.$route.params.surveyId,
        Answer: this.modelValue.Answer ? this.modelValue.Answer : null,
      }
      const url = this.$route.name=='first-survey'
        ? `/api/Survey/addPublicAnswersQuestion`
        : `/api/Survey/addAnswersQuestion`
      this.$axios
        .post(url, obj)
        .then((response) => {
          this.$emit('getProgress')
        })
        .catch((error) => {
        })
    },
  },
  computed: {},
}
</script>
<style scoped>
.inner-box {
  padding: 16px;
  border: 1px solid var(--neutral-100);
  border-radius: 16px;
  background-color: #ffffff;
  box-shadow: 0px 12px 64px 0px #64879e29;
  position: relative;
}
input::placeholder {
  font-style: italic;
}

.blue-bg {
  background-color: #f0f8ff;
}
textarea.form-control {
  min-height: 40px!important;
}
@media (min-width: 992px) {
  textarea.form-control {
    min-height: 48px !important;
  }

  .inner-box {
    padding: 24px;
  }
}


</style>
