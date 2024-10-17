export interface loginRequestDTO {
  email: string;
  password: string;
}

export interface registerRequestDTO {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
  profilePictureUrl?: string;
  preferredLanguage?: string;
  preferredCurrency?: string;
  roles: string[];
}

export interface updateRequestDTO {
  userID: string;
  username: string;
  oldPassword?: string;
  newPassword?: string;
}
