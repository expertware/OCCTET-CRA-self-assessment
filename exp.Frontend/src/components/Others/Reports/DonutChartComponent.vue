<template><div ref="chart" style="height: 80%" class="flex"></div></template>
<script>
import * as echarts from 'echarts'
import { graphic } from 'echarts'
export default {
  name: '',
  props: {
    companySizes: {
      type: Array,
      deafault() {
        return []
      },
    },
  },
  data() {
    return {
      myChart: null,
    }
  },
  methods: {
    InitialiseChart() {
      this.myChart = echarts.init(this.$refs.chart)
      this.myChart.showLoading()
      const gradients = [
        new graphic.LinearGradient(0, 0, 1, 1, [
          { offset: 1, color: '#80BADE' },

          { offset: 0, color: '#0075BD' },
        ]),
        new graphic.LinearGradient(0, 0, 1, 1, [
          { offset: 1, color: '#85D2F5' },

          { offset: 0, color: '#1EACED' },
        ]),
        new graphic.LinearGradient(0, 0, 1, 1, [
          { offset: 1, color: '#A3D5F0' },

          { offset: 0, color: '#67BAE7' },
        ]),
        new graphic.LinearGradient(0, 0, 1, 1, [
          { offset: 1, color: '#DCF2FE' },

          { offset: 0, color: '#C3E9FE' },
        ]),
      ]
      const datas = this.companySizes.map((item) => ({ name: item.Name, value: item.Count }))

      const coloredData = datas.map((item, index) => ({
        ...item,
        itemStyle: {
          color: gradients[index % gradients.length],
        },
        gradientColor: gradients[index % gradients.length].colorStops[1].color,
      }))
      this.myChart.hideLoading()
      const option = {
        tooltip: {
          trigger: 'item',
        },
        legend: {
          show: false,
        },
        label: {
          show: true,
          position: 'outer',
          formatter: function (params) {
            const idx = params.dataIndex
            return `{name|${params.name.split(' ')[0]}}:   {p${idx}|${params.percent}%}`
          },
          fontFamily: 'Roboto',
          fontSize: 18,
          rich: {
            name: {
              color: '#14181F',
              fontSize: 18,
              fontFamily: 'Roboto',
            },
            ...Object.fromEntries(
              coloredData.map((item, index) => [
                `p${index}`,
                { color: '#0081C5', fontSize: 15, fontWeight: 'bold', lineHeight: 30, },
              ]),
            ),
          },
        },
        series: [
          {
            itemStyle: {
              borderWidth: 3,
              borderColor: '#fff',
            },
            type: 'pie',
            radius: ['55%', '90%'],
            center: ['50%', '50%'],
            startAngle: 0,
             labelLine: { length: 10, length2: 8 },
            endAngle: 360,
            label: {
              show: true,
              position: 'outer',
              fontSize: 15,
              fontFamily: 'Roboto',
              color: '#14181F',
            },

            data: coloredData,
          },
        ],
      }

      this.myChart.setOption(option)
    },
     handleResize() {
      if (this.myChart) {
        this.myChart.resize()
      }
    },
  },
  watch: {
    companySizes: {
      handler(newVal) {
        if (newVal) {
          this.InitialiseChart()
        }
      },
    },
  },
  mounted(){
    window.addEventListener('resize', this.handleResize)
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.handleResize)
  },
}
</script>
<style scoped></style>
