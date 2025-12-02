<template>
  <div class="card">
    <div class="description">
      <div class="line line-2"></div>
      <div class="line line-1"></div>
    </div>
   
      <div class="description">
        <div class="line line-3"></div>
        <div class="line line-4"></div>
        <div class="line line-5"></div>
      </div>
   
  </div>
</template>
<script>
export default {
  name: '',
  data() {
    return {}
  },
}
</script>
<style scoped>
.card {
  width: 100%;
  background: transparent;
  border: none;
  border-radius: 10px;
}

.card .description {
  margin-bottom: 24px;
}
.description .line {
  background: #d9d9d9;
  border-radius: 10px;
  height: 13px;
  margin: 10px 0;
  overflow: hidden;
  position: relative;
}
.description .line-1 {
  width: calc(100% - 15%);
}

.description .line-3 {
  width: 100%;
  border-radius: 4px;
  height: 22px;
}
.description .line-4 {
  width: 40%;
  border-radius: 4px;
  height: 22px;
}
.description .line-5 {
  width: 70%;
  border-radius: 4px;
  height: 22px;
}
.card .btns .btn {
  height: 24px;
  width: 100%;
  background: #d9d9d9;
  border-radius: 4px;
  position: relative;
  overflow: hidden;
  margin-bottom: 16px;
}
.btn-2 {
  width: 40%;
}
.btn-3 {
  width: 70%;
}

.details span::before,
.description .line::before,
.btns .btn::before {
  position: absolute;
  content: '';
  height: 100%;
  width: 100%;
  background-image: linear-gradient(
    to right,
    #d9d9d9 0%,
    rgba(0, 0, 0, 0.07) 20%,
    #d9d9d9 40%,
    #d9d9d9 100%
  );
  background-repeat: no-repeat;
  background-size: 450px 400px;
  animation: shimmer 1s linear infinite;
}
.header .img::before {
  background-size: 650px 600px;
}
.details span::before {
  animation-delay: 0.2s;
}
.btns .btn-2::before {
  animation-delay: 0.22s;
}
@keyframes shimmer {
  0% {
    background-position: -450px 0;
  }
  100% {
    background-position: 450px 0;
  }
}
</style>
