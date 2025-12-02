<template>
    <div class="text-center mt-5 ">
        <img width="118" height="118" src="@/assets/images/not-found.webp" alt="">
        <h1 >{{text}}</h1>
    </div>
</template>
<script>
export default{
    name:"NotFound",
    props:{
        text:{
            type: String,
            default(){
                return 'Elements not found'
            }
        }
    },
    data(){
        return{
            
        }
    }
}
</script>
<style scoped>
h1{
    font-size: 24px;
    line-height: 26.4px;
    font-weight: 600;
    color: var(--neutral-900);
    margin-top: 24px;
    margin-bottom:80px;
}
</style>