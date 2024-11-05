<template>
  <vue3-datepicker v-model="selectedDate" @update="onDateChange" />
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
