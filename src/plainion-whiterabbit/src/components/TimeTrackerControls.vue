<template>
  <div class="controls">
    <vue3-datepicker v-model="selectedDate" @update="onDateChange" />
    <button @click="toggleTimer">{{ isTiming ? 'Stop' : 'Start' }}</button>
    <input v-model="comment" placeholder="Enter comment" />
    <span v-if="isTiming">{{ elapsedTime }}</span>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, computed } from 'vue'
  import { TauriApi } from '../TauriApi'

  export default defineComponent({
    setup() {
      const isTiming = ref(false)
      const comment = ref('')
      const startTime = ref<Date | null>(null)

      const elapsedTime = computed(() => {
        if (!isTiming.value || !startTime.value) return '00:00:00'
        const duration = new Date().getTime() - startTime.value.getTime()
        return new Date(duration).toISOString().substr(11, 8) // hh:mm:ss format
      })

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

      return { isTiming, comment, elapsedTime, toggleTimer }
    }
  })
</script>
