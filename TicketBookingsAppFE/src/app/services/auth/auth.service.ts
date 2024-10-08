import { Injectable } from '@angular/core';
import { TokenService } from '../token/token.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { loginRequestDTO, registerRequestDTO } from '../../models/DTOs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseRequestUrlHttps = 'https://localhost:7290/api/Auth';
  private baseRequestUrlHttp = 'http://localhost:5027/api/auth';

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  //` Login Function
  login(loginPayload: loginRequestDTO): Observable<any> {
    return this.http
      .post<any>(`${this.baseRequestUrlHttp}/login`, loginPayload)
      .pipe(
        tap((response) => {
          // Store the JWT token using the TokenService
          this.tokenService.setToken(response.jwtToken);
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
  }

  //` Check if the user is logged in
  isLoggedIn(): boolean {
    return this.tokenService.getToken() ? true : false;
  }
}
