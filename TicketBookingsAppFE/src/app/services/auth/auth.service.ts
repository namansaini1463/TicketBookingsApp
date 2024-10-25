import { Injectable } from '@angular/core';
import { TokenService } from '../token/token.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, throwError } from 'rxjs';
import {
  loginRequestDTO,
  registerRequestDTO,
  updateRequestDTO,
} from '../../models/Auth';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseRequestUrlHttps = 'https://localhost:7290/api/Auth';
  private readonly baseRequestUrlHttp = 'http://localhost:5027/api/auth';

  // BehaviorSubject to track login state
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  LoggedIn$ = this.isLoggedInSubject.asObservable();

  // BehaviorSubject to track the current username
  private usernameSubject = new BehaviorSubject<string | null>(null);
  username$ = this.usernameSubject.asObservable();

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private userService: UserService
  ) {
    // Initialize the logged-in state and username
    this.isLoggedInSubject.next(this.checkInitialLoginState());
    this.setInitialUsername(); // Set initial username from the token if available
  }

  // Check the initial login state
  private checkInitialLoginState(): boolean {
    try {
      return !!this.tokenService.getToken(); // Return true if token exists
    } catch (error) {
      console.error('Error checking initial login state:', error);
      return false;
    }
  }

  // Set initial username from the stored token (if available)
  private setInitialUsername(): void {
    const token = this.tokenService.getToken();
    if (token) {
      // Assuming userService.getUserName() retrieves the stored username from localStorage or sessionStorage
      const storedUsername = this.userService.getUserName();
      if (storedUsername) {
        this.usernameSubject.next(storedUsername); // Emit the stored username
      }
    }
  }

  //` Login Function
  login(loginPayload: loginRequestDTO): Observable<any> {
    return this.http
      .post<any>(`${this.baseRequestUrlHttp}/login`, loginPayload)
      .pipe(
        tap((response) => {
          // Store the JWT token using the TokenService
          this.tokenService.setToken(response.jwtToken);

          // Set userID and username using UserService
          this.userService.setUserProfile(response.userProfile);
          this.userService.setUserName(response.username); // Assuming response contains 'username'

          // Emit the username through BehaviorSubject
          this.usernameSubject.next(response.username);

          // Emit the login state as true
          this.isLoggedInSubject.next(true);
        })
      );
  }

  register(registerPayload: registerRequestDTO): Observable<any> {
    // Create FormData object to send as 'multipart/form-data'
    const formData = new FormData();

    // Append the registration data to the FormData object
    formData.append('Username', registerPayload.username);
    formData.append('Email', registerPayload.email);
    formData.append('FirstName', registerPayload.firstName);
    formData.append('LastName', registerPayload.lastName);
    formData.append('PhoneNumber', registerPayload.phoneNumber || ''); // Optional phone number
    formData.append(
      'PreferredLanguage',
      registerPayload.preferredLanguage || 'English'
    );
    formData.append(
      'PreferredCurrency',
      registerPayload.preferredCurrency || 'INR'
    );
    formData.append('Password', registerPayload.password);

    // Append the profile picture if it exists
    if (registerPayload.profilePicture) {
      formData.append('ProfilePicture', registerPayload.profilePicture || null);
    }

    // Send the form data
    return this.http.post<any>(
      `${this.baseRequestUrlHttp}/register`,
      formData // Send as FormData
    );
  }

  //` Logout method
  logout(): void {
    this.tokenService.removeToken(); // Remove token on logout
    this.userService.removeUserProfile(); // Clear user ID
    this.userService.removeUserName();
    // this.userService.removeUserName(); // Clear username

    // Clear the username and emit the login state as false
    this.usernameSubject.next(null); // Emit null to clear the username
    this.isLoggedInSubject.next(false); // Emit the logged out state
  }

  //` Check if the user is logged in
  isLoggedIn(): boolean {
    return this.tokenService.getToken() ? true : false;
  }

  // Update user function to handle FormData
  updateUser(updatePayload: updateRequestDTO): Observable<any> {
    // Create FormData object to send as 'multipart/form-data'
    const formData = new FormData();

    // Append the update data to the FormData object
    formData.append('userID', updatePayload.userID);
    formData.append('Username', updatePayload.username || '');
    formData.append('Email', updatePayload.email || '');
    formData.append('FirstName', updatePayload.firstName || '');
    formData.append('LastName', updatePayload.lastName || '');
    formData.append('PhoneNumber', updatePayload.phoneNumber || ''); // Optional phone number
    formData.append(
      'PreferredLanguage',
      updatePayload.preferredLanguage || 'English'
    );
    formData.append(
      'PreferredCurrency',
      updatePayload.preferredCurrency || 'INR'
    );

    // Append the password fields only if they exist
    if (updatePayload.oldPassword) {
      formData.append('OldPassword', updatePayload.oldPassword);
    }
    if (updatePayload.newPassword) {
      formData.append('NewPassword', updatePayload.newPassword);
    }

    // Append the profile picture if it exists
    if (updatePayload.profilePicture) {
      formData.append('ProfilePicture', updatePayload.profilePicture);
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.tokenService.getToken()}`,
    });

    // Send the form data
    return this.http
      .put<any>(`${this.baseRequestUrlHttp}/update`, formData, { headers })
      .pipe(
        tap((response) => {
          this.userService.setUserProfile(response.userProfile);

          // Update username if it was changed
          if (response.username) {
            this.usernameSubject.next(response.username);
          }

          alert('Profile updated successfully. You will be logged out.');

          // Log out the user after a successful update
          this.logout();
        })
      );
  }
}
