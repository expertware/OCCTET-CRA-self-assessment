<template>
  <div class="flex">
    <img
      :width="width"
      v-if="status.toLowerCase().includes('progress')"
      src="@/assets/images/survey-menu-in-progress.svg"
      alt=""
    />
    <img
      :width="width"
      v-if="status.toLowerCase().includes('started')"
      src="@/assets/images/survey-status-not-started.svg"
      alt=""
    />
    <img
      :width="width"
      v-if="status.toLowerCase().includes('finish')"
      src="@/assets/images/survey-menu-checked.svg"
      alt=""
    />
  </div>
</template>
<script>
export default {
  name: 'StatusSurveyModalComponent',
  props: {
    status: {
      type: String,
      default() {
        return ''
      },
    },
    width: {
      default() {
        return 24
      },
    },
  },
  data() {
    return {}
  },
}
</script>
<style scoped></style>
