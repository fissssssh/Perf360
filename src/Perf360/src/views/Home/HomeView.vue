<template>
  <div class="h-full flex flex-col">
    <header>
      <nav class="border-b border-gray-300">
        <div class="container mx-auto flex items-center justify-between">
          <el-menu
            mode="horizontal"
            router
            :ellipsis="false"
            :default-active="router.currentRoute.value.path"
            style="--el-menu-bg-color: transparent; border: none"
          >
            <el-menu-item index="/dashboard">Dashboard</el-menu-item>
            <el-sub-menu index="/admin" v-auth="{ role: 'admin' }">
              <template #title>Admin</template>
              <el-menu-item index="/admin/reviews">Reviews</el-menu-item>
              <el-menu-item index="/admin/users">Users</el-menu-item>
              <el-menu-item index="/admin/review-roles">Review Roles</el-menu-item>
            </el-sub-menu>
          </el-menu>
          <user-menu></user-menu>
        </div>
      </nav>
    </header>
    <main class="flex-1 overflow-auto">
      <div class="container mx-auto py-8">
        <RouterView />
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import UserMenu from '@/components/UserMenu.vue'
const router = useRouter()
</script>
