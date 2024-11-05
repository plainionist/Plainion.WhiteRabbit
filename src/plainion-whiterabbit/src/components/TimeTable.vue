<template>
  <AgGridVue
    class="ag-theme-alpine"
    style="width: 100%; height: 400px"
    :rowData="items"
    :columnDefs="columnDefs"
    @cellValueChanged="onCellValueChanged"
  />
</template>

<script lang="ts">
  import { defineComponent, ref, h } from 'vue'
  import { AgGridVue } from 'ag-grid-vue3'
  import { TauriApi } from '../TauriApi'
  import { listen } from '@tauri-apps/api/event'
  import 'ag-grid-community/styles/ag-grid.css'
  import 'ag-grid-community/styles/ag-theme-alpine.css'
  import { faTrash } from '@fortawesome/free-solid-svg-icons'
  import { icon } from '@fortawesome/fontawesome-svg-core'

  export default defineComponent({
    components: { AgGridVue },
    setup() {
      const trashIconHtml = icon(faTrash).html[0]

      const items = ref([])
      const columnDefs = ref([
        { headerName: 'Begin', field: 'start', editable: true, resizable: false, suppressMovable: true, width: 100 },
        { headerName: 'End', field: 'stop', editable: true, resizable: false, suppressMovable: true, width: 100 },
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
            button.onclick = () => {
              onDeleteRow(params.data)
            }
            return button
          }
        }
      ])
      const selectedDate = ref(new Date())

      async function fetch() {
        items.value =
          (await TauriApi.invokePlugin<Array<Object>>({
            controller: 'home',
            action: 'day',
            data: { date: selectedDate.value.toISOString() }
          })) ?? []
      }

      async function update() {
        await TauriApi.invokePlugin({
          controller: 'home',
          action: 'updateItem',
          data: items.value
        })
      }
      async function onCellValueChanged() {
        update()
      }

      async function onDeleteRow(rowData: any) {
        items.value = items.value.filter((item) => item !== rowData)
        update()
      }

      listen('measurement-stopped', () => {
        fetch()
      })

      listen<string>('date-selected', async (event) => {
        const localDate = new Date(event.payload)
        selectedDate.value = localDate
        fetch()
      })

      return { items, columnDefs, onCellValueChanged, onDeleteRow }
    }
  })
</script>
