import { type AddReview, type Review, type ReviewRecord } from '@/models/review'
import client from './client'

export default {
  async getReviews() {
    const resp = await client.get<Review[]>(`api/reviews`)
    return resp.data
  },

  async createReview(review: AddReview) {
    const resp = await client.post<Review>(`api/reviews`, review)
    return resp.data
  },

  async delete(id: number) {
    const resp = await client.delete(`api/reviews/${id}`)
    return resp.data
  },

  async getReviewRecords(id: number) {
    const resp = await client.get<ReviewRecord[]>(`api/reviews/${id}/review-records`)
    return resp.data
  },
}
