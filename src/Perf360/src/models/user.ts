export interface User {
  id: string
  username: string
  nickname: string
  email: string
  phoneNumber: string
  roles: string[]
}

export interface AddUser {
  username?: string
  nickname?: string
  email?: string
  phoneNumber?: string
}
