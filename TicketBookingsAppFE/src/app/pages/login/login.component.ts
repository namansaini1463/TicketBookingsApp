import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { loginRequestDTO } from '../../models/DTOs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  userLoginForm!: FormGroup; //^ Define the form group

  constructor(
    private fb: FormBuilder, // Injecting FormBuilder
    private authService: AuthService, // Injecting AuthService
    private router: Router // Injecting Router for redirection
  ) {}

  ngOnInit(): void {
    // Initialize the form using FormBuilder
    this.userLoginForm = this.fb.group({
      userNameOrEmail: ['', [Validators.required]], // Email field with validation
      password: ['', [Validators.required, Validators.minLength(4)]], // Password field with validation
    });
  }

  // Method to handle form submission
  onFormSubmit(): void {
    if (this.userLoginForm.valid) {
      let loginDTO: loginRequestDTO = {
        userNameOrEmail: this.userLoginForm.get('userNameOrEmail')?.value,
        password: this.userLoginForm.get('password')?.value,
      };

      // Call the login method in AuthService
      this.authService.login(loginDTO).subscribe(
        (response) => {
          alert('Login successful');
          // Redirect to dashboard or other protected route
          this.router.navigate(['/']);
        },
        (error) => {
          console.error('Login failed', error);
          alert('Invalid credentials'); // Show an error message
        }
      );
    } else {
      alert('Please enter valid credentials'); // Show a validation error if form is invalid
    }
  }
}
