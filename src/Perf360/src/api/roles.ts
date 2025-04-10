import type { AddRole, Role } from '@/models/role'
import client from './client'

export default {
  async getRoles() {
    const resp = await client.get<Role[]>('api/roles')
    return resp.data
  },

  async createRole(role: AddRole) {
    const resp = await client.post('api/roles', role)
    return resp.data
  },

  async deleteRole(id: string) {
    const resp = await client.delete(`api/roles/${id}`)
    return resp.data
  },
}
