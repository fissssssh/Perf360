export interface User {
  id: string
  username: string
  email: string
  phoneNumber: string
  roles: string[]
}

export interface AddUser {
  username?: string
  email?: string
  phoneNumber?: string
}
