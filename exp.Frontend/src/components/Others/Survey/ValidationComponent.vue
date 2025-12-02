<template>
  <h6 v-if="!answer && submitted" class="error-message mb-3">This field is required</h6>
  <h6 v-if="answer?.Answer == 'Other' && !answer.Mentions && submitted" class="error-message mb-3">
    Please specify what the other option is
  </h6>
</template>
<script>
export default {
  name: 'ValidationComponent',
  props: {
    answer: {
      default() {
        return null
      },
    },
    submitted: {
      default() {
        return null
      },
    },
  },
  data() {
    return {}
  },
}
</script>
<style scoped></style>


