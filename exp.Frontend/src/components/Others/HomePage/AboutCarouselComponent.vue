<template>
  <div class="carousel-wrapper">
    <img class="bg" src="@/assets/images/carousel/corner-carousel.png" alt="" />
    <div class="wrapper-items">
      <div
        class="carousel-item"
        v-for="(item, i) in items"
        :key="i"
        :style="getItemStyle(i)"
        :class="{ active: i === activeIndex }"
      >
        <img class="" :src="ShowDynamicImage(item.Icon)" alt="" />

        <div class="label">{{ item.Label }}</div>
      </div>
    </div>
    <img
      ref="rotatingImage"
      class="polygon-transparent rotating-img"
      src="@/assets/images/carousel/polygon-transparent.png"
      alt=""
    />
  </div>
</template>

<script>
import conformity from '@/assets/images/carousel/conformity.svg'
import automated from '@/assets/images/carousel/automated.svg'
import oss from '@/assets/images/carousel/oss.svg'
import dependency from '@/assets/images/carousel/dependency.svg'
import compliance from '@/assets/images/carousel/compliance.svg'
import complianceCheck from '@/assets/images/carousel/compliance-check.svg'
const imageMap = {
  conformity,
  automated,
  oss,
  dependency,
  compliance,
  'compliance-check': complianceCheck,
}
export default {
  data() {
    return {
      currentAngle: 0,
      rotationInterval: null,
      items: [
        { Label: 'Specs Guide', Icon: 'conformity' },
        { Label: 'Assessment Tool', Icon: 'automated' },
        { Label: 'OSS Sharing Platform', Icon: 'oss' },
        { Label: 'Dependency Tools', Icon: 'dependency' },
        { Label: 'Reporting Tool', Icon: 'compliance' },
        { Label: 'Checklist', Icon: 'compliance-check' },
      ],
      angle: 0,
      activeIndex: 0,
      radius: 800,
      stepDeg: 60, // 360 / 6
      intervalId: null,
    }
  },
  methods: {
    startRotation() {
      this.rotationInterval = setInterval(() => {
        this.currentAngle += 60
        this.$refs.rotatingImage.style.transform = `rotate(${this.currentAngle}deg)`
      }, 3200)
    },
    getItemStyle(index) {
      const angleDeg = this.angle + index * this.stepDeg + 30
      const angleRad = (angleDeg * Math.PI) / 180
      const x = Math.cos(angleRad) * this.radius
      const y = Math.sin(angleRad) * this.radius

      const normalizedAngle = ((angleDeg % 360) + 360) % 360

      let opacity = 1

      if (normalizedAngle >= 150 && normalizedAngle <= 210) {
        opacity = 1 - (normalizedAngle - 150) / 60
      }

      return {
        transform: `translate(${x}px, ${y}px)`,
        opacity: opacity,
        normalizedAngle: normalizedAngle,
      }
    },

    stepRotate() {
      this.angle += this.stepDeg
      this.activeIndex = (this.activeIndex + 1) % this.items.length
    },
    startLoop() {
      this.intervalId = setInterval(() => {
        this.stepRotate()
      }, 3200) // 600ms animatie + 1s stop
    },
    ShowDynamicImage(image) {
      return imageMap[image] || ''
    },
  },
  
  mounted() {
    this.startLoop()
    setTimeout(()=>{
      this.startRotation();
    }, "200")
  },
  beforeUnmount() {
    clearInterval(this.intervalId);
    clearInterval(this.rotationInterval);
  },
}
</script>

<style scoped>
.carousel-wrapper {
  position: relative;
  width: 100%;
  height: 100%;
  margin: auto;
  background-repeat: no-repeat;
  background-size: contain;
  background-position: center center;
  z-index: 2;
}
.carousel-wrapper img.bg {
  width: 100%;
}

.wrapper-items {
  position: absolute;
  right: -200px;
  top: -180px;
}

.carousel-item {
  position: absolute;
  top: 50%;
  left: 50%;
  width: 234px;
  height: 234px;
  transform: translate(-50%, -50%);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  transition:
    transform 2s ease-in-out,
    opacity 1.2s ease;
  background-image: url('@/assets/images/carousel/polygon.png');
  background-repeat: no-repeat;
  background-size: 203px 228px;
  background-position: center center;
}

.carousel-item.active {
  transform: scale(1.1) translate(-50%, -50%);
}

.label {
  font-size: 15px;
  text-align: center;
  color: #0081c5;
  line-height: 110.00000000000001%;
  margin-top: 21px;
  font-weight: 600;
  max-width: 127px;
}

.items {
  position: absolute;
  top: -83px;
  left: 495px;
}

.polygon-transparent {
  position: absolute;
  top: -185px;
  right: -199px;
  z-index: 1;
}
.rotating-img {
  transition: transform 2s ease-in-out;
  transform-origin: center center;
}
</style>
