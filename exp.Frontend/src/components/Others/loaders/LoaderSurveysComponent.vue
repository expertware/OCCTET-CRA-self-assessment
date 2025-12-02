<template>
    <div class="card">
    <div class="header">
      <div class="img"></div>
      <div class="details">
        <span class="name"></span>
        <span class="about"></span>
        <span class="about"></span>
        <span class="name mt-2"></span>
      </div>

    </div>
    
    <div class="btns">
      <div class="btn btn-1"></div>
      <div class="btn btn-2"></div>
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
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0 6px 20px #ddd;
}
.card .header{
  display: flex;
  align-items: center;
  width: 100%;
}
.header .img{
  height: 50px;
  width:50px;
  background: #d9d9d9;
  border-radius: 16px;
  position: relative;
  overflow: hidden;
  min-width: 50px;
}
.header .details{
  padding-left: 20px;
  width: 100%;
}
.details span{
  display: block;
  background: #d9d9d9;
  border-radius: 10px;
  overflow: hidden;
  position: relative;
}
.details .name{
  height: 15px;
  width: 50%;
}
.details .about{
  height: 24px;
  width: 100%;
  margin-top: 10px;
}
.card .btns{
    margin-top: 35px;
  display: flex;
}
.card .btns .btn{
  height: 45px;
  width: 100%;
  background: #d9d9d9;
  border-radius: 25px;
  position: relative;
  overflow: hidden;
}
.btns .btn-1{
  margin-right: 8px;
}
.btns .btn-2{
  margin-left: 8px;
}
.header .img::before,
.details span::before,
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
.header .img::before{
  background-size: 650px 600px;
}
.details span::before{
  animation-delay: 0.2s;
}
.btns .btn-2::before{
  animation-delay: 0.22s;
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