import { ref, Ref } from 'vue'

export function useTimer() {
  let startTime: Date | null = null
  const elapsedTime: Ref<String | null> = ref(null)
  let intervalId: number | null = null

  function formatElapsedTime(startTime: Date) {
    const now = new Date()
    const duration = now.getTime() - startTime.getTime()
    return new Date(duration).toISOString().slice(11, 19)
  }

  function startTimer() {
    startTime = new Date()
    elapsedTime.value = '00:00:00'

    intervalId = window.setInterval(() => {
      if (startTime) {
        elapsedTime.value = formatElapsedTime(startTime)
      }
    }, 1000)
  }

  function stopTimer() {
    if (intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
    }

    const startedTime = startTime

    startTime = null
    elapsedTime.value = null

    return startedTime
  }

  return {
    elapsedTime,
    startTimer,
    stopTimer
  }
}
