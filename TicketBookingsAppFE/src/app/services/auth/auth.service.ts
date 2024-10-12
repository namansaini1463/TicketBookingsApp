import { Injectable } from '@angular/core';
import { TokenService } from '../token/token.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, throwError } from 'rxjs';
import { loginRequestDTO, registerRequestDTO } from '../../models/Auth';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseRequestUrlHttps = 'https://localhost:7290/api/Auth';
  private readonly baseRequestUrlHttp = 'http://localhost:5027/api/auth';

  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  LoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private userService: UserService
  ) {
    // Initialize the logged-in state
    this.isLoggedInSubject.next(this.checkInitialLoginState());
  }
  private checkInitialLoginState(): boolean {
    try {
      return !!this.tokenService.getToken();
    } catch (error) {
      console.error('Error checking initial login state:', error);
      return false;
    }
  }

  // constructor(private http: HttpClient, private tokenService: TokenService) {}

  //` Login Function
  login(loginPayload: loginRequestDTO): Observable<any> {
    return this.http
      .post<any>(`${this.baseRequestUrlHttp}/login`, loginPayload)
      .pipe(
        tap((response) => {
          // Store the JWT token using the TokenService
          this.tokenService.setToken(response.jwtToken);
          this.userService.setUserID(response.userID);
        })
      );
  }

  //` Register Function
  register(registerPayload: registerRequestDTO): Observable<any> {
    return this.http.post<any>(
      `${this.baseRequestUrlHttp}/register`,
      registerPayload
    );
  }

  //` Logout method
  logout(): void {
    this.tokenService.removeToken(); // Remove token on logout
    this.userService.removeUserID();
  }

  //` Check if the user is logged in
  isLoggedIn(): boolean {
    return this.tokenService.getToken() ? true : false;
  }
}
