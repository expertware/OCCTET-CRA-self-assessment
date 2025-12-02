<template>
  <div class="card">
    <div class="row">
      <div class="col-6">
        <div class="description">
          <div class="line line-1"></div>
          <div class="line line-2"></div>
          <div class="line line-3"></div>
        </div>
        <div class="description">
          <div class="line line-4"></div>
          <div class="line line-5"></div>
          <div class="line line-6"></div>
        </div>
      </div>
      <div class="col-6">
        <div class="header">
          <div class="img"></div>
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
  width: 100%;
  background: #fff;
  padding: 0px;
  border-radius: 10px;
  border: none;
}
.card .header {
  display: flex;
  align-items: center;
  justify-content: end;
  padding-right: 36px;
}
.header .img {
  height: 200px;
  width: 200px;
  background: #d9d9d9;
  border-radius: 50%;
  position: relative;
  overflow: hidden;
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
  width: 100%;
}
.description .line-3 {
  width: 100%;
}
.description .line-3 {
  width: 40%;
}
.description .line-4 {
  width: 70%;
  height: 24px;
}
.description .line-5 {
  width: 70%;
  height: 24px;
}
.description .line-6 {
  width: 70%;
  height: 24px;
}

.header .img::before,
.description .line::before{
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

@keyframes shimmer {
  0% {
    background-position: -450px 0;
  }
  100% {
    background-position: 450px 0;
  }
}
</style>
