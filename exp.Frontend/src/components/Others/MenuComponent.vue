<template>
  <div class="top-menu px-5" :class="[{ scrolled: isScrolled }]">
    <img class="logo-occtet-big-screen" src="@/assets/images/logo-small.svg" alt="logo-occtet" />
    <div class="container">
      <menu>
        <img class="logo-occtet" src="@/assets/images/logo-small.svg" alt="logo-occtet" />
        <button
          type="button"
          v-for="(item, index) in menuItems"
          :key="item.Id"
          @click="$emit('scroll', item.Id)"
          :class="{ active: activeSection === item.Id }"
        >
          {{ item.Name }}
        </button>
      </menu>
    </div>
  </div>
  <div class="d-none-desktop" :class="  { 'open':showMobileMenu, 'scrolled':isScrolled || showMobileMenu}">
    <div class="header-mobile">
      <img src="@/assets/images/logo-small.svg" alt="logo-occtet" />
      <div class="menu-bars" @click="showMobileMenu = !showMobileMenu">
        <img src="@/assets/images/menu-bar.png" class="bar-1" />
        <img src="@/assets/images/menu-bar.png" class="bar-2" />
        <img src="@/assets/images/menu-bar.png" class="bar-3" />
      </div>
    </div>
    <div class="items-mobile">
   
        <button
          type="button"
          v-for="(item, index) in menuItems"
          :key="item.Id"
          @click="$emit('scroll-mobile', item.Id), showMobileMenu=!showMobileMenu"
          :class="{ active: activeSection === item.Id }"
        >
          {{ item.Name }}
        </button>
      
    </div>
  </div>
</template>
<script>
export default {
  name: '',
  emits: ['scroll', 'scroll-mobile'],
  data() {
    return {
      showMobileMenu: false,
      isScrolled: false,
      activeSection: null,
      menuItems: [
        {
          Name: 'Home',
          Id: 'home',
        },

        {
          Name: 'Register',
          Id: 'register',
        },

        {
          Name: 'Surveys',
          Id: 'surveys',
        },
        {
          Name: 'Why register',
          Id: 'whyRegister',
        },

        {
          Name: 'FAQ',
          Id: 'faq',
        },
        {
          Name: 'Contact',
          Id: 'contact',
        },
      ],
    }
  },
  mounted() {
    window.addEventListener('scroll', this.HandleScrollMenu)
    window.addEventListener('scroll', this.OnScrollSections)
    this.OnScrollSections()
  },
  beforeUnmount() {
    window.removeEventListener('scroll', this.HandleScrollMenu)
  },
  methods: {
    HandleScrollMenu() {
      this.isScrolled = window.scrollY > 0
    },
    OnScrollSections() {
      const scrollPos = window.scrollY + window.innerHeight / 2
      for (const item of this.menuItems) {
        const el = document.getElementById(item.Id)
        if (el) {
          const top = el.offsetTop
          const bottom = top + el.offsetHeight
          if (scrollPos >= top && scrollPos <= bottom) {
            this.activeSection = item.Id
            break
          }
        }
      }
    },
  },
}
</script>
<style scoped>
.top-menu {
  height: 88px;
  width: 100%;
  display: none;
  align-items: center;
  position: absolute;
  top: 0;
  z-index: 10;
  background-color: transparent;
  transition:
    background-color 0.3s ease,
    position 0.3s ease;
}

.top-menu.scrolled {
  position: fixed;
  background-color: #00304c;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.header-mobile {
  height: 68px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: absolute;
  top: 0px;
  z-index: 3;
  padding-left: 20px;
  padding-right: 20px;
  transition:
    width 0.5s ease,
    opacity 0.6s ease;
  
}


.logo-occtet-big-screen {
  display: none;
  position: absolute;
}
menu {
  display: flex;
  align-items: center;
  gap: 40px;
  padding: 0px;
}

menu button {
  background-color: transparent;
  border: none;
  color: #ffffff;
  opacity: 60%;
  cursor: pointer;
  font-size: 14px;
  line-height: 110.00000000000001%;
  font-weight: 300;
  letter-spacing: 0.4px;
}

menu button.active {
  color: white;
  opacity: 1;
  font-weight: 600;
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
  margin-top: 120px;
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

@media (min-width: 1690px) {
  .logo-occtet-big-screen {
    display: block;
  }
  .logo-occtet {
    display: none;
  }
}

.d-none-desktop{
  opacity: 1;
  height: 68px;
  width: 100%;
  display: flex;
  align-items: start;
  justify-content: space-between;
  position: absolute;
  top: 0px;
  z-index: 3;
  transition:
    height 0.5s ease,
    opacity 0.6s ease;
  background: transparent; 
  position: fixed;
}
.d-none-desktop.open {
height: 100vh;
opacity: 1;
}

.d-none-desktop.scrolled {
background: rgba(0, 51, 82, 0.9); 
  position: fixed;
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
}
@media (min-width: 992px) {
  .top-menu {
    display: flex;
  }
  .d-none-desktop{
    display: none;
  }

}
</style>
