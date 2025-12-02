<template>
  <div class="chart-container row">
    <div class="col-6">
      <h4 class="fs-20px fw-600 color-N-900 mb-4">
        Have you conducted a self-assessment against CRA requirements?
      </h4>
      <div class="custom-legend">
        <ul>
          <li v-for="(label, index) in chartOptions.labels" :key="index">
            <span class="marker" :style="{ backgroundColor: chartOptions.colors[index] }"></span>
            {{ label }}
            <span class="value">{{ chartOptions.series[index] }}</span>
          </li>
        </ul>
      </div>
    </div>
    <div class="col-6">
      <apexchart
        width="90%"
        class="d-flex justify-content-end"
        type="pie"
        :options="chartOptions"
        :series="chartOptions.series"
        ref="pieChart"
      ></apexchart>
    </div>
  </div>
</template>
<script>
import ApexCharts from 'vue3-apexcharts'

export default {
  name: '',
  data() {
    return {
      chartOptions: {
        series: [44, 55, 13],
        colors: ['#1684CD', '#89CB17', '#5EC0FF'],
        chart: {
          type: 'pie',
          dropShadow: {
            enabled: true,
            top: 3,
            left: 0,
            blur: 4,
            opacity: 0.2,
          },
          animations: {
            enabled: true,
            speed: 800,
            animateGradually: {
              enabled: true,
              delay: 150,
            },
            dynamicAnimation: {
              enabled: true,
              speed: 350,
            },
          },
        },
        legend: {
          show: false,
          position: 'left',
          fontSize: '15px',
          fontWeight: 400,
          fontFamily: 'Roboto',
          formatter: undefined,

          labels: {
            colors: ['#14181F'],
          },
          markers: {
            size: 9,
            shape: 'square',
            strokeWidth: 2,
            fillColors: undefined,
            customHTML: undefined,
            onClick: undefined,
            offsetX: 0,
            offsetY: 0,
          },
        },
        fill: {
          opacity: 1,
          type: 'gradient',
          gradient: {
            type: 'vertical',
            gradientToColors: ['#033858', '#61960D', '#0284D4'],

            shade: 'light',
            shadeIntensity: 0.2,
            inverseColors: true,
            opacityFrom: 0.9,
            opacityTo: 0.9,
            stops: [0, 100],
          },
        },
        states: {
          hover: {
            filter: {
              type: 'none',
              value: 0.2,
            },
          },
        },
        labels: ['No', 'Yes', 'Planning to do so'],
        responsive: [
          {
            breakpoint: 480,
            options: {
              legend: {
                position: 'left',
              },
            },
          },
        ],
      },
    }
  },
  components: {
    ApexCharts,
  },
  methods: {},
}
</script>
<style scoped>
.chart-legend-wrapper {
  display: flex;
  justify-content: space-between;
}

.custom-legend ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.custom-legend li {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
  color: var(--neutral-900);
}

.custom-legend .marker {
  width: 16px;
  height: 16px;
  border-radius: 4px;
  margin-right: 8px;
}

.custom-legend .value {
  margin-left: auto;
  background: var(--primary-200);
  color: var(--primary-600);
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 13px;
  font-weight: 700;
}

.apexcharts-tooltip {
  border: 2px solid #ea9d19 !important;
  border-radius: 8px;
  box-shadow: 0px 2px 8px rgba(0, 0, 0, 0.1);
}
</style>
