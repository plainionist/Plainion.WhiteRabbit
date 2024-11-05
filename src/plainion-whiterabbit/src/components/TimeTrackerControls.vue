<template>
  <div class="controls flex items-center gap-2 mb-2">
    <DatePicker v-model="selectedDate" @update="onDateChange" />
    <button @click="toggleTimer" class="flex items-center justify-center w-10 h-10 bg-blue-500 text-white rounded-full hover:bg-blue-700">
      <font-awesome-icon :icon="isTiming ? 'stop' : 'play'" />
    </button>
    <input
      v-model="comment"
      placeholder="Enter comment"
      class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-80"
    />
    <span v-if="isTiming" class="text-gray-600">{{ elapsedTime }}</span>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, computed } from 'vue'
  import { TauriApi } from '../TauriApi'
  import DatePicker from './DatePicker.vue'

  export default defineComponent({
    components: { DatePicker },
    setup() {
      const isTiming = ref(false)
      const comment = ref('')
      const startTime = ref<Date | null>(null)
      const selectedDate = ref(new Date())

      const elapsedTime = computed(() => {
        if (!isTiming.value || !startTime.value) return '00:00:00'
        const duration = new Date().getTime() - startTime.value.getTime()
        return new Date(duration).toISOString().substr(11, 8) // hh:mm:ss format
      })

      function onDateChange() {
        // Handle date change logic
      }

      async function toggleTimer() {
        if (isTiming.value) {
          startTime.value = null
          isTiming.value = false

          await TauriApi.invokePlugin<Array<Object>>({
            controller: 'home',
            action: 'stop',
            data: {
              startTime: startTime.value,
              stopTime: new Date(),
              comment: comment.value
            }
          })
        } else {
          startTime.value = new Date()
          isTiming.value = true
        }
      }

      return { isTiming, comment, elapsedTime, selectedDate, toggleTimer, onDateChange }
    }
  })
</script>
