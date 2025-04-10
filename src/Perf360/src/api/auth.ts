import type { User } from '@/models/user'
import client from './client'

export default {
  getToken(username: string, password: string) {
    return client.post<{ token: string }>(
      'api/token',
      { username, password },
      { headers: { 'Content-Type': 'multipart/form-data' } },
    )
  },

  getCurrentUserInformation() {
    return client.get<User>('api/current-user-information')
  },
}
