<template>
  <div class="container-bubble-chart">
    <h3>Organization Registered</h3>
    <div ref="chart" class="chart-bubble"></div>
    <div v-if="bubbleData?.length == 0" class="w-100 flex">
      <div class="content">
        <img src="@/assets/images/not-found.webp" alt="" />
        <h4 class="fw-600">No organisation registered</h4>
      </div>
    </div>
  </div>
</template>
<script>
import * as echarts from 'echarts'
import { graphic } from 'echarts'
export default {
  name: '',
  data() {
    return {}
  },
  props: {
    bubbleData: {
      type: Array,
      default() {
        return []
      },
    },
  },
  methods: {
    InitialiseChart(dataResult) {
      if (!dataResult || dataResult.length === 0) return

      const data = dataResult
      const singlePoint = data.length === 1

      this.myChart = echarts.init(this.$refs.chart)
      this.myChart.showLoading()

      const values = data.map((d) => d[0])
      const max = Math.max(...values)
      const min = Math.min(...values)

      const normalize = (val, minSize = 50) => {
        if (max === min) return 60
        return minSize + val / 2
      }

      const xAxisLabels = data.map((d) => d[1])
      const markAreaData = []

      // if there are at least 2 values
      if (!singlePoint) {
        for (let i = 0; i < xAxisLabels.length - 1; i += 2) {
          markAreaData.push([
            {
              xAxis: xAxisLabels[i],
              itemStyle: {
                color: new graphic.LinearGradient(0, 0, 0, 1, [
                  { offset: 0, color: '#ffffff' },
                  { offset: 1, color: '#F5F7F8' },
                ]),
                shadowColor: '#C0C0C0',
                shadowBlur: 1,
                shadowOffsetX: 0,
                shadowOffsetY: 1,
              },
            },
            {
              xAxis: xAxisLabels[i + 1],
            },
          ])
        }
      }

      this.myChart.hideLoading()
      const option = {
        backgroundColor: '#ffffff',
        title: { show: false },
        legend: { show: false },
        grid: {
          left: '8%',
          top: '10%',
          right: '8%',
          bottom: '15%',
        },
        xAxis: {
          type: 'category',
          data: xAxisLabels,
          axisLabel: {
            fontSize: 10,
            color: '#7B8794',
            rotate: 45,
            align: 'right',
          },
          ...(singlePoint && {
            min: 0,
            max: 2,
            axisTick: { show: false },
            axisLabel: {
              formatter: () => xAxisLabels[0],
            },
          }),
        },
        yAxis: {
          type: 'value',
          scale: true,
          splitLine: { show: false },
        },
        tooltip: {
          trigger: 'item',
          formatter: function (params) {
            return `Data: ${params.value[0]}<br/>Count: ${params.value[1]}`
          },
        },
        series: [
          {
            type: 'scatter',
            data: data.map((d) => {
              const value = d[0]
              const date = d[1]
              const ratio = max === min ? 1 : (value - min) / (max - min)
              const green = Math.floor(210 - ratio * (210 - 100))
              const blue = Math.floor(255 - ratio * (255 - 200))
              const colorTop = `rgb(11, ${green}, ${blue},1)`
              const colorBottom = `rgb(11, ${Math.min(green, 255)}, ${Math.min(blue, 255)}, 0.5)`

              return {
                value: [date, value],
                symbolSize: normalize(value),
                itemStyle: {
                  color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                    { offset: 0, color: colorTop },
                    { offset: 1, color: colorBottom },
                  ]),
                  shadowBlur: 15,
                  shadowColor: 'rgba(0, 136, 204, 0.3)',
                },
              }
            }),
            label: {
              show: true,
              formatter: function (param) {
                return `${param.value[1]}`
              },
              color: '#fff',
              fontWeight: 'bold',
              fontSize: 15,
            },
            ...(markAreaData.length > 0 && {
              markArea: {
                data: markAreaData,
              },
            }),
          },
        ],
      }

      this.myChart.setOption(option)
    },
  },
  watch: {
    bubbleData: {
      handler(newVal) {
        if (!newVal || newVal.length === 0) {
          if (this.myChart) {
            this.myChart.dispose()
            this.myChart = null
          }
          return
        }
        this.InitialiseChart(newVal)
      },
      deep: true,
    },
  },
}
</script>
<style scoped>
.container-bubble-chart {
  height: 467px;
  width: 100%;
  border-radius: 16px;
  background-color: #ffffff;
  position: relative;
}
.chart-bubble {
  height: 360px;
  width: 100%;
  border-radius: 16px;
  overflow: hidden;
}
h3 {
  font-size: 24px;
  color: var(--neutral-900);
  font-weight: 600;
  padding: 24px;
  padding-bottom: 20px;
}
.content {
  position: absolute;
  text-align: center;
  top: 192px;
  color: var(--neutral-900);
}
</style>
