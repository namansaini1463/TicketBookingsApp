export interface loginRequestDTO {
  userNameOrEmail: string;
  password: string;
}

export interface registerRequestDTO {
  name: string;
  username: string;
  password: string;
  phoneNumber: string;
  roles: string[];
}