<template>
  <div class="card">
    <div class="row h-100">
      <div class="col">
        <div class="description">
          <div class="line line-1"></div>
          <div class="line line-2"></div>
        </div>
      </div>
      <div class="col">
        <div class="header">
          <div class="img"></div>
        </div>
      </div>
      <div class="col">
        <div class="description">
          <div class="line line-3"></div>
          <div class="line line-4"></div>
        </div>
      </div>
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
  height: 100%;
  width: 100%;
  background: #fff;
  padding: 30px;
  border-radius: 10px;
  border: none;
}
.card .header {
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
.header .img {
  height: 150px;
  width: 150px;
  background: #d9d9d9;
  border-radius: 50%;
  position: relative;
  overflow: hidden;
}
.card .description {
  display: flex;
  flex-direction: column;
  height: 100%;
  justify-content: space-between;
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
  width: calc(100% - 40%);
}

.header .img::before,
.description .line::before {
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
