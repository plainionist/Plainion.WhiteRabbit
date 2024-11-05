<template>
  <DatePicker
    v-model="selectedDate"
    @update="onDateChange"
    class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-32"
  />
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue'
  import DatePicker from 'vue3-datepicker'
  import { emit } from '@tauri-apps/api/event'

  export default defineComponent({
    components: { DatePicker },
    setup() {
      const selectedDate = ref(new Date())

      async function onDateChange() {
        emit('date-selected', selectedDate.value)
        console.log('date selected')
      }

      onMounted(() => {
        onDateChange()
      })

      return { selectedDate, onDateChange }
    }
  })
</script>
