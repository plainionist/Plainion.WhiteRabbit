<template>
  <DatePicker v-model="selectedDate" class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-32 h-8" />
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted, watch } from 'vue'
  import DatePicker from 'vue3-datepicker'
  import { emit } from '@tauri-apps/api/event'

  export default defineComponent({
    components: { DatePicker },
    setup() {
      const selectedDate = ref(new Date())

      watch(selectedDate, () => {
        onDateChange()
      })

      async function onDateChange() {
        emit('date-selected', selectedDate.value)
      }

      onMounted(() => {
        onDateChange()
      })

      return { selectedDate, onDateChange }
    }
  })
</script>
