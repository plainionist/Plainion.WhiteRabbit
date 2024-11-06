import { ref } from 'vue'

export function useTimer() {
  let startTime: Date | null = null
  const elapsedTime = ref('00:00:00')
  let intervalId: number | null = null

  function formatElapsedTime(startTime: Date) {
    const now = new Date()
    const duration = now.getTime() - startTime.getTime()
    return new Date(duration).toISOString().substr(11, 8)
  }

  function startTimer() {
    startTime = new Date()

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
    elapsedTime.value = '00:00:00'

    return startedTime
  }

  return {
    elapsedTime,
    startTimer,
    stopTimer
  }
}
