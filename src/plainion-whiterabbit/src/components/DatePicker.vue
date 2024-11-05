<template>
  <DatePicker v-model="selectedDate" @update="onDateChange" 
    class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-32"/>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue'
  import DatePicker from 'vue3-datepicker'
  import { TauriApi } from '../TauriApi'

  export default defineComponent({
    components: { DatePicker },
    setup() {
      const selectedDate = ref(new Date())

      async function onDateChange() {
        return await TauriApi.invokePlugin<string>({ controller: 'home', action: 'day', data: selectedDate.value })
      }

      return { selectedDate, onDateChange }
    }
  })
</script>
