<template>
    <div class="card">
    <div v-for="(item, index) in 5" class="description">
      <div class="line line-1"></div>
      <div class="line line-2"></div>
    </div>
  </div>
</template>
<script>
export default{
    name:"",
    data(){
        return{
            
        }
    }
}
</script>
<style scoped>
.card{
  width: 100%;
  border: none;
  background: transparent;
  padding: 10px 20px 30px 30px;
}

.card .description{
  display: flex;
  justify-content: space-between;
}
.description .line{
  background: #d9d9d9;
  border-radius: 10px;
  height: 22px;
  margin: 6px 0;
  overflow: hidden;
  position: relative;
}
.description .line-1{
  width: 50%;
}
.description .line-2{
  width: 20%;
}
.card .btns{
  display: flex;
}

.description .line::before,
.btns .btn::before{
  position: absolute;
  content: "";
  height: 100%;
  width: 100%;
  background-image: linear-gradient(to right, #d9d9d9 0%, rgba(0,0,0,0.07) 20%, #d9d9d9 40%, #d9d9d9 100%);
  background-repeat: no-repeat;
  background-size: 450px 400px;
  animation: shimmer 1s linear infinite;
}

@keyframes shimmer {
  0%{
    background-position: -450px 0;
  }
  100%{
    background-position: 450px 0;
  }
}
</style>