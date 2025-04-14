<template>
  <div class="flex justify-between items-center gap-4 mb-4">
    <el-button @click="router.back()">Back</el-button>
    <el-switch v-model="mode" active-text="Details" inactive-text="Simple"></el-switch>
  </div>
  <div class="grid gap-4 mb-4" :class="[mode ? 'grid-cols-1' : 'grid-cols-4']">
    <el-card v-for="(v, k) in records" :key="k" :header="k.toString()">
      <div>
        <el-rate
          :model-value="getAverageScore(v)"
          disabled
          show-score
          score-template="{value} points"
        ></el-rate>

        <el-table :data="v" v-show="mode">
          <el-table-column label="Reviewer" prop="reviewer"></el-table-column>
          <el-table-column label="Name" prop="name"></el-table-column>
          <el-table-column label="Score" prop="score"></el-table-column>
          <el-table-column label="Comment" prop="comment"></el-table-column>
        </el-table>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import api from '@/api'
import type { ReviewRecord } from '@/models/review'
import { groupBy, meanBy, round } from 'lodash'

const props = defineProps<{ id: number }>()
const router = useRouter()
const records: Ref<{ [key: string]: ReviewRecord[] }> = ref({})
const mode = ref(false)

onMounted(() => {
  api.reviews.getReviewRecords(props.id).then((r) => {
    records.value = groupBy(r, (x) => x.target)
  })
})

function getAverageScore(records: ReviewRecord[]) {
  return round(
    meanBy(records, (r) => r.score),
    2,
  )
}
</script>
