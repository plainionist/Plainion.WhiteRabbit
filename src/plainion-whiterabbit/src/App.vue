<template>
  <div class="m-1">
    <TimeTrackerControls />
    <TimeTable />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  import TimeTrackerControls from './components/TimeTrackerControls.vue'
  import TimeTable from './components/TimeTable.vue'

  export default defineComponent({
    components: {
      TimeTrackerControls,
      TimeTable
    }
  })
</script>
