<template>
  <RouterView />
   <GlobalLoaderComponent v-if="$store.state.isLoading == true"></GlobalLoaderComponent>

</template>
<script >
import { RouterLink, RouterView } from 'vue-router';
import GlobalLoaderComponent from './components/Others/loaders/GlobalLoaderComponent.vue';
export default{
    name:"",
    components:{
      GlobalLoaderComponent, 
    },
    data(){
        return{
            
        }
    },
    mounted() {
    document.addEventListener("DOMContentLoaded", function () {
      document.addEventListener("hide.bs.modal", function (event) {
        if (document.activeElement) {
          document.activeElement.blur();
        }
      });
    });
  },
}

</script>

<style scoped></style>
