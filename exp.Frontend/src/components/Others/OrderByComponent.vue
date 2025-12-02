<template>
  <div >
    <img
      v-if="orderBy === value"
      src="@/assets/images/icon-ascendent.svg"
      alt="sort-ascendent"
    />
    <img
      v-else-if="orderBy === value + '_desc'"
      src="@/assets/images/icon-descendent.svg"
      alt="sort-descendent"
    />
  
    <img v-else src="@/assets/images/icon-null.svg" alt="no-sort" />
  </div>
  </template>
  <script>
  export default {
    name: "OrderBy",
    props: {
      value: {
        type: String,
        default() {
          return "name";
        },
      },
      orderBy: {
        type: String,
        default() {
          return "name";
        },
      },
    },
    data() {
      return {};
    },
  };
  </script>
  <style scoped>
  img{
    margin-right: 8px;
  }
  div{
    display: inline-block!important;
    width: 28px;

  }
</style>