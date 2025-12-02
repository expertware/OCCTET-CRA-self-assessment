<template>
    <div class="card">
    <div class="description">
      <div class="line line-2"></div>
      <div class="line line-2"></div>
      <div class="line line-2"></div>
      <div class="line line-2"></div>
      <div class="line line-2"></div>
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
  background: #fff;
  padding: 0px 30px;
  border-radius: 10px;
  border:none;

}

.description .line{
  background: #EFEFEF;
  border-radius: 10px;
  height: 40px;
  margin: 24px 0;
  overflow: hidden;
  position: relative;
}




.description .line::before,
.btns .btn::before{
  position: absolute;
  content: "";
  height: 100%;
  width: 100%;
background-image: linear-gradient(to right, #EFEFEF 0%, rgba(0,0,0,0.07) 20%, #EFEFEF 40%, #EFEFEF 100%);
  background-repeat: no-repeat;
  background-size: 100% 400px;
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
