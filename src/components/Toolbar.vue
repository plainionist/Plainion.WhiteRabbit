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
  import { computed, defineComponent, ref, ShallowRef, useTemplateRef } from 'vue'
  import { TauriApi } from '../TauriApi'
  import DatePicker from './DatePicker.vue'
  import { emit } from '@tauri-apps/api/event'
  import ActionsMenu from './ActionsMenu.vue'
  import { listen } from '@tauri-apps/api/event'
  import { useCompactWindow } from '../composables/useCompactWindow'
  import { useTimer } from '../composables/useTimer'

  export default defineComponent({
    components: { DatePicker, ActionsMenu },
    setup() {
      const comment = ref('')
      const isToday = ref(true)
      const commentInput: Readonly<ShallowRef<HTMLElement | null>> = useTemplateRef('commentInput')

      const { minimizeWindow, restoreWindow } = useCompactWindow()
      const { elapsedTime, startTimer, stopTimer } = useTimer()

      const isTiming = computed(() => elapsedTime.value !== null);

      async function toggleTimer() {
        if (isTiming.value) {
          const startedTime = stopTimer()

          restoreWindow()

          await TauriApi.invokePlugin({
            controller: 'home',
            action: 'addActivity',
            data: {
              begin: startedTime,
              end: new Date(),
              comment: comment.value
            }
          })

          emit('measurement-stopped')
        } else {
          minimizeWindow()
          startTimer()
        }
      }

      listen<string>('date-selected', async (event) => {
        isToday.value = new Date(event.payload).toDateString() === new Date().toDateString()
        commentInput.value?.focus()
      })

      return { isTiming, comment, elapsedTime, toggleTimer, isToday }
    }
  })
</script>
