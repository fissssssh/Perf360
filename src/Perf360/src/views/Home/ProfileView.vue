<template>
  <div class="flex flex-col items-center gap-4">
    <el-avatar class="cursor-pointer">{{ authStore.currentUserInformation?.username }}</el-avatar>
    <div>{{ authStore.currentUserInformation?.nickname }}</div>
    <div>{{ authStore.currentUserInformation?.email }}</div>
    <div>{{ authStore.currentUserInformation?.phoneNumber }}</div>
    <div><el-button @click="changePasswordDialogVisile = true">Change Password</el-button></div>
  </div>
  <el-dialog v-model="changePasswordDialogVisile" title="Change Password">
    <el-form v-model="changePasswordForm" @submit.prevent>
      <el-form-item label="Old Password">
        <el-input v-model="changePasswordForm.oldPassword" type="password" />
      </el-form-item>
      <el-form-item label="New Password">
        <el-input v-model="changePasswordForm.newPassword" type="password" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="changePasswordDialogVisile = false">Cancel</el-button>
      <el-button type="primary" @click="changePassword" :disabled="!canConfirmChangePassword"
        >Confirm</el-button
      >
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import api from '@/api'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const changePasswordDialogVisile = ref(false)
const changePasswordForm: Ref<{ oldPassword?: string; newPassword?: string }> = ref({})
const canConfirmChangePassword = computed(
  () => !!changePasswordForm.value.oldPassword && !!changePasswordForm.value.newPassword,
)

async function changePassword() {
  if (canConfirmChangePassword.value) {
    await api.account.changePassword(
      changePasswordForm.value.oldPassword!,
      changePasswordForm.value.newPassword!,
    )

    authStore.logout()
  }
}
</script>
