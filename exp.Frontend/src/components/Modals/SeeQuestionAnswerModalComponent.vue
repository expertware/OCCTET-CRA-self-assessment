<template>
  <div
    class="modal fade"
    id="seeQuestionAnswer"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <img class="gif" src="@/assets/images/modal-gif.gif" alt="" />
        <div class="modal-header pb-0 p-md-5">
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
          <div class="card border-light-subtle shadow-sm w-100" style="margin-top: 40px">
            <div class="card-body text-align-start px-2 px-md-5">
              <h4 class="mb-2 text-muted">
                {{ displayedTextOxy }}
              </h4>
              <h5 class="fw-bold">{{ displayedTextQuestion }}</h5>
            </div>
          </div>
        </div>

        <div class="modal-body px-3 px-md-5 bg-white" ref="textContainer2">
          <div
            class="row align-items-center opacity-75 text-center text-md-start"
            v-if="!hideCarousel"
          >
            <div class="col-12 col-md-auto opacity-75">
              <img
                :src="ShowDynamicImageGif('', currentStep.icon)"
                alt="Computer man"
                style="width: 100px"
              />
            </div>
            <div class="col-12 col-md">
              <h3 class="mb-2 fw-700">{{ currentStep.label }}</h3>
              <p>{{ currentStep.description }}</p>
            </div>
          </div>
          <div class="oxy">
            <div ref="textContainer" class="color-N-900"></div>
          </div>
        </div>
        <div class="modal-footer bg-white border-0 flex">
          <div class="container-stars text-center">
            <img v-if="showStarsAi" width="70" src="@/assets/images/ai-icon-hover.gif" alt="" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import DOMPurify from 'dompurify'

export default {
  name: 'SeeQuestionAnswerModal',
  props: {
    question: null,
    option: null,
    oxyResponse: false,
  },

  data() {
    return {
      showStarsAi: false,
      hideCarousel: false,
      stopTyping: false,
      displayedTextOxy: null,
      displayedTextQuestion: null,
      typingTimeoutOxy: null,
      typingTimeoutQuestion: null,
      userScrolledUp: false,
      currentIndex: 0,
      intervalId: null,
      steps: [
        {
          icon: 'brain',
          title: 'Secure by Design',
          label: 'Thinking',
          description:
            "Hi, I'm Oxi! First, I’m analyzing your query carefully to spot any product risks and make sure everything fits within EU Cyber Resilience standards.",
        },
        {
          icon: 'open-book',
          title: 'Hunting for Vulnerabilities',
          label: 'Search',
          description:
            'Next, I’m diving into vulnerability databases, compliance rules, and technical docs to find the most current security info.',
        },
        {
          icon: 'validation',
          title: 'Validating Data',
          label: 'Validate',
          description:
            'I’m double-checking all the data I found to make sure nothing slips through the cracks.',
        },
        {
          icon: 'analyse',
          title: 'Assessing the Impact',
          label: 'Analyse',
          description:
            'Now, I’m figuring out how serious each vulnerability is and prioritizing what matters most, aligned with CRA security goals.',
        },
        {
          icon: 'recommendation',
          title: 'Generating Recommendations',
          label: 'Recommend',
          description:
            'I’m putting together smart recommendations to help you tackle those risks effectively.',
        },
        {
          icon: 'feedback',
          title: 'Incorporating Feedback',
          label: 'Refine',
          description:
            'If you’ve got feedback or new compliance info, I’ll adjust my answers so they stay spot on.',
        },
        {
          icon: 'image',
          title: 'Compliance Made Clear',
          label: 'Format',
          description:
            'I’ll deliver your answer in a clear, actionable way that keeps your CRA compliance journey on track.',
        },
        {
          icon: 'report',
          title: 'Final Reporting',
          label: 'Report',
          description:
            'Finally, I’ll prepare a detailed summary report for your records and audits.',
        },
      ],
    }
  },

  emits: ['reset'],

  methods: {
    ShowDynamicImageGif(imagePath, defaultImg) {
      if (!imagePath) {
        return new URL(`/src/assets/images/animated-icons/${defaultImg}.gif`, import.meta.url).href
      }
      return imagePath
    },

    ScrollToBottom(force = false) {
      this.$nextTick(() => {
        const el = this.$refs.textContainer2
        if (!el) return

        const threshold = 30
        const isNearBottom = el.scrollHeight - el.scrollTop - el.clientHeight < threshold

        if (!force && this.userScrolledUp && !isNearBottom) return

        el.scrollTo({ top: el.scrollHeight, behavior: 'smooth' })

        if (isNearBottom) this.userScrolledUp = false
      })
    },

    HandleCloseModal() {
      let modal = document.getElementById('seeQuestionAnswer')
      if (modal) {
        let app = this
        modal.addEventListener('hidden.bs.modal', function () {
          clearTimeout(app.typingTimeoutOxy)
          clearTimeout(app.typingTimeoutQuestion)
          app.stopTyping = true
          app.showStarsAi = false
          app.currentIndex = 0
          app.userScrolledUp = false
          app.$emit('reset')
          app.displayedTextOxy = null
          app.displayedTextQuestion = null
          app.$refs.textContainer.innerHTML = null
        })
      }
    },

    HandleOpenModal() {
      let modal = document.getElementById('seeQuestionAnswer')
      if (modal) {
        let app = this
        modal.addEventListener('show.bs.modal', function () {
          app.currentIndex = 0
          app.StartShowOxyMessage()
          app.hideCarousel = false
        })
      }
    },

    async StartShowText() {
      if (!this.option) return
      this.stopTyping = false
      this.showStarsAi = true
      this.hideCarousel = true

      const container = this.$refs.textContainer
      container.innerHTML = ''
      this.ScrollToBottom(true)

      const scrollContainer = this.$refs.textContainer2
      scrollContainer.addEventListener('scroll', () => {
        const nearBottom =
          scrollContainer.scrollHeight - scrollContainer.scrollTop - scrollContainer.clientHeight <
          30
        this.userScrolledUp = !nearBottom
      })

      const parser = new DOMParser()
      const cleanHTML = this.option.replace(/\\n/g, '\n').replace(/\\t/g, ' ').replace(/\\"/g, '"')
      const doc = parser.parseFromString(cleanHTML, 'text/html') //transform this.option in a dom document to go from node to node

      const typeNode = async (node, parent) => {
        if (this.stopTyping) return

        if (node.nodeType === Node.TEXT_NODE) {
          const span = document.createElement('span')
          parent.appendChild(span)
          const cleanText = DOMPurify.sanitize(node.textContent)
          let counter = 0
          for (let char of cleanText) {
            if (this.stopTyping) return
            span.textContent += char
            counter++
            if (counter % 5 === 0) {
              this.ScrollToBottom()
            }
            await new Promise((r) => setTimeout(r, 20))
          }
        } else if (node.nodeType === Node.ELEMENT_NODE) {
          const el = document.createElement(node.tagName)
          parent.appendChild(el)
          for (let attr of node.attributes) {
            el.setAttribute(attr.name, attr.value)
          }
          for (let child of Array.from(node.childNodes)) {
            await typeNode(child, el)
          }
        }
      }

      for (let child of Array.from(doc.body.childNodes)) {
        await typeNode(child, container)
        if (this.stopTyping) break
      }
      this.showStarsAi = false
    },
    StartShowOxyMessage() {
      const text = "Hello! I'm Oxy, your CRA assistant. I'm here to help with your question:"
      let index = 0
      this.displayedTextOxy = ''
      const typeNextChar = () => {
        if (index < text.length) {
          this.displayedTextOxy += text[index]
          index++
          this.typingTimeoutOxy = setTimeout(typeNextChar, 50)
        } else {
          this.StartShowQuestion()
        }
      }
      typeNextChar()
    },
    StartShowQuestion() {
      if (this.question) {
        const text = this.question
        let index = 0
        this.displayedTextQuestion = ''
        const typeNextChar = () => {
          if (index < text.length) {
            this.displayedTextQuestion += text[index]
            index++
            this.typingTimeoutQuestion = setTimeout(typeNextChar, 50)
          } else {
            if (!this.oxyResponse) this.StartShowText()
          }
        }
        typeNextChar()
      }
    },
    startCarousel() {
      if (!this.intervalId) {
        this.intervalId = setInterval(() => {
          this.currentIndex = (this.currentIndex + 1) % this.steps.length
        }, 5000)
      }
    },

    stopCarousel() {
      if (this.intervalId) {
        clearInterval(this.intervalId)
        this.intervalId = null
      }
    },
  },

  mounted() {
    this.HandleCloseModal()
    this.HandleOpenModal()
    this.startCarousel()
  },

  computed: {
    currentStep() {
      return this.steps[this.currentIndex]
    },
  },

  beforeUnmount() {
    this.stopCarousel()
  },

  watch: {
    option: {
      handler(newVal, oldVal) {
        this.stopCarousel()
        this.startCarousel()
        if (newVal !== oldVal && this.oxyResponse) {
          this.StartShowText()
        }
      },
      deep: true,
    },
  },
}
</script>

<style scoped>
.card-body {
  background: linear-gradient(180deg, #e0f2fe 0%, #ffffff 27.4%);
}
.separator {
  height: 1px;
  width: 100%;
  background-color: var(--neutral-200);
  margin: 12px 0px 24px 0px;
}

.modal-dialog {
  max-width: 900px;
  height: auto;
}


.modal-content::before {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: 12px;
  padding: 1px;
  background: linear-gradient(
    116.81deg,
    #ffffff 0%,
    rgba(255, 255, 255, 0.1) 19.23%,
    rgba(255, 255, 255, 0.7) 38.94%,
    rgba(255, 255, 255, 0.15) 60.58%,
    rgba(255, 255, 255, 0.75) 83.17%,
    rgba(255, 255, 255, 0.1) 100%
  );
  mask:
    linear-gradient(#000 0 0) content-box,
    linear-gradient(#000 0 0);
  mask-composite: exclude;
}

.modal-header {
  border-bottom: 0px;
  background-image: none;
  background-color: #ffffff;
  display: flex;
  flex-direction: column;
  text-align: center;
}

.gif {
  position: absolute;
  z-index: 2;
  width: 100px;
  top: -28px;
  left: 50%;
  transform: translateX(-50%);
}
.modal-content {
  border: none;
  padding: 15px;
  border-radius: 12px;
  position: relative;
  background: transparent;
  position: relative;
  height: 92dvh;
}

.btn-close {
  position: absolute;
  right: 33px;
  top: 35px;
}
.oxy {
  max-height: unset;
}
.modal-body {
  overflow-y: auto;
  max-height: 100vh;
}
@media (min-width: 768px) {
  .modal-body {
    overflow: auto;
    max-height: 40vh;
  }
  .btn-close {
    right: 47px;
    top: 47px;
  }
  .modal-header {
    background-color: transparent;
    background-image: url('@/assets/images/ai-modal-header.png');
    background-size: cover;
  }
  .gif {
    width: 172px;
    top: -71px;
  }
  .modal-content {
    padding: 24px;
    height: auto;
  }
}

.cursor {
  position: relative;
  width: 24em;
  margin: 0 auto;
  border-right: 2px solid rgba(255, 255, 255, 0.75);
  font-size: 30px;
  text-align: center;
  white-space: nowrap;
  overflow: hidden;
  transform: translateY(-50%);
}

/* Animation */
.typewriter-animation {
  animation:
    typewriter 5s steps(50) 1s 1 normal both,
    blinkingCursor 500ms steps(50) infinite normal;
}

@keyframes typewriter {
  from {
    width: 0;
  }

  to {
    width: 100%;
  }
}

@keyframes blinkingCursor {
  from {
    border-right-color: rgba(255, 255, 255, 0.75);
  }

  to {
    border-right-color: transparent;
  }
}

.text-opacity::after {
  content: '|';
  animation: blink 1s step-end infinite;
}

@keyframes blink {
  50% {
    opacity: 0;
  }
}

.question {
  font-size: 18px;
  font-weight: 600;
}

.oxi-text {
  background-color: #ffffff;
  backdrop-filter: blur(80px);
  box-shadow: 0px 14px 30px 0px rgba(61, 61, 61, 0.26);
  color: var(--neutral-900);
  font-style: italic;
  width: 100%;
  padding: 14px 19px;
  border-radius: 1px 8px 8px 8px;
  margin-bottom: 40px;
  margin-top: 88px;
  font-size: 18px;
}

/* h1, h2, h3, h4, h5, h6 {
    margin-bottom: .5rem!important;
    font-weight: bold!important;
}

.oxy ul{
     margin-bottom: 16px !important;
} */

.modal-footer {
  padding: 0px;
  border-top: 1px solid var(--neutral-200);
}
.container-stars {
  height: 70px;
  margin: 0px;
}
</style>
