import client from './client'

export default {
  async delete(id: number) {
    const resp = await client.delete(`api/review-dimensions/${id}`)
    return resp.data
  },
}
