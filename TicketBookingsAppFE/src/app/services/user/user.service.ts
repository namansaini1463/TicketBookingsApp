import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private USER_ID_KEY = 'userID'; // Key for local storage
  private USERNAME_KEY = 'username';

  constructor() {}

  // Store userID in local storage
  setUserID(userID: string): void {
    localStorage.setItem(this.USER_ID_KEY, userID);
  }

  // Retrieve userID from local storage
  getUserID(): string | null {
    return localStorage.getItem(this.USER_ID_KEY);
  }

  // Remove userID from local storage
  removeUserID(): void {
    localStorage.removeItem(this.USER_ID_KEY);
  }

  // Store username in local storage
  setUserName(username: string): void {
    localStorage.setItem(this.USERNAME_KEY, username);
  }

  // Retrieve userID from local storage
  getUserName(): string | null {
    return localStorage.getItem(this.USERNAME_KEY);
  }

  // Remove userID from local storage
  removeUserName(): void {
    localStorage.removeItem(this.USERNAME_KEY);
  }
}
