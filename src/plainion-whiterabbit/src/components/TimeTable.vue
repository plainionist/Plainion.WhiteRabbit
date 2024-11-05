<template>
  <Vue3EasyDataTable :items="items" :headers="headers" editable border-cell hide-footer />
</template>

<script lang="ts">
  import { defineComponent, ref, Ref } from 'vue'
  import Vue3EasyDataTable from 'vue3-easy-data-table'
  import type { Header, Item } from 'vue3-easy-data-table'
  import { TauriApi } from '../TauriApi'
  import 'vue3-easy-data-table/dist/style.css'
  import { listen } from '@tauri-apps/api/event'

  export default defineComponent({
    components: { Vue3EasyDataTable },
    setup() {
      const items: Ref<Item[]> = ref([])
      const headers: Ref<Header[]> = ref([
        { text: 'Begin', value: 'start' },
        { text: 'End', value: 'stop' },
        { text: 'Comment', value: 'comment' }
      ])
      const selectedDate = ref(new Date())

      async function fetch() {
        items.value =
          (await TauriApi.invokePlugin<Item[]>({
            controller: 'home',
            action: 'day',
            data: { date: selectedDate.value.toISOString() }
          })) ?? []
      }

      listen('measurement-stopped', () => {
        fetch()
      })

      listen<string>('date-selected', async (event) => {
        const localDate = new Date(event.payload)
        selectedDate.value = localDate
        fetch()
      })

      return { items, headers }
    }
  })
</script>
