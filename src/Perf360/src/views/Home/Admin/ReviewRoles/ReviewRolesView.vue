<template>
  <el-button class="mb-4" type="primary" @click="addRoleDialogVisible = true"
    >Add Review Role</el-button
  >
  <el-table :data="reviewRoles" stripe>
    <el-table-column label="Name" prop="name" />
    <el-table-column>
      <template #default="scope">
        <router-link :to="`/admin/review-roles/${scope.row.id}/review-dimensions`" class="mr-3">
          <el-button :icon="List"></el-button>
        </router-link>
        <el-popconfirm
          :hide-after="0"
          :title="`Are you sure to delete role '${scope.row.name}'`"
          @confirm="deleteRole(scope.row.id)"
        >
          <template #reference>
            <el-button type="danger" :icon="Delete"></el-button>
          </template>
        </el-popconfirm>
      </template>
    </el-table-column>
  </el-table>

  <el-dialog v-model="addRoleDialogVisible" title="Add role">
    <el-form v-model="addRoleForm" @submit.prevent>
      <el-form-item label="Name">
        <el-input v-model="addRoleForm.name" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="addRoleDialogVisible = false">Cancel</el-button>
      <el-button type="primary" @click="createRole">Confirm</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import api from '@/api'
import type { AddReviewRole, ReviewRole } from '@/models/review-role'
import { Delete, List } from '@element-plus/icons-vue'
import { onMounted, type Ref } from 'vue'

const reviewRoles: Ref<ReviewRole[]> = ref([])
const addRoleDialogVisible = ref(false)
const addRoleForm: Ref<AddReviewRole> = ref({})

onMounted(() => {
  getRoles()
})

async function getRoles() {
  reviewRoles.value = await api.reviewRoles.getRoles()
}

async function createRole() {
  try {
    await api.reviewRoles.createRole(addRoleForm.value)
    await getRoles()
  } catch (error) {
    console.error(error)
  } finally {
    addRoleForm.value = {}
    addRoleDialogVisible.value = false
  }
}

async function deleteRole(id: number) {
  await api.reviewRoles.deleteRole(id)
  await getRoles()
}
</script>
