<template>
  <div class="controls flex items-center gap-2 mb-2">
    <ActionsMenu />
    <DatePicker />
    <button
      @click="toggleTimer"
      class="flex items-center justify-center w-10 h-10 bg-blue-500 text-white rounded-full hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed disabled:opacity-50"
      :disabled="!isToday"
    >
      <font-awesome-icon :icon="isTiming ? 'stop' : 'play'" />
    </button>
    <input
      v-model="comment"
      placeholder="Enter comment"
      class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-72"
      ref="commentInput"
    />
    <span class="border border-gray-300 px-2 py-1 rounded" v-if="isTiming">{{ elapsedTime }}</span>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, ShallowRef, useTemplateRef } from 'vue'
  import { TauriApi } from '../TauriApi'
  import DatePicker from './DatePicker.vue'
  import { emit } from '@tauri-apps/api/event'
  import ActionsMenu from './ActionsMenu.vue'
  import { listen } from '@tauri-apps/api/event'
  import { getCurrentWindow, LogicalPosition, LogicalSize, currentMonitor } from '@tauri-apps/api/window'

  export default defineComponent({
    components: { DatePicker, ActionsMenu },
    setup() {
      const isTiming = ref(false)
      const comment = ref('')
      const startTime = ref<Date | null>(null)
      const isToday = ref(true)
      const elapsedTime = ref('00:00:00')
      let intervalId: number | null = null
      const commentInput: Readonly<ShallowRef<HTMLElement | null>> = useTemplateRef('commentInput')

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

      async function toggleTimer() {
        if (isTiming.value) {
          stopTimer()

          undoSlim()

          await TauriApi.invokePlugin({
            controller: 'home',
            action: 'addActivity',
            data: {
              begin: startTime.value,
              end: new Date(),
              comment: comment.value
            }
          })

          emit('measurement-stopped')

          startTime.value = null
          isTiming.value = false
          elapsedTime.value = '00:00:00'
        } else {
          makeSlim()

          startTime.value = new Date()
          isTiming.value = true

          startTimer()
        }
      }

      listen<string>('date-selected', async (event) => {
        isToday.value = new Date(event.payload).toDateString() === new Date().toDateString()
        commentInput.value?.focus()
      })

      let originalSize: any = null
      let originalPosition: any = null

      async function makeSlim() {
        const window = getCurrentWindow()

        originalSize = await window.innerSize()
        originalPosition = await window.outerPosition()

        await window.setDecorations(false)

        const panelHeight = 50
        await window.setSize(new LogicalSize(originalSize.width, panelHeight))
        await window.setPosition(new LogicalPosition(originalPosition.x, 0))
      }

      async function undoSlim() {
        const window = getCurrentWindow()

        if (originalSize && originalPosition) {
          await window.setDecorations(true)

          await window.setSize(originalSize)
          await window.setPosition(originalPosition)
        }
      }

      return { isTiming, comment, elapsedTime, toggleTimer, isToday }
    }
  })
</script>
