<template>
  <div class="row">
    <div
      v-for="(annex, index) in annexes"
      class="col"
      :key="annex.id"
      :class="['annex mb-2', { 'is-active': currentIndex == index }]"
      @click="currentIndex = index; this.$refs.activeAnnex.scrollTop = 0"
    >
      {{ annex.Name }}
    </div>
  </div>
  <div ref="activeAnnex" v-if="annexes.length > 0" class="w-100 active-annex" id="gallery">
    <div v-html="annexes[currentIndex].Content"></div>
  </div>
</template>
<script>
import 'vue3-carousel/dist/carousel.css'
export default {
  name: '',
  props: {
    copyAnnexes: Array,
    activeAnnex: null,
  },
  data() {
    return {
      annexes: [],
      currentIndex: 0,
    }
  },
  methods: {
    ShowAnnex(id) {},
  },
  mounted() {
    const offcanvasAnnexDesktop = document.getElementById('offCanvasAnnexDesktop')
    offcanvasAnnexDesktop.addEventListener('hide.bs.offcanvas', () => {
      this.currentIndex=0
    })
    offcanvasAnnexDesktop.addEventListener('show.bs.offcanvas', () => {
       this.$refs.activeAnnex.scrollTop = 0
    })
    const offCanvasMobileAnnexes = document.getElementById('offCanvasMobileAnnexes')
    offCanvasMobileAnnexes.addEventListener('hide.bs.offcanvas', () => {
     this.currentIndex=0
    })
     offCanvasMobileAnnexes.addEventListener('show.bs.offcanvas', () => {
      this.$refs.activeAnnex.scrollTop = 0
    })
  },
  watch: {
    copyAnnexes(newVal, oldVal) {
      this.annexes = JSON.parse(JSON.stringify(newVal))
    },
    activeAnnex(newVal, oldVal) {
      this.currentIndex = newVal.Index
    },
  },
}
</script>
<style scoped>
.annex {
  width: 100%;
  font-size: 18px;
  display: flex;
  justify-content: center;
  cursor: pointer;
  opacity: 0.6;
  transition: all 0.3s ease-in-out;
}
.annex:hover {
  font-weight: 600;
}

.annex.is-active {
  opacity: 1;
  color: var(--primary-400);
  font-weight: 600;
}

.active-annex{
  height: 100%;
  overflow: auto;
}
</style>
