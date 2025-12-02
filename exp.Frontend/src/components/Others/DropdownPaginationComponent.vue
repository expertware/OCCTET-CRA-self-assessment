<template>
  <div class="sm-custom dropdown">
    <button
      class="dropdown-toggle"
      type="button"
      data-bs-toggle="dropdown"
      aria-expanded="false"
    >
      {{ value }} items / page
    </button>
    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1" role="menu">
      <li v-for="option in options" :key="option">
        <div class="dropdown-item pointer" @click="ChangePageSize(option)">
          {{ option }}
        </div>
      </li>
    </ul>
  </div>
</template>
<script>
export default {
  name: "dropdownPagination",
  emits: ["changePageSize"],
  props:{
    pageSize:{
      type:Number,
      default(){
        return 8;
      }
    }
  },
  data() {
    return {
      options: [1,2, 3, 8, 16, 24],
      value: 8,
    };
  },
  methods: {
    ChangePageSize(option) {
      this.value = option;
      this.$emit("changePageSize", this.value);
    },

  },

  mounted(){
    if(this.pageSize){
      this.value = JSON.parse(JSON.stringify(this.pageSize));
    }

  },
};
</script>
<style scoped>
.sm-custom.dropdown {
  background-color: none;
  border-radius: 8px;
  height: 32px;
}

.sm-custom.dropdown button {
  border-radius: 8px;
  height: 32px;
  width: auto;
  height: 100%;
  border: 1px solid var(--neutral-200);
  background-color: transparent;
  padding-left: 8px;
  padding-right: 34px;
  position: relative;
}

.sm-custom.dropdown .dropdown-toggle::after {
  position: absolute;
  top: 6px;
  right: 10px;

  content: url("@/assets/images/arrow-select.svg");
  border: none;
}
</style>
