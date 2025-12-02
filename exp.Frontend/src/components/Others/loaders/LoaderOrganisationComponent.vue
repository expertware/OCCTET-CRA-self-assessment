<template>
  <div class="card">
    <div class="header row">
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
        </div>
      </div>
      <div class="col-6 col-md-4 mb-3 mb-md-4">
        <div class="details">
          <span class="name"></span>
          <span class="about"></span>
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
  background-color: transparent;
  border: 0px;
  border-radius: 0px;
}

.header .img {
  height: 75px;
  width: 75px;
  background: #d9d9d9;
  border-radius: 50%;
  position: relative;
  overflow: hidden;
}

.details span {
  display: block;
  background: #d9d9d9;
  border-radius: 10px;
  overflow: hidden;
  position: relative;
}
.details .name {
  height: 15px;
  width: 60%;
}
.details .about {
  height: 13px;
  width: 100%;
  margin-top: 10px;
}

.header .img::before,
.details span::before {
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

@keyframes shimmer {
  0% {
    background-position: -450px 0;
  }
  100% {
    background-position: 450px 0;
  }
}
</style>
