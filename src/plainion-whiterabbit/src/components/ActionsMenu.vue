<template>
  <div>
    <button @click="toggleMenu" class="menu-button">
      <font-awesome-icon icon="bars" />
    </button>

    <div v-if="showMenu" class="dropdown-menu">
      <button @click="openReport('day')">Daily Report</button>
      <button @click="openReport('week')">Weekly Report</button>
      <button @click="openReport('month')">Monthly Report</button>
    </div>
  </div>
  <ReportModal v-if="showReportModal" :reportData="reportData" @close="showReportModal = false" />
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue'
  import ReportModal from './ReportModal.vue' // Create this component for displaying reports
  import { TauriApi } from '../TauriApi'

  export default defineComponent({
    components: { ReportModal },
    setup() {
      const showReportModal = ref(false)
      const reportData = ref([])
      const showMenu = ref(false)

      function toggleMenu() {
        showMenu.value = !showMenu.value
      }

      async function openReport(period: string) {
        // Fetch report data from the backend based on the selected period
        const data = await TauriApi.invokePlugin<Array<Object>>({
          controller: 'home',
          action: 'generateReport',
          data: { period } // Can be 'day', 'week', or 'month'
        })

        showMenu.value = false
        reportData.value = data
        showReportModal.value = true
      }

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
