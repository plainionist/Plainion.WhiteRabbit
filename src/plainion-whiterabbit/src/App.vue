<template>
  <div class="p-1 container">
    <Toolbar />
    <TimeTable />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  import Toolbar from './components/Toolbar.vue'
  import TimeTable from './components/TimeTable.vue'

  export default defineComponent({
    components: {
      Toolbar,
      TimeTable
    }
  })
</script>

<style scoped>
  .container {
    display: flex;
    flex-direction: column;
    height: 100vh;
  }
</style>
