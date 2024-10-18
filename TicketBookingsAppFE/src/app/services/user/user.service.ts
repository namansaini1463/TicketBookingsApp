import { Injectable } from '@angular/core';
import { UserProfile } from '../../models/Auth';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private USER_KEY = 'userProfile'; // Key for storing user profile in local storage
  private USERNAME_KEY = 'username'; // Key for storing username in local storage

  constructor() {}

  // Store the complete user profile in local storage
  setUserProfile(user: UserProfile): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(user)); // Save user profile as a JSON string
  }

  // Retrieve the user profile from local storage
  getUserProfile(): UserProfile | null {
    const storedUser = localStorage.getItem(this.USER_KEY);
    if (storedUser) {
      return JSON.parse(storedUser) as UserProfile; // Parse the JSON string back to a UserProfile object
    }
    return null; // Return null if the user profile is not found
  }

  // Remove the user profile from local storage
  removeUserProfile(): void {
    localStorage.removeItem(this.USER_KEY); // Clear the stored user profile from local storage
  }

  // Store the username separately in local storage (optional if needed)
  setUserName(username: string): void {
    localStorage.setItem(this.USERNAME_KEY, username); // Save username as a string
  }

  // Retrieve the username from local storage
  getUserName(): string | null {
    return localStorage.getItem(this.USERNAME_KEY); // Return the username or null if not found
  }

  // Remove the stored username from local storage
  removeUserName(): void {
    localStorage.removeItem(this.USERNAME_KEY); // Clear the stored username from local storage
  }
}
