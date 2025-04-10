<template>
  <header class="mb-4">
    <el-button @click="router.back()">Back</el-button>
  </header>
  <el-tabs type="border-card">
    <el-tab-pane v-for="(v, k) in records" :key="k">
      <template #label>
        <el-badge
          :value="v.filter((r) => r.score !== 0 && !r.score).length"
          type="warning"
          :offset="[8, 0]"
          :show-zero="false"
        >
          <span>{{ k }}</span>
        </el-badge>
      </template>
      <el-table :data="v">
        <el-table-column label="Name" prop="name"></el-table-column>
        <el-table-column label="Description" prop="description"></el-table-column>
        <el-table-column label="Score" prop="score">
          <template #default="scope">
            <el-input-number
              v-model="scope.row.score"
              :max="5"
              :step="0.1"
              :precision="1"
              @blur="updateScore(scope.row.id, scope.row.score)"
            />
          </template>
        </el-table-column>
        <el-table-column label="Comment" prop="comment">
          <template #default="scope">
            <el-input
              v-model="scope.row.comment"
              type="textarea"
              @blur="updateComment(scope.row.id, scope.row.comment)"
            />
          </template>
        </el-table-column>
      </el-table>
    </el-tab-pane>
  </el-tabs>
</template>

<script setup lang="ts">
import api from '@/api'
import type { ReviewRecord } from '@/models/review'
import { groupBy } from 'lodash'

const props = defineProps<{ id: number }>()
const router = useRouter()
const records: Ref<{ [key: string]: ReviewRecord[] }> = ref({})

onMounted(() => {
  api.dashboard.getMyReview(props.id).then((r) => {
    records.value = groupBy(r, (x) => x.target)
  })
})

async function updateScore(id: number, score: number) {
  await api.dashboard.patchMyReviewRecord(id, [{ path: '/score', op: 'replace', value: score }])
}
async function updateComment(id: number, comment: string) {
  await api.dashboard.patchMyReviewRecord(id, [{ path: '/comment', op: 'replace', value: comment }])
}
</script>
