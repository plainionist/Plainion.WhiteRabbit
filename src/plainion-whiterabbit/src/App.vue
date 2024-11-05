<template>
  <div class="m-1">
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
