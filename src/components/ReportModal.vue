<template>
  <div class="report-overlay">
    <div class="report-content">
      <table>
        <thead>
          <tr>
            <th style="width: 100%; text-align: left">Comment</th>
            <th>Total Time</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(entry, index) in reportData" :key="index">
            <td>{{ entry.comment }}</td>
            <td>{{ formatTotalTime(entry.totalTime) }}</td>
          </tr>
        </tbody>
      </table>
      <button @click="$emit('close')" class="close-button">Close</button>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  import type { ReportEntry } from '../types/types';

  export default defineComponent({
    props: {
      reportData: {
        type: Array<ReportEntry>,
        required: true
      }
    },
    methods: {
      formatTotalTime(totalTime: number) {
        const hours = Math.floor(totalTime / 3600)
          .toString()
          .padStart(2, '0')
        const minutes = Math.floor((totalTime % 3600) / 60)
          .toString()
          .padStart(2, '0')
        const seconds = (totalTime % 60).toString().padStart(2, '0')
        return `${hours}:${minutes}:${seconds}`
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
