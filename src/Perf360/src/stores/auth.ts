import api from '@/api'
import type { User } from '@/models/user'
import { useStorage } from '@vueuse/core'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', () => {
  const token = useStorage<string | null>('token', null)
  const currentUserInformation: Ref<User | null> = ref(null)
  const isAuthenticated = computed(() => !!token.value)

  async function login(username: string, password: string) {
    const resp = await api.auth.getToken(username, password)
    token.value = resp.data.token
    await getCurrentUserInformation()
  }

  async function getCurrentUserInformation() {
    if (isAuthenticated.value) {
      const resp = await api.auth.getCurrentUserInformation()
      currentUserInformation.value = resp.data
    }
  }

  function logout() {
    token.value = null
    location.reload()
  }

  return {
    token,
    currentUserInformation,
    isAuthenticated,
    login,
    getCurrentUserInformation,
    logout,
  }
})
