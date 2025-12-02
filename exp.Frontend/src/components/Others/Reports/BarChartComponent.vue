<template>
  <div class="item" v-for="(answer, index) in answers" :key="index">
    <h6 class="text">{{ answer.Name }}</h6>
    <div class="flex gap-5">
      <div class="progress">
        <div
          class="progress-bar"
          role="progressbar"
          :style="{ width: progressPercent(answer) + '%' }"
          :aria-valuenow="answer.Count"
          aria-valuemin="0"
          :aria-valuemax="maxValue"
        ></div>
      </div>
      {{ answer.Count }}
    </div>
  </div>
</template>
<script>
export default {
  name: '',
  components: {
  },
  props: {
    answers: {
      type: Array,
    },
  },
  methods: {
    progressPercent(answer) {
      if (answer.Count == 0) {
        return 0
      } else {
        const percent = (answer.Count / this.maxValue) * 100
        return Math.min(100, Math.round(percent))
      }
    },
  },
  data() {
    return {
      maxValue: 0,
    }
  },
  watch: {
    answers: {
      handler(newVal) {
        this.maxValue = newVal.reduce((sum, a) => sum + a.Count, 0)
      },
    },
  },
}
</script>
<style scoped>
h4 {
  margin-bottom: 34px;
}
.item {
  color: var(--neutral-900);
  font-size: 13px;
  display: flex;
  justify-content: space-between;
  margin-bottom: 22px;
  gap: 36px;
}
.item .flex {
  width: 100%;
}

.progress {
  width: 100%;
  flex-grow: 1;
  height: 16px;
  background-color: var(--neutral-100);
  border-radius: 2px;
  box-shadow: 4px 0px 12px 0px #00000029;
}

.progress-bar {
  background: linear-gradient(90deg, rgba(12, 165, 235, 0.2) 0%, #0ca5eb 69.63%, #0081c5 99.88%);
}

.text {
  font-size: 13px;
  min-width: 120px;
}
</style>
