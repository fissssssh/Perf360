import type { AddUser, User } from '@/models/user'
import client from './client'

export default {
  async getUsers() {
    const resp = await client.get<User[]>('api/users')
    return resp.data
  },

  async createUser(user: AddUser) {
    const resp = await client.post<User>('api/users', user)
    return resp.data
  },

  async deleteUser(id: string) {
    const resp = await client.delete(`api/users/${id}`)
    return resp.data
  },

  async getUserRoles(id: string) {
    const resp = await client.get<string[]>(`api/users/${id}/roles`)
    return resp.data
  },

  async editUserRoles(id: string, roles: string[]) {
    const resp = await client.post(`api/users/${id}/roles`, roles)
    return resp.data
  },
}
