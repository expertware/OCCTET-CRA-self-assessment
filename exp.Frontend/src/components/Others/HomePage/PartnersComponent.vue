<template>
  <div class="partners-section">
    <h1 class="title">PARTNERS</h1>

    <Carousel
    :autoplay="2000"
      :wrap-around="true"
      v-bind="settings"
      :breakpoints="breakpoints"
      class="carousel-partners"
      ref="myCarousel2"
    >
      <Slide v-for="(item, index) in list" :key="index">
        <a class="img-container" :href="item.Link" target="_blank"
          >
            <img class="img-carousel-partners" :src="GetImagePath(item.Name)" alt="" /></a>
      </Slide>
    </Carousel>
  </div>
</template>
<script>
import 'vue3-carousel/dist/carousel.css'
import { Carousel, Slide, Pagination, Navigation } from 'vue3-carousel'

export default {
  name: '',
  components: {
    Carousel,
    Slide,
    Pagination,
    Navigation,
  },
  data() {
    return {
      list: [
        { Name: 1, Link: 'https://www.eclipse.org/' },
        { Name: 2, Link: 'https://www.redalertlabs.com/' },
        { Name: 3, Link: 'https://www.digitalsme.eu/' },
        { Name: 4, Link: 'https://www.doubleopen.org/' },
        { Name: 5, Link: 'https://bitsea.de/' },
        { Name: 6, Link: 'https://www.expertware.net/' },
      ],
      breakpoints: {
        500: {
          itemsToShow: 2,
          snapAlign: 'center',
        },
        990: {
          itemsToShow: 3,
          snapAlign: 'center',
        },
        1600: {
          itemsToShow: 4,
          snapAlign: 'start',
        },
        1700: {
          itemsToShow: 6,
          snapAlign: 'start',
        },
      },
      settings: {
        itemsToShow: 1,
        snapAlign: 'center',
      },
      indexSlide: 0,
      indexEvent: 0,
    }
  },
  methods: {
    GetImagePath(item) {
      return new URL(`/src/assets/images/partners/${item}.png`, import.meta.url).href
    },
  },
}
</script>
<style scoped>
.partners-section {
  margin-top: 95px;
  padding-bottom: 50px;
}
.title {
  font-family: 'Roboto Condensed';
  font-weight: 600 !important;
  color: var(--neutral-900);
  text-align: center;
  margin-bottom: 24px;
  font-size: 36px;
  text-transform: uppercase;
}

.img-container {
  
  height: 168px;
  width: 100%;
  background-color: #fff;
  margin: 0px 16px;
  padding: 16px 24px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s;
}

.img-container:hover{
  box-shadow: -6px 10px 29px -1px #44515a29
}

.img-carousel-partners {
  width: 100%;
  max-width: 70%;
  max-height: 100px;
  object-fit: contain;
  
}


@media(min-width:768px){
  .partners-section {
  margin-top: 120px;
  padding-bottom: 120px;
}
}
@media (min-width: 1200px) {
  .title {
    font-size: 48px;
    margin-bottom: 56px;
  }
}
</style>
