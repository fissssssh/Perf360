export interface ReviewRole {
  id: number
  name: string
}

export interface AddReviewRole {
  name?: string
}

export interface AddReviewDimension {
  targetRoleId?: number
  name?: string
  description?: string
}

export interface ReviewDimension extends AddReviewDimension {
  id: number
}
