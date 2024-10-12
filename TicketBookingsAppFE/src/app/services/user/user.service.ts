import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private USER_ID_KEY = 'userID'; // Key for local storage

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
}
