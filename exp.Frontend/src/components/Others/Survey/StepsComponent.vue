<template>
  <div class="d-flex gap-2 gap-md-4">
    <div class="item" :class="{ active: activeStep == item }" v-for="(item, index) in steps">
      {{ item }}
    </div>
  </div>
</template>
<script>
export default {
  name: '',
  props: {
    activeStep: null,
  },
  data() {
    return {
      steps: ['step 1', 'step 2', 'step 3'],
    }
  },
}
</script>
<style scoped>
.item {
  font-size: 15px;
  text-transform: uppercase;
  color: var(--neutral-700);
  padding: 0px 16px;
  padding-bottom: 4px;
}
.item.active {
  color: var(--neutral-900);
  font-weight: 600;
  position: relative;
}
.item.active::after {
  content: '';
  position: absolute;
  left: 0px;
  top: 32px;
  background-color: var(--primary-400);
  height: 3px;
  background-size: contain;
  background-repeat: no-repeat;
  height: 3px;
  width: 100%;
  border-radius: 8px;
}

@media (min-width: 768px) {
  .item {
    font-size: 20px;
  }
}
</style>
