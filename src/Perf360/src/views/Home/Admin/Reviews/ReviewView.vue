<template>
  <el-button @click="router.back()" class="mb-4">Back</el-button>
  <el-table :data="records">
    <el-table-column
      label="Reviewer"
      prop="reviewer"
      :filters="reviewerFilters"
      :filter-method="(v, r) => r.reviewer === v"
    ></el-table-column>
    <el-table-column
      label="Target"
      prop="target"
      :filters="targetFilters"
      :filter-method="(v, r) => r.target === v"
    ></el-table-column>
    <el-table-column label="Name" prop="name"></el-table-column>
    <el-table-column label="Description" prop="description"></el-table-column>
    <el-table-column label="Score" prop="score"></el-table-column>
    <el-table-column label="Comment" prop="comment"></el-table-column>
  </el-table>
</template>

<script setup lang="ts">
import api from '@/api'
import type { ReviewRecord } from '@/models/review'

const props = defineProps<{ id: number }>()
const records: Ref<ReviewRecord[]> = ref([])
const router = useRouter()
const reviewerFilters: Ref<{ text: string; value: string }[]> = ref([])
const targetFilters: Ref<{ text: string; value: string }[]> = ref([])

onMounted(() => {
  api.reviews.getReviewRecords(props.id).then((r) => {
    records.value = r
    const reviewers = new Set<string>()
    const targets = new Set<string>()

    for (const record of r) {
      reviewers.add(record.reviewer)
      targets.add(record.target)
    }

    reviewers.forEach((v) => reviewerFilters.value.push({ text: v, value: v }))
    targets.forEach((v) => targetFilters.value.push({ text: v, value: v }))
  })
})
</script>
