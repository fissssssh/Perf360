<template>
  <el-table :data="reviews" stripe>
    <el-table-column label="Name">
      <template #default="scope">
        <router-link :to="`/dashboard/my-reviews/${scope.row.id}`">{{
          scope.row.name
        }}</router-link>
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup lang="ts">
import api from '@/api'
import type { Review } from '@/models/review'

const reviews: Ref<Review[]> = ref([])

onMounted(() => {
  api.dashboard.getMyReviews().then((r) => (reviews.value = r))
})
</script>
