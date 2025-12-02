<template>
  <div class="container-lg p-se-20-px">
    <div class="first-registration-section">
      <img class="img-2" src="@/assets/images/register-deco-2.png" alt="" />
      <img class="img-3" src="@/assets/images/register-deco-3.png" alt="" />
      <div class="row">
        <div class="col-12 col-lg-6 text-wrapper">
          <h1 class="title text-center text-md-start">First registration</h1>
          <h4>
            To begin your CRA compliance assessment, please register your organization. Only the
            legally authorized representative of your organization can complete this registration.
            
            

            Registration gives you access to the CRA self-assessment surveys, provides a unique
            access code to track your progress, and allows you to view and compare your results with
            other SMEs across the EU.
            <p class="mt-3">Important:</p>
            <ul>
              <li>Only one authorized legal representative can register a company.</li>
              <li>Once registered, further attempts will show the company already exists.In that case, request the unique access code from your representative.</li>

              <li>
                Keep this code safe — it’s required to view results and continue your CRA compliance
                journey.
              </li>
            </ul>
          </h4>
          <button
            class="btn btn-light fit-content"
            type="button"
            @click="$emit('scroll', 'register')"
          >
            Register
          </button>
        </div>
        <div class="col-12 col-lg-6 d-flex align-items-center justify-content-end z-index-1 mt-4 mt-lg-0">
          <video
            ref="myVideo"
            :src="videoSrc"
            controls
            muted
            playsinline
            class="h-auto rounded shadow w-100"
          >
            Your browser does not support the video tag.
          </video>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import videoFile from '@/assets/videos/occtet-overview.mp4'
export default {
  name: '',
  data() {
    return {
      videoSrc: videoFile,
    }
  },
  emits: ['scroll'],
  mounted(){
    const video= this.$refs.myVideo
    const observer = new IntersectionObserver(
      (entries)=>{
        const entry=entries[0]
        if (entry.isIntersecting) {
          video.currentTime = 1
          video.play()
        } else {
          video.pause()
        }
      },
      {
        threshold: 0.5, // 50% of the video must be visible
      },
    )
    observer.observe(video)
  }
}
</script>
<style scoped>
.first-registration-section {
  background: linear-gradient(138.2deg, #1f82be 0%, #0a4a71 100%);
  padding: 20px;
  box-shadow: 0px 12px 64px 0px #64879e29;
  border-radius: 24px;
  position: relative;
}

.text-wrapper {
  display: flex;
  flex-direction: column;
  gap: 30px;
  color: #ffffff;
}

.img-1 {
  position: absolute;
  bottom: 0px;
  left: 0px;
  border-radius: 24px;
}
.img-3 {
  position: absolute;
  top: 0px;
  right: 0px;
  border-radius: 24px;
}
.img-2 {
  position: absolute;
  bottom: 0px;
  right: 0px;
  border-radius: 24px;
}
.title {
  font-size: 36px;
  font-family: 'Roboto Condensed';
  font-weight: 600 !important;
}

@media (min-width: 1200px) {
  .title {
    font-size: 48px;
  }
}

@media(min-width:768px){
  .first-registration-section {
  padding: 60px;
}
}
</style>
