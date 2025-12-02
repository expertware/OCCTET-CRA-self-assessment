<template>
  <div class="container-lg p-0">
    <ul class="bread-crumbs mb-3 p-se-20-px">
      <li class="d-flex align-items-start">
        {{
          $route.name == '!first-survey' || this.$store.state.user.UserRoles.includes('Guest')
            ? 'Home'
            : 'Dashboard'
        }}
        / &nbsp
      </li>
      <li v-for="(item, index) in breadCrumbs" :key="index" class="d-flex align-items-start">
        <div v-if="index > 0"><h6>&nbsp / &nbsp</h6></div>
        <router-link
          :class="{ active: currentRouteName == item.to.name }"
          :to="{ name: item.to.name, params: item.to.params }"
        >
          {{ item.text }}
        </router-link>
      </li>
    </ul>
  </div>
</template>
<script>
export default {
  name: 'BreadcrumbComponent',
  data() {
    return {}
  },
  computed: {
    breadCrumbs() {
      if (typeof this.$route.meta.breadCrumb === 'function') {
        return this.$route.meta.breadCrumb.call(this, this.$route)
      }
      return this.$route.meta.breadCrumb
    },
    currentRouteName() {
      return this.$route.name
    },
  },
}
</script>
<style scoped>
ul {
  margin-bottom: 0px;
  padding: 0px;
}
ul li {
  color: var(--neutral-900);
  font-size: 12px;
  line-height: 13.2px;
}
a {
  color: var(--neutral-900);
  font-size: 12px;
  line-height: 13.2px;
}

.router-link-active {
  color: var(--primary-500);
}

.bread-crumbs {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
}
</style>
