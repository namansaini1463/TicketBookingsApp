import { Injectable } from '@angular/core';

const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor() {}

  //`Saving the token in the local storage
  setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
  }

  //` Retrieving the token
  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  //` Removing the token
  removeToken(): void {
    localStorage.removeItem(TOKEN_KEY);
  }
}
