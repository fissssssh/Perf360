import router from '@/router'
import { useAuthStore } from '@/stores/auth'
import axios, { AxiosError } from 'axios'
interface ApiError {
  code?: string
  description?: string
}

const client = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  adapter: 'fetch',
  validateStatus(status) {
    return status < 400
  },
})

client.interceptors.request.use((request) => {
  const authStore = useAuthStore()
  if (authStore.isAuthenticated) {
    request.headers.Authorization = `Bearer ${authStore.token}`
  }
  return request
})

client.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError[]>) => {
    if (error.status === 401) {
      ElMessage.error({ message: 'Login expired' })
      const authStore = useAuthStore()
      authStore.logout()
      router.push({ name: 'login' })
    } else if (error.status === 403) {
      ElMessage.error({ message: 'Forbidden' })
      return Promise.reject(error)
    } else if (error.response?.data) {
      ElMessage.error({ message: JSON.stringify(error.response.data) })
      return Promise.reject(error)
    } else {
      ElMessage.error({ message: 'Unknow error' })
      return Promise.reject(error)
    }
  },
)

export default client
