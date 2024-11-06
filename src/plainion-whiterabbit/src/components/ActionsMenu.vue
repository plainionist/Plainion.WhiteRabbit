<template>
  <div>
    <button @click="toggleMenu" class="menu-button">
      <font-awesome-icon icon="bars" />
    </button>

    <div v-if="showMenu" class="dropdown-menu">
      <button @click="openReport('day')">Summary Day</button>
      <button @click="openReport('week')">Summary Week</button>
      <button @click="openReport('month')">Summary Month</button>
    </div>
  </div>
  <ReportModal v-if="showReportModal" :reportData="reportData" @close="showReportModal = false" />
</template>

<script lang="ts">
  import { defineComponent, ref, Ref } from 'vue'
  import ReportModal from './ReportModal.vue'
  import { TauriApi } from '../TauriApi'
  import { listen } from '@tauri-apps/api/event'

  export default defineComponent({
    components: { ReportModal },
    setup() {
      const showReportModal = ref(false)
      const reportData: Ref<Array<Object>> = ref([])
      const showMenu = ref(false)
      const selectedDate = ref(new Date())

      function toggleMenu() {
        showMenu.value = !showMenu.value
      }

      async function openReport(period: string) {
        const data = await TauriApi.invokePlugin<Array<Object>>({
          controller: 'home',
          action: 'report',
          data: { date: selectedDate.value, period }
        })

        showMenu.value = false
        reportData.value = data ?? []
        showReportModal.value = true
      }

      listen<string>('date-selected', async (event) => {
        selectedDate.value = new Date(event.payload)
      })

      return { showReportModal, reportData, openReport, showMenu, toggleMenu }
    }
  })
</script>

<style>
  .menu-button {
    margin-left: 5px;
    border: none;
    cursor: pointer;
    font-size: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .dropdown-menu {
    position: absolute;
    top: 40px; /* Adjust as needed */
    left: 0;
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 5px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
    display: flex;
    flex-direction: column;
    z-index: 1000;
  }

  .dropdown-menu button {
    background: transparent;
    border: none;
    padding: 10px 15px;
    cursor: pointer;
    text-align: left;
    width: 100%;
  }

  .dropdown-menu button:hover {
    background-color: #f0f0f0;
  }
</style>
