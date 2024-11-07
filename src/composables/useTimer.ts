import { ref, Ref } from 'vue'

export function useTimer() {
  let stopwatch: Date | null = null
  const elapsedTime: Ref<String | null> = ref(null)
  let intervalId: number | null = null

  function formatElapsedTime(time: Date) {
    const now = new Date()
    const duration = now.getTime() - time.getTime()
    return new Date(duration).toISOString().slice(11, 19)
  }

  function startTimer(startTime: Date) {
    stopwatch = startTime
    elapsedTime.value = '00:00:00'

    intervalId = window.setInterval(() => {
      if (stopwatch) {
        elapsedTime.value = formatElapsedTime(startTime)
      }
    }, 1000)
  }

  function stopTimer() {
    if (intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
    }

    const startedTime = stopwatch

    stopwatch = null
    elapsedTime.value = null

    return startedTime
  }

  return {
    elapsedTime,
    startTimer,
    stopTimer
  }
}
