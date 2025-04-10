<template>
  <div class="h-full w-full flex items-center justify-center">
    <el-card class="w-96">
      <template #header>
        <h1>Perf 360</h1>
      </template>
      <el-form>
        <el-form-item label="UserName">
          <el-input v-model="loginForm.username"></el-input>
        </el-form-item>
        <el-form-item label="Password">
          <el-input v-model="loginForm.password" type="password" show-password></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="flex justify-end">
          <el-button type="primary" @click="login">Login</el-button>
        </div>
      </template>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'

const props = defineProps<{ returnUrl?: string }>()
const loginForm: Ref<{ username: string; password: string }> = ref({ username: '', password: '' })
const authStore = useAuthStore()
const router = useRouter()

async function login() {
  await authStore.login(loginForm.value.username, loginForm.value.password)
  router.replace({ path: props.returnUrl ?? '/' })
}
</script>
