import type {
  AddReviewDimension,
  AddReviewRole,
  ReviewDimension,
  ReviewRole,
} from '@/models/review-role'
import client from './client'

export default {
  async getRoles() {
    const resp = await client.get<ReviewRole[]>('api/review-roles')
    return resp.data
  },

  async createRole(role: AddReviewRole) {
    const resp = await client.post('api/review-roles', role)
    return resp.data
  },

  async deleteRole(id: number) {
    const resp = await client.delete(`api/review-roles/${id}`)
    return resp.data
  },

  async getReviewTargets(id: number) {
    const resp = await client.get<ReviewRole[]>(`api/review-roles/${id}/review-targets`)
    return resp.data
  },

  async getReviewDimensions(id: number, targetRoleId: number) {
    const resp = await client.get<ReviewDimension[]>(
      `api/review-roles/${id}/review-targets/${targetRoleId}/dimensions`,
    )
    return resp.data
  },

  async createReviewDimensions(id: number, dimension: AddReviewDimension) {
    const resp = await client.post<ReviewDimension>(
      `api/review-roles/${id}/review-targets/${dimension.targetRoleId}/dimensions`,
      dimension,
    )
    return resp.data
  },
}
