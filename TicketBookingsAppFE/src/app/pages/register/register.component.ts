import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { registerRequestDTO } from '../../models/DTOs';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  userRegisterForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.userRegisterForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10,15}$')]],
      password: ['', [Validators.required, Validators.minLength(4)]],
    });
  }

  // Redirect to events if user logged in
  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      // Redirect to events page if already logged in
      this.router.navigate(['/events']);
    }
  }
  // Handle form submission
  onSubmit(): void {
    if (this.userRegisterForm.valid) {
      let registerDTO: registerRequestDTO = {
        name: this.userRegisterForm.get('username')?.value,
        username: this.userRegisterForm.get('email')?.value,
        password: this.userRegisterForm.get('password')?.value,
        phoneNumber: this.userRegisterForm.get('phone')?.value,
        roles: [],
      };

      // Call AuthService to register
      this.authService.register(registerDTO).subscribe(
        (response) => {
          console.log(response);
          alert(response.message);
          this.router.navigate(['/login']); // Redirect to login on success
        },
        (error) => {
          console.error('Registration failed', error.error);
          alert(error.error.message);
        }
      );
    } else {
      alert('Please fill in all the required fields correctly.');
    }
  }
}
