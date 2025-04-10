import type { ReviewRecord, Review } from '@/models/review'
import client from './client'
import type { Operation } from 'fast-json-patch'

export default {
  async getMyReviews() {
    const resp = await client.get<Review[]>('api/dashboard/my-reviews')
    return resp.data
  },

  async getMyReview(id: number) {
    const resp = await client.get<ReviewRecord[]>(`api/dashboard/my-reviews/${id}`)
    return resp.data
  },

  async patchMyReviewRecord(id: number, patchDoc: Operation[]) {
    const resp = await client.patch(`api/dashboard/my-review-records/${id}`, patchDoc)
    return resp.data
  },
}
