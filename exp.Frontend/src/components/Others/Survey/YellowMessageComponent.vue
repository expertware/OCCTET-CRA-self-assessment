<template>
  <div class="yellow-card">
    <img src="@/assets/images/yellow-card-icon.svg" class="pe-3" alt="" />

    <h6 class="fw-600 color-N-900">
      {{ promptText }}
    </h6>
  </div>
</template>
<script>
export default {
  name: '',
  props: {
    promptText: '',
  },
  data() {
    return {}
  },
}
</script>
<style scoped>
.yellow-card {
  min-height: 84px;
  padding: 24px;
  background-color: #ffffea;
  background-image: url("data:image/svg+xml,%3csvg width='100%25' height='100%25' xmlns='http://www.w3.org/2000/svg'%3e%3crect width='100%25' height='100%25' fill='none' rx='16' ry='16' stroke='%23FFBF00FF' stroke-width='2' stroke-dasharray='12%2c 16' stroke-dashoffset='8' stroke-linecap='round'/%3e%3c/svg%3e");
  border-radius: 16px;
  display: flex;
  align-items:center;
  margin-bottom: 16px;
}
</style>
