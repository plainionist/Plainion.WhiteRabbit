import { getCurrentWindow, LogicalPosition, LogicalSize } from '@tauri-apps/api/window'

export function useCompactWindow() {
  let originalSize: any = null
  let originalPosition: any = null

  async function minimizeWindow() {
    const window = getCurrentWindow()

    originalSize = await window.innerSize()
    originalPosition = await window.outerPosition()

    await window.setDecorations(false)
    await window.setAlwaysOnTop(true)
    await window.setSkipTaskbar(true)

    const panelHeight = 50
    await window.setSize(new LogicalSize(originalSize.width, panelHeight))
    await window.setPosition(new LogicalPosition(originalPosition.x, 0))
  }

  async function restoreWindow() {
    const window = getCurrentWindow()

    if (originalSize && originalPosition) {
      await window.setDecorations(true)

      await window.setSize(originalSize)
      await window.setPosition(originalPosition)
    }
  }

  return {
    minimizeWindow,
    restoreWindow
  }
}
