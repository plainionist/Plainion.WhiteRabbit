<template>
  <div class="controls flex items-center gap-2 mb-2">
    <ActionsMenu />
    <DatePicker v-model="selectedDate" @update="onDateChange" />
    <button @click="toggleTimer" class="flex items-center justify-center w-10 h-10 bg-blue-500 text-white rounded-full hover:bg-blue-700">
      <font-awesome-icon :icon="isTiming ? 'stop' : 'play'" />
    </button>
    <input
      v-model="comment"
      placeholder="Enter comment"
      class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-72"
    />
    <span class="border border-gray-300 px-2 py-1 rounded" v-if="isTiming">{{ elapsedTime }}</span>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue'
  import { TauriApi } from '../TauriApi'
  import DatePicker from './DatePicker.vue'
  import { emit } from '@tauri-apps/api/event'
  import ActionsMenu from './ActionsMenu.vue'

  export default defineComponent({
    components: { DatePicker, ActionsMenu },
    setup() {
      const isTiming = ref(false)
      const comment = ref('')
      const startTime = ref<Date | null>(null)
      const selectedDate = ref(new Date())
      const elapsedTime = ref('00:00:00')
      let intervalId: number | null = null

      function formatElapsedTime(startTime: Date) {
        const now = new Date()
        const duration = now.getTime() - startTime.getTime()
        return new Date(duration).toISOString().substr(11, 8)
      }

      function startTimer() {
        intervalId = window.setInterval(() => {
          if (startTime.value) {
            elapsedTime.value = formatElapsedTime(startTime.value)
          }
        }, 1000)
      }

      function stopTimer() {
        if (intervalId !== null) {
          clearInterval(intervalId)
          intervalId = null
        }
      }

      function onDateChange() {
        // Handle date change logic
      }

      async function toggleTimer() {
        if (isTiming.value) {
          stopTimer()
          startTime.value = null
          isTiming.value = false
          elapsedTime.value = '00:00:00'

          await TauriApi.invokePlugin({
            controller: 'home',
            action: 'stop',
            data: {
              startTime: startTime.value,
              stopTime: new Date(),
              comment: comment.value
            }
          })
          emit('measurement-stopped')
        } else {
          startTime.value = new Date()
          isTiming.value = true
          startTimer()
        }
      }

      return { isTiming, comment, elapsedTime, selectedDate, toggleTimer, onDateChange }
    }
  })
</script>
