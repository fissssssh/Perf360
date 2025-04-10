<template>
  <div class="flex items-center justify-between mb-4">
    <div class="space-x-4">
      <el-button @click="router.back()">Back</el-button>
      <el-select
        style="width: 120px"
        v-model="selectedTargetId"
        @change="switchPanel"
        @visible-change="onTargetSeletcVisibleChange"
      >
        <el-option
          v-for="target in targets"
          :key="target.id"
          :label="target.name"
          :value="target.id"
        />
      </el-select>
    </div>
    <el-button
      type="primary"
      :icon="Plus"
      @click="addReviewDimensionDialogVisible = true"
    ></el-button>
  </div>

  <el-table :data="dimensions" stripe>
    <el-table-column label="Name" prop="name"></el-table-column>
    <el-table-column label="Description" prop="description"></el-table-column>
    <el-table-column>
      <template #default="scope">
        <el-popconfirm
          :hide-after="0"
          :title="`Are you sure to delete dimension '${scope.row.name}'`"
          @confirm="deleteDemension(scope.$index, scope.row.id)"
        >
          <template #reference>
            <el-button type="danger" :icon="Delete"></el-button>
          </template>
        </el-popconfirm>
      </template>
    </el-table-column>
  </el-table>

  <el-dialog
    v-model="addReviewDimensionDialogVisible"
    title="Add dimension"
    @open="onAddReviewDimensionDialog"
  >
    <el-form v-model="addReviewDimensionForm" @submit.prevent>
      <el-form-item label="Target">
        <el-select v-model="addReviewDimensionForm.targetRoleId">
          <el-option
            v-for="role in reviewRoles"
            :key="role.id"
            :value="role.id"
            :label="role.name"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="Name">
        <el-input v-model="addReviewDimensionForm.name" />
      </el-form-item>
      <el-form-item label="Description">
        <el-input v-model="addReviewDimensionForm.description" type="textarea" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="addReviewDimensionDialogVisible = false">Cancel</el-button>
      <el-button type="primary" @click="createReviewDimension">Confirm</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import api from '@/api'
import type { AddReviewDimension, ReviewDimension, ReviewRole } from '@/models/review-role'
import { Delete, Plus } from '@element-plus/icons-vue'

const props = defineProps<{ id: number }>()
const router = useRouter()
const targets: Ref<ReviewRole[]> = ref([])
const selectedTargetId: Ref<number | undefined> = ref()
const dimensions: Ref<ReviewDimension[]> = ref([])

const addReviewDimensionDialogVisible = ref(false)
const reviewRoles: Ref<ReviewRole[]> = ref([])
const addReviewDimensionForm: Ref<AddReviewDimension> = ref({})

onMounted(() => {
  api.reviewRoles.getReviewTargets(props.id).then(async (r) => {
    targets.value = r
    if (targets.value.length > 0) {
      selectedTargetId.value = targets.value[0].id
      await api.reviewRoles.getReviewDimensions(props.id, selectedTargetId.value).then((r) => {
        dimensions.value = r
      })
    }
  })
})

async function onTargetSeletcVisibleChange(isOpen: boolean) {
  if (isOpen) {
    targets.value = await api.reviewRoles.getReviewTargets(props.id)
  }
}

function switchPanel() {
  if (!selectedTargetId.value) {
    return
  }

  api.reviewRoles.getReviewDimensions(props.id, selectedTargetId.value).then((data) => {
    dimensions.value = data
  })
}

async function onAddReviewDimensionDialog() {
  addReviewDimensionForm.value = {}
  reviewRoles.value = await api.reviewRoles.getRoles()
  if (selectedTargetId.value) {
    addReviewDimensionForm.value.targetRoleId = selectedTargetId.value
  }
}

async function createReviewDimension() {
  const dimension = await api.reviewRoles.createReviewDimensions(
    props.id,
    addReviewDimensionForm.value,
  )
  addReviewDimensionDialogVisible.value = false

  if (addReviewDimensionForm.value.targetRoleId === selectedTargetId.value) {
    dimensions.value.push(dimension)
  }
}

async function deleteDemension(index: number, id: number) {
  await api.reviewDimensions.delete(id)
  dimensions.value.splice(index, 1)
}
</script>
