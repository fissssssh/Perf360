<template>
  <header class="mb-4">
    <el-button type="primary" @click="addReviewDialogVisible = true">Add Review</el-button>
  </header>
  <el-table :data="reviews" stripe>
    <el-table-column label="Name">
      <template #default="scope">
        <router-link :to="`reviews/${scope.row.id}`">{{ scope.row.name }}</router-link>
      </template>
    </el-table-column>
    <el-table-column>
      <template #default="scope">
        <el-popconfirm
          :hide-after="0"
          :title="`Are you sure to delete review '${scope.row.name}'`"
          @confirm="deleteReview(scope.row.id)"
        >
          <template #reference>
            <el-button type="danger" :icon="Delete"></el-button>
          </template>
        </el-popconfirm>
      </template>
    </el-table-column>
  </el-table>
  <el-dialog v-model="addReviewDialogVisible" title="Add Review" @open="onAddReviewDialogOpen">
    <el-form v-model="addReviewForm" @submit.prevent>
      <el-form-item label="Name">
        <el-input v-model="addReviewForm.name" />
      </el-form-item>
      <el-form-item label="Participants">
        <el-select v-model="addReviewForm.participants" multiple value-key="userId">
          <el-option v-for="u in users" :key="u.userId" :label="u.username" :value="u"></el-option>
        </el-select>
        <div class="flex flex-col space-y-2 mt-2">
          <div
            class="flex items-center space-x-2"
            v-for="p in addReviewForm.participants"
            :key="p.userId"
          >
            <span>{{ p.username }}</span>
            <el-select style="width: 200px" multiple v-model="p.reviewRoleIds">
              <el-option
                v-for="r in reviewRoles"
                :key="r.id"
                :label="r.name"
                :value="r.id"
              ></el-option>
            </el-select>
          </div>
        </div>
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="addReviewDialogVisible = false">Cancel</el-button>
      <el-button type="primary" @click="createReview">Confirm</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import api from '@/api'
import type { AddReview, Participant, Review } from '@/models/review'
import type { ReviewRole } from '@/models/review-role'
import { Delete } from '@element-plus/icons-vue'

const reviews: Ref<Review[]> = ref([])

const users: Ref<Participant[]> = ref([])
const reviewRoles: Ref<ReviewRole[]> = ref([])
const addReviewDialogVisible = ref(false)
const addReviewForm: Ref<AddReview> = ref({ name: '', participants: [] })

onMounted(() => {
  api.reviews.getReviews().then((r) => {
    reviews.value = r
  })
})

async function onAddReviewDialogOpen() {
  users.value = (await api.users.getUsers()).map((u) => ({ userId: u.id, username: u.username }))
  reviewRoles.value = await api.reviewRoles.getRoles()
}

async function createReview() {
  await api.reviews.createReview(addReviewForm.value)
  addReviewDialogVisible.value = false
  reviews.value = await api.reviews.getReviews()
}

async function deleteReview(id: number) {
  await api.reviews.delete(id)
  reviews.value = await api.reviews.getReviews()
}
</script>
