<template>
  <div class="controls flex items-center gap-2">
    <ActionsMenu />
    <DatePicker />
    <button
      @click="toggleTimer"
      class="flex items-center justify-center w-8 h-8 bg-blue-500 text-white rounded-full hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed disabled:opacity-50"
      :disabled="!isToday"
    >
      <font-awesome-icon :icon="isTiming ? 'stop' : 'play'" />
    </button>
    <input
      v-model="comment"
      placeholder="Enter comment"
      class="border border-gray-300 px-2 py-1 rounded focus:outline-none focus:border-blue-500 w-72 h-8"
      ref="commentInput"
    />
    <span class="border border-gray-300 px-2 py-1 rounded" v-if="isTiming">{{ elapsedTime }}</span>
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, ShallowRef, useTemplateRef } from 'vue'
  import DatePicker from './DatePicker.vue'
  import { emit } from '@tauri-apps/api/event'
  import ActionsMenu from './ActionsMenu.vue'
  import { listen } from '@tauri-apps/api/event'
  import { useCompactWindow } from '../composables/useCompactWindow'
  import { useTimer } from '../composables/useTimer'
  import { Activity } from '../types/types'

  export default defineComponent({
    components: { DatePicker, ActionsMenu },
    setup() {
      const comment = ref('')
      const isToday = ref(true)
      const commentInput: Readonly<ShallowRef<HTMLElement | null>> = useTemplateRef('commentInput')
      let selectedActivity: Activity | null = null

      const { minimizeWindow, restoreWindow } = useCompactWindow()
      const { elapsedTime, startTimer, stopTimer } = useTimer()

      const isTiming = computed(() => elapsedTime.value !== null)

      async function toggleTimer() {
        if (isTiming.value) {
          const startedTime = stopTimer()

          restoreWindow()

          if (selectedActivity) {
            selectedActivity.end = new Date()
            selectedActivity.comment = comment.value
          } else {
            selectedActivity = {
              idx: -1,
              begin: startedTime,
              end: new Date(),
              comment: comment.value
            }
          }

          await emit('measurement-stopped', selectedActivity)

          selectedActivity = null
        } else {
          minimizeWindow()
          comment.value = selectedActivity?.comment ?? comment.value
          startTimer(selectedActivity?.begin ?? new Date())
        }
      }

      listen<string>('date-selected', async (evt: any) => {
        isToday.value = new Date(evt.payload).toDateString() === new Date().toDateString()
        commentInput.value?.focus()
      })

      listen<string>('activity-selected', async (evt: any) => {
        if (!evt.payload) {
          selectedActivity = null
        } else {
          selectedActivity = {
            begin: evt.payload.begin ? new Date(evt.payload.begin) : null,
            end: evt.payload.end ? new Date(evt.payload.end) : null,
            comment: evt.payload.comment,
            idx: evt.payload.idx
          }
        }
      })

      return { isTiming, comment, elapsedTime, toggleTimer, isToday }
    }
  })
</script>
