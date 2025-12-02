<template>
  <div class="top-menu">
    <router-link
      :to="{
        name: 'home',
      }"
    >
      <img
        class="logo-occtet absolute d-none-till-1700px"
        src="@/assets/images/logo-small.svg"
        alt="logo-occtet"
      />
    </router-link>
    <div v-if="this.$store.state.user.UserName && !this.$store.state.user.UserRoles.includes('Guest')" class="container-lg">
      <div class="flex-align-center gap-3">
        <router-link
          :to="{
            name: 'home',
          }"
        >
          <img
            class="logo-occtet d-none-after-1700px me-5"
            src="@/assets/images/logo-small.svg"
            alt="logo-occtet"
          />
        </router-link>
        <div class="flex-align-center-gap-3" v-for="(item, index) in menuItems">
          <router-link
            :to="{
              name: item.Link,
            }"
            v-if="HaveAccessOnPage(item.Roles)"
            class="menu-item d-none d-lg-flex"
          >
            <img :src="$utils.ShowDynamicImageSvg('', item.Icon)" alt="" />
            {{ item.Name }}
          </router-link>
        </div>
        <div class="flex-align-center justify-content-end gap-3 w-100">
          <div class="fs-13px color-white flex gap-2">
            <img src="@/assets/images/profile.svg" alt="" />

            {{ this.$store.state.user.UserName }}
          </div>
          <button type="button" class="btn" @click="Logout()" title="logout">
            <img src="@/assets/images/log-out.svg" alt="" />
          </button>
        </div>
      </div>
    </div>
  </div>

  <div class="mobile-menu" :class="{ open: showMobileMenu }">
    <div class="header-mobile-menu">
      <img src="@/assets/images/logo-small.svg" alt="logo-occtet" />
      <div class="menu-bars" @click="showMobileMenu = !showMobileMenu">
        <img src="@/assets/images/menu-bar.png" class="bar-1" />
        <img src="@/assets/images/menu-bar.png" class="bar-2" />
        <img src="@/assets/images/menu-bar.png" class="bar-3" />
      </div>
    </div>
    <div class="items-mobile">
      <div v-for="(item, index) in menuItems">
        <router-link
          type="button"
          :key="item.Id"
          :to="{ name: item.Link }"
          @click="showMobileMenu = !showMobileMenu"
          :class="{ active: item.Active.includes($route.name) }"
        >
          {{ item.Name }}
        </router-link>
      </div>
    </div>
  </div>
</template>
<script>
import token from '@/services/token.service'
import menuAdmin from '../../utils/menuAdmin.json'
export default {
  name: '',
  data() {
    return {
      showMobileMenu: false,
      menuItems: menuAdmin,
    }
  },
  methods: {
    HaveAccessOnPage(roles){
      const user = this.$store.getters.getUser
      const userRoles = user.UserRoles || []
      const matchingPage =roles.some((role) => userRoles.includes(role))

      if(matchingPage){
        return true
      }else{
        return false
      }
    },
    Logout() {
      this.$axios
        .post('/api/Auth/logout')
        .then(() => {
          token.removeAuthTokens()
          this.$store.dispatch('removeLoggedUser').then(() => {
            this.$router.push({ name: 'login' })
          })
        })
        .catch((error) => {
          this.$utils.ToastNotifySomethingWentWrong()
        })
    },
    ShowDynamicImage(defaultImg) {
      const defaultimg = `images/admin-${defaultImg}.svg`
      return new URL(`/src/assets/${defaultimg}`, import.meta.url).href
    },
  },
}
</script>
<style scoped>
.top-menu {
  height: 65px;
  width: 100%;
  background-color: #14181f;
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: fixed;
  z-index: 12;
}
.logo-occtet {
  width: 133px;
}

.logo-occtet.absolute {
  position: absolute;
  top: 12.19px;
  left: 40px;
}
.menu-item {
  display: flex;
  align-items: center;
  font-size: 13px;
  gap: 10px;
  padding: 10px 12px;
  line-height: 110.00000000000001%;
  color: #ffffff;
  border-radius: 8px;
  transition: all 0.3s;
  border: 1px solid transparent;
}
.menu-item:hover {
  border: 1px solid var(--primary-500);
}

a {
  color: var(--neutral-500);
}
.router-link-active {
  color: #ffffff;
}
.d-none-till-1700px {
  display: none;
}
.d-none-after-1700px {
  display: block;
}
@media (min-width: 1700px) {
  .d-none-till-1700px {
    display: block;
  }
  .d-none-after-1700px {
    display: none;
  }
}

/*-----------------------------------------------------------------mobile*/
.mobile-menu {
  height: 65px;
  width: 100%;
  background-color: #14181f;
  position: fixed;
  z-index: 12;
  transition: 0.4s;
}

.mobile-menu.open {
  height: 100vh;
  background-color: rgba(20, 24, 31, 0.85);
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
}
.header-mobile-menu {
  height: 68px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-left: 20px;
  padding-right: 20px;
}

.menu-bars {
  background: #ffffffcc;
  width: 36px;
  height: 36px;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 3px;
  cursor: pointer;
}

.menu-bars img {
  transition: 0.4s;
}
.open .bar-1 {
  -webkit-transform: rotate(-45deg) translate(-3px, 4px);
  transform: rotate(-45deg) translate(-3px, 4px);
}
.open .bar-2 {
  opacity: 0;
}
.open .bar-3 {
  -webkit-transform: rotate(45deg) translate(-3px, -4px);
  transform: rotate(45deg) translate(-3px, -4px);
}

.items-mobile {
  margin-top: 60px;
  display: flex;
  align-items: center;
  flex-direction: column;
  gap: 40px;
  z-index: 2;
  width: 100%;
  transition:
    width 0.5s ease,
    opacity 0.6s ease;
  opacity: 0;
  pointer-events: none;
}

.items-mobile button {
  background-color: transparent;
  padding: none;
  border: none;
  color: var(--neutral-500);
}

.items-mobile button.active {
  color: white;
  opacity: 1;
  font-weight: 600;
}

.open .items-mobile {
  opacity: 1;
  pointer-events: auto;
}

@media (min-width: 1000px) {
  .mobile-menu {
    display: none;
  }
  .router-link-active {
    background-color: var(--primary-500);
  }
}
</style>
