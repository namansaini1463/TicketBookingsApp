export interface loginRequestDTO {
  emailOrUsername: string;
  password: string;
}

export interface UserProfile {
  userID: string;
  username: string;
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

export interface registerRequestDTO {
  username: string;
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
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  profilePictureUrl: string;
  preferredLanguage: string;
  preferredCurrency: string;
  oldPassword: string;
  newPassword: string;
}
