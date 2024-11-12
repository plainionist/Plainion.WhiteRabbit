<template>
  <div class="grid-container pt-2">
    <AgGridVue
      class="ag-theme-alpine"
      :rowData="items"
      :columnDefs="columnDefs"
      :gridOptions="gridOptions"
      @cellValueChanged="onCellValueChanged"
      @rowSelected="onRowSelected"
    />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, Ref, onMounted } from 'vue'
  import { AgGridVue } from 'ag-grid-vue3'
  import { TauriApi } from '../TauriApi'
  import { emit, listen } from '@tauri-apps/api/event'
  import 'ag-grid-community/styles/ag-grid.css'
  import 'ag-grid-community/styles/ag-theme-alpine.css'
  import { faTrash } from '@fortawesome/free-solid-svg-icons'
  import { icon } from '@fortawesome/fontawesome-svg-core'
  import { GridOptions } from 'ag-grid-community'
  import { Activity } from '../types/types'

  export default defineComponent({
    components: { AgGridVue },
    setup() {
      function timeValueFormatter(params: any) {
        if (!params.value) return ''
        const date = new Date(params.value)
        return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false })
      }

      function durationFormatter(params: any) {
        if (!params.value) return ''
        return params.value.split(":").slice(0, 2).join(":")
      }

      function timeValueParser(params: any) {
        // User is expected to enter HH:mm
        const [hours, minutes] = params.newValue.split(':').map(Number)

        let date = new Date(selectedDate.value)
        date.setHours(hours)
        date.setMinutes(minutes)
        date.setSeconds(0)
        date.setMilliseconds(0)

        return date
      }

      const trashIconHtml = icon(faTrash).html[0]
      const columnDefs = ref([
        {
          headerName: 'Begin',
          field: 'begin',
          editable: true,
          resizable: false,
          suppressMovable: true,
          width: 100,
          valueFormatter: timeValueFormatter,
          valueParser: timeValueParser
        },
        {
          headerName: 'End',
          field: 'end',
          editable: true,
          resizable: false,
          suppressMovable: true,
          width: 100,
          valueFormatter: timeValueFormatter,
          valueParser: timeValueParser
        },
        {
          headerName: 'Duration',
          field: 'duration',
          editable: false,
          resizable: false,
          suppressMovable: true,
          width: 100,
          valueFormatter: durationFormatter
        },
        { headerName: 'Comment', field: 'comment', editable: true, resizable: false, suppressMovable: true, flex: 1 },
        {
          headerName: '',
          field: 'actions',
          resizable: false,
          suppressMovable: true,
          width: 50,
          cellRenderer: (params: any) => {
            const button = document.createElement('button')
            button.innerHTML = trashIconHtml
            button.onclick = () => onDeleteRow(params.data)
            return button
          }
        }
      ])
      const gridOptions: Ref<GridOptions> = ref({
        headerHeight: 35,
        rowHeight: 35,
        rowSelection: {
          mode: 'singleRow',
          checkboxes: false,
          enableClickSelection: true
        }
      })

      const items: Ref<Array<Activity>> = ref([])
      const selectedDate = ref(new Date())

      async function fetch() {
        let data = await TauriApi.invokePlugin<Array<any>>({
          controller: 'activities',
          action: 'get',
          data: { date: selectedDate.value.toLocaleDateString() }
        })
        
        data = (data ?? []).map((item, idx) => ({
          begin: item.begin ? new Date(item.begin) : null,
          end: item.end ? new Date(item.end) : null,
          duration: item.duration,
          comment: item.comment,
          idx
        }))

        const emptyRow: Activity = { idx: data.length, begin: null, end: null, comment: '' }
        items.value = [...data, emptyRow]
      }

      async function update() {
        await TauriApi.invokePlugin({
          controller: 'activities',
          action: 'update',
          data: {
            date: selectedDate.value,
            items: items.value
          }
        })

        // make sure that backend is the only master
        await fetch()
      }

      async function onCellValueChanged(params: any) {
        await update()

        // somehow still not working ...
        if (params.node.isSelected()) {
          params.node.setSelected(false)
          setTimeout(() => {
            params.node.setSelected(true)
          }, 150)
        } else {
          params.node.setSelected(true)
        }
      }

      async function onDeleteRow(rowData: any) {
        items.value = items.value.filter((item) => item.idx !== rowData.idx)
        update()
      }

      function onRowSelected(evt: any) {
        if (!evt.node.isSelected()) {
          return
        }

        const rowData = evt.node.data
        if (rowData.begin && !rowData.end) {
          emit('activity-selected', evt.data)
        } else {
          emit('activity-selected', null)
        }
      }

      listen('measurement-stopped', (evt: any) => {
        const activity = evt.payload as Activity

        if (activity.idx === -1) {
          items.value.push(activity)
        } else {
          items.value = items.value.map((item) => (item.idx === activity.idx ? activity : item))
        }

        update()
      })

      listen<string>('date-selected', async (evt) => {
        selectedDate.value = new Date(evt.payload)
        fetch()
      })

      onMounted(() => {
        fetch()
      })

      return { items, columnDefs, gridOptions, onCellValueChanged, onDeleteRow, onRowSelected }
    }
  })
</script>

<style scoped>
  .grid-container {
    flex-grow: 1;
    display: flex;
    flex-direction: column;
  }

  .ag-theme-alpine {
    width: 100%;
    flex-grow: 1;
  }
</style>
