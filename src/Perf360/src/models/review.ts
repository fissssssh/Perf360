export interface Review {
  id: number
  name: string
}

export interface Participant {
  userId: string
  username: string
  reviewRoleIds?: number[]
  reviewRoleName?: number
}

export interface AddReview {
  name: string
  participants: Participant[]
}

export interface ReviewRecord {
  id: number
  reviewId: number
  name: string
  description?: string
  reviewerId: string
  reviewer: string
  targetId: string
  target: string
  score?: number
  comment?: string
}
