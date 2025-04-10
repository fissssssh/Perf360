<template>
  <el-dropdown>
    <el-avatar class="cursor-pointer">{{ authStore.currentUserInformation?.username }}</el-avatar>
    <template #dropdown>
      <el-dropdown-menu>
        <el-dropdown-item @click="authStore.logout">Logout</el-dropdown-item>
      </el-dropdown-menu>
    </template>
  </el-dropdown>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
</script>
