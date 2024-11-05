<template>
  <Vue3EasyDataTable :items="items" :headers="headers" editable />
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted, Ref } from 'vue'
  import Vue3EasyDataTable from 'vue3-easy-data-table'
  import type { Header, Item } from 'vue3-easy-data-table'
  import { TauriApi } from '../TauriApi'
  import 'vue3-easy-data-table/dist/style.css'

  export default defineComponent({
    components: { Vue3EasyDataTable },
    setup() {
      const items: Ref<Item[]> = ref([])
      const headers: Ref<Header[]> = ref([
        { text: 'Start Time', value: 'start' },
        { text: 'Stop Time', value: 'stop' },
        { text: 'Comment', value: 'comment' }
      ])

      onMounted(async () => {
        items.value = (await TauriApi.invokePlugin<Item[]>({ controller: 'home', action: 'day', data: new Date() })) ?? []
      })

      return { items, headers }
    }
  })
</script>
