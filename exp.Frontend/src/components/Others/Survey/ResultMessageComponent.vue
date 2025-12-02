<template>
  <h3 class="fw-600 mb-3 px-3 px-sm-3">{{ surveyType == 'score' ? 'Results' : 'Result' }}</h3>
  <div
    class="white-box"
    :class="{ 'py-4': surveyType != 'score', 'align-items-center': surveyType == 'rule_check' }"
  >
    <div v-if="surveyType == 'score'" class="row container-score">
      <div class="col-12 col-xxl">
        <h6 v-for="(item, index) in report.OrganisationResult" class="result-row">
          {{ item.Category }}
          <span class="flex-align-center position-relative ps-5">
            <div
              @click="$emit('resultOxySuggestions', { Title: item.Category, Score: item.Result, CategoryOfScore:true })"
              v-if="$route.name != 'first-survey'"
              class="ai-icon"
            ></div>
            {{ item.Result }}
          </span>
        </h6>
      </div>
      <div class="col-12 col-xxl-auto px-5">
        <div class="score">
          <img width="1" class="rotate-90" src="@/assets/images/bar-result-1.svg" alt="" />
          <div class="score-circle">
            <svg width="100" height="100">
              <circle cx="50" cy="50" r="45" stroke="#ddd" stroke-width="5" fill="none" />
              <circle
                id="progress-bar"
                cx="50"
                cy="50"
                r="45"
                :stroke-dasharray="circumference"
                :stroke-dashoffset="dashOffset"
                stroke="#00aaff"
                stroke-width="5"
                fill="none"
                stroke-linecap="round"
                transform="rotate(-90 50 50)"
              />
              <text x="50%" y="45%" text-anchor="middle" fill="#666" font-size="18">Score</text>
              <text
                x="50%"
                y="70%"
                text-anchor="middle"
                fill="#00aaff"
                font-size="18"
                font-weight="600"
              >
                {{ result }}
              </text>
            </svg>
          </div>
          <img width="1" class="rotate-90" src="@/assets/images/bar-result-2.svg" alt="" />
        </div>
      </div>
      <div class="col-12 col-xxl mb-5 position-relative">
        <button
          type="button"
          class="btn btn-info btn-sm btn-oxy-result"
          @click="$emit('resultOxySuggestions', { Title: null, Score: result })"
        >
          <img src="@/assets/images/ai-icon.svg" alt="" /> Suggestions
        </button>
        <div class="result-chart" v-if="chartOptions.xaxis.categories" id="chart" :key="key">
          <apexchart
            type="radar"
            :options="chartOptions"
            :series="series"
            ref="radarChart"
          ></apexchart>
        </div>
      </div>
    </div>
    <div class="row h-100" v-if="surveyType == 'rule_check'">
      <img class="bg-blue-result" src="@/assets/images/blue-bg-result.png" alt="" />

      <div class="col-12 col-lg-6 h-100">
        <h4 class="fw-600 z-index-3">{{ result }}</h4>
        <button
          v-if="$route.name != 'first-survey'"
          type="button"
          class="btn btn-info btn-sm mt-3"
          @click="$emit('resultOxySuggestions', { Title: result, Score: 'no-score' })"
        >
          <img src="@/assets/images/ai-icon.svg" alt="" /> Suggestions
        </button>
      </div>
      <div class="col-12 col-lg-6 text-center">
        <img class="man-on-laptop" src="@/assets/images/man-on-laptop.svg" alt="" />
      </div>
    </div>
  </div>
</template>
<script>
import ApexCharts from 'vue3-apexcharts'
export default {
  name: '',
  components: {
    ApexCharts,
  },
  emits: ['resultOxySuggestions'],
  data() {
    return {
      key: 0,
      series: [],

      chartOptions: {
        chart: {
          height: 900,
          width: '100%',
          type: 'radar',
          events: {
            updated: (chartContext, config) => {
              this.ColorLast()
            },
          },
        },
        colors: ['#89CB17', '#0081C5'],
        responsive: [
          {
            breakpoint: 650,
            options: {
              plotOptions: {
                radar: {
                  size: 100,
                },
              },
              legend: {
                position: 'bottom',
              },
            },
          },
          {
            breakpoint: 1200,
            options: {
              plotOptions: {
                radar: {
                  size: 200,
                },
              },
              legend: {
                position: 'bottom',
              },
            },
          },
          {
            breakpoint: 1400,
            options: {
              plotOptions: {
                radar: {
                  size: 220,
                },
              },
              legend: {
                position: 'bottom',
              },
            },
          },
          {
            breakpoint: 1600,
            options: {
              plotOptions: {
                radar: {
                  size: 100,
                },
              },
              legend: {
                position: 'bottom',
              },
            },
          },
          {
            breakpoint: 1800,
            options: {
              plotOptions: {
                radar: {
                  size: 130,
                },
              },
              legend: {
                position: 'bottom',
              },
            },
          },
        ],

        yaxis: {
          labels: {
            style: {
              colors: '#929EAA',
              fontSize: '12px',
              fontWeight: 400,
              cssClass: 'custom-radar-y-labels',
            },
          },
          stepSize: 1,
        },
        xaxis: {
          categories: [],
          labels: {
            show: true,

            style: {
              colors: [
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
                '#14181F',
              ],
              fontSize: '12px',
              fontFamily: 'Roboto',
            },
          },
        },
        fill: {
          opacity: 1,
          type: 'gradient',
          gradient: {
            type: 'horizontal',
            gradientToColors: ['#FFffff', '#0081C5'],
            shadeIntensity: 0.2,
            opacityFrom: 0.2,
            opacityTo: 0.5,
            stops: [0, 90, 100],
          },
        },
        stroke: {
          show: true,
          width: 2,
          colors: ['#89CB17', '#0081C5'],
          dashArray: 0,
        },
        markers: {
          size: 4,
          hover: {
            size: 6,
          },
          strokeColors: ['#89CB17', '#0081C5'],
          strokeWidth: 1,
          colors: ['#fff'],
        },
        plotOptions: {
          radar: {
            size: 170,
            polygons: {
              strokeColor: '#89CB17',
              fill: {
                colors: ['#fff'],
              },
            },
          },
        },
       
      },
    }
  },
  props: {
    report: {},
    result: null,
    surveyType: '',
    maxScore: {
      type: Number,
      default: 4,
    },
  },
  computed: {
    percentage() {
      return (this.result / this.maxScore) * 100
    },
    circumference() {
      //length of circle
      const r = 45
      return 2 * Math.PI * r
    },
    dashOffset() {
      //how much from stroke is hide
      return this.circumference * (1 - this.percentage / 100)
    },
  },
  methods: {
    SplitText(str, maxLen = 20) {
      const words = str.split(' ')
      const lines = []
      let currentLine = ''
      for (const word of words) {
        if ((currentLine + ' ' + word).trim().length <= maxLen) {
          currentLine += (currentLine ? ' ' : '') + word
        } else {
          lines.push(currentLine)
          currentLine = word
        }
      }
      if (currentLine) {
        lines.push(currentLine)
      }
      return lines
    },
    GetSeries() {
      this.series = [
        {
          name: 'Series 1',
          data: [5, 5, 5, 5, 5, 5, 5, 5],
        },
        {
          name: 'Series 2',
          data: [1, 2, 3, 4, 5, 5, 5, 1],
        },
      ]
      const unformattedCategories = [
        ['1. Lifecycle', '3/4'],
        ['Confidentiality', '2/3'],
        ['3. Protection from', 'unathorized access', '2/3'],
        ['4. April', '2.44/3.15'],
        ['5. May', '2/3'],
        ['6. June', '2/3'],
        ['7. June', '2/3'],
        ['7. Protection from unathorized access', '2/3'],
      ]
      const formattedCategories = unformattedCategories.map((category) => {
        const [label, ...rest] = category
        const splitLabel = this.SplitText(label, 20)
        return [...splitLabel, ...rest]
      })
      this.chartOptions.xaxis.categories = formattedCategories
      this.key++
    },
    ColorLast() {
      const tspans = document.querySelectorAll('tspan:last-child')
      if (tspans.length) {
        tspans.forEach((tspan) => {
          const newX = parseFloat(tspan.getAttribute('x')) || 0
          const newY = parseFloat(tspan.getAttribute('dy')) || 0

          const text = tspan.textContent

          if (text.length >= 2) {
            const parent = tspan.parentNode

            const tspan1 = document.createElementNS('http://www.w3.org/2000/svg', 'tspan')
            tspan1.setAttribute('fill', '#0CA5EB')
            if (newX < 0) {
              tspan1.setAttribute('x', newX - 10)
            } else {
              tspan1.setAttribute('x', newX + 10)
            }

            const numbers = text.split('/')

            tspan1.setAttribute('dy', newY)
            tspan1.textContent = numbers[0]

            const tspan2 = document.createElementNS('http://www.w3.org/2000/svg', 'tspan')
            tspan2.setAttribute('fill', '#6F7C8E')
            tspan2.textContent = '/'

            const tspanRest = document.createElementNS('http://www.w3.org/2000/svg', 'tspan')
            tspanRest.setAttribute('fill', 'green')
            tspanRest.textContent = numbers[1]

            parent.removeChild(tspan)
            parent.appendChild(tspan1)
            parent.appendChild(tspan2)
            parent.appendChild(tspanRest)
          }
        })
      }
    },
  },
  mounted() {},
  watch: {
    report(newVal, oldVal) {
      this.series = [
        {
          Name: 'Series 1',
          data: newVal.DefaultResult.map((item) => item.Result),
        },
        {
          Name: 'Series 2',
          data: newVal.OrganisationResult.map((item) => item.Result),
        },
      ]

      const unformattedCategories = newVal.OrganisationResult.map((item) => [
        item.Category,
        `${item.Result}/4`,
      ])
      const formattedCategories = unformattedCategories.map((category) => {
        const [label, ...rest] = category
        const splitLabel = this.SplitText(label, 20)
        return [...splitLabel, ...rest]
      })

      const labelColorCount = formattedCategories.length
      const labelColors = Array(labelColorCount).fill('#14181F')

      this.$refs.radarChart.updateSeries(this.series)
      this.$refs.radarChart.updateOptions(
        {
          chart: {
            events: {
              updated: (chartContext, config) => {
                this.ColorLast()
              },
            },
          },
          xaxis: {
            categories: formattedCategories,
            labels: {
              show: true,
              style: {
                colors: labelColors,
                fontSize: '12px',
                fontFamily: 'Roboto',
              },
            },
          },
        },
        false,
        true,
      )
    },
  },
}
</script>
<style scoped>
.white-box {
  padding: 40px 24px;
  margin-bottom: 16px;
  min-height: 170px;
  transition: all 0.3s;
  position: relative;
}
@media (max-width: 576px) {
  .white-box {
    border-radius: 0px;
    padding: 16px;
  }
}
h6 {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 0px;
  border-top: 1px solid var(--neutral-50);
}
h6:first-of-type {
  border-top: none;
}
.grey-card {
  min-height: 84px;
  padding: 24px;
  background-color: var(--neutral-100);
  background-image: url("data:image/svg+xml,%3csvg width='100%25' height='100%25' xmlns='http://www.w3.org/2000/svg'%3e%3crect width='100%25' height='100%25' fill='none' rx='16' ry='16' stroke='%239C9C9CFF' stroke-width='2' stroke-dasharray='12%2c 16' stroke-dashoffset='8' stroke-linecap='round'/%3e%3c/svg%3e");
  border-radius: 16px;
  display: flex;
  align-items: center;
  margin-bottom: 16px;
}

h3 {
  font-size: 24px;
  color: var(--neutral-900);
}

.score {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: row;
  gap: 16px;
}
.score-circle {
  width: 100px;
  height: 100px;
}

@media (max-width: 1400px) {
  .rotate-90 {
    transform: rotate(-90deg);
    margin: 0px 60px;
  }
}

@media (min-width: 1400px) {
  .score {
    flex-direction: column;
  }
}

.container-score {
  min-height: 438px;
}
.man-on-laptop {
  position: relative;
  width: 100%;
  max-width: 496px;
}
.bg-blue-result {
  position: absolute;
  height: 100%;
  top: 0px;
  right: -12px;
}

.z-index-3 {
  position: relative;
  z-index: 3;
}
@media (min-width: 1400px) {
  .man-on-laptop {
    position: absolute;
    bottom: 0px;
    right: 80px;
  }
}

.btn-oxy-result {
  font-size: 13px;
  color: var(--primary-500);
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s;
  position: absolute;
  top: -16px;
  right: 20px;
  z-index: 2;
}

.result-row .ai-icon {
  position: absolute;
  left: 3px;
  top: -9px;
  opacity: 0;
  height: 0px;
  transition: all 0.3s;
  opacity: 1;
}

@media (min-width: 768px) {
  .result-row .ai-icon {
    opacity: 0;
  }
  .result-row:hover .ai-icon {
    opacity: 1;
  }
}
</style>
