export interface UserModel {

  email: string
  name: string
  password: string
  address: string
  zip_code: number
  cvr?: number
}

export interface LoginModel {

  username: string
  password: string

}
