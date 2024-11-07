<template>
  <div class="report-overlay">
    <div class="report-content">
      <h2 class="text-center font-bold text-lg mb-4">{{ data.headline }}</h2>
      <table class="w-full">
        <thead>
          <tr>
            <th class="text-left">Comment</th>
            <th class="text-right">Duration</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(entry, index) in data.entries" :key="index">
            <td class="text-left">{{ entry.comment }}</td>
            <td class="text-right">{{ entry.duration }}</td>
          </tr>
        </tbody>
        <tfoot class="m-10">
          <tr>
            <td class="text-left font-bold pt-4">Total</td>
            <td class="text-right pt-4">{{ data.total }}</td>
          </tr>
        </tfoot>
      </table>
      <button @click="$emit('close')" class="close-button">Close</button>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, PropType } from 'vue'
  import type { ReportVM } from '../types/types'

  export default defineComponent({
    props: {
      data: {
        type: Object as PropType<ReportVM>,
        required: true
      }
    }
  })
</script>

<style>
  .report-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 2000;
  }

  .report-content {
    background-color: white;
    padding: 20px;
    border-radius: 10px;
    width: 90%;
    max-height: 95%;
    overflow-y: auto;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
  }

  .close-button {
    position: absolute;
    bottom: 10px;
    right: 10px;
    padding: 5px 10px;
    background-color: #3b82f6;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
  }
</style>
