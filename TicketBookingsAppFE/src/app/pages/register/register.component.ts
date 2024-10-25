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
import { registerRequestDTO } from '../../models/Auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  userRegisterForm!: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    // Initialize the form with all required fields
    this.userRegisterForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.pattern('^[0-9]{10,15}$')]], // Optional phone number
      password: ['', [Validators.required, Validators.minLength(8)]],
      profilePictureUrl: [''], // Optional
      preferredLanguage: [''], // Optional
      preferredCurrency: [''], // Optional
    });
  }

  // Redirect to events if user logged in
  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      // Redirect to events page if already logged in
      this.router.navigate(['/events']);
    }
  }

  onSubmit(): void {
    if (this.userRegisterForm.valid) {
      // Create register DTO object from form values
      let registerDTO: registerRequestDTO = {
        username: this.userRegisterForm.get('username')?.value,
        firstName: this.userRegisterForm.get('firstName')?.value,
        lastName: this.userRegisterForm.get('lastName')?.value,
        email: this.userRegisterForm.get('email')?.value,
        password: this.userRegisterForm.get('password')?.value,
        phoneNumber:
          this.userRegisterForm.get('phoneNumber')?.value || undefined, // Optional phone number
        preferredLanguage:
          this.userRegisterForm.get('preferredLanguage')?.value || undefined, // Optional
        preferredCurrency:
          this.userRegisterForm.get('preferredCurrency')?.value || undefined, // Optional
        profilePicture: this.selectedFile, // Profile picture as File object
        roles: [],
      };

      console.log(registerDTO);

      // Call AuthService to register the user
      this.authService.register(registerDTO).subscribe(
        (response) => {
          console.log(response);
          alert(response.message);
          this.router.navigate(['/login']); // Redirect to login on success
        },
        (error) => {
          console.error('Registration failed', error.error);

          alert(error.error);
        }
      );
    } else {
      alert('Please fill in all the required fields correctly.');
    }
  }

  onFileChange(event: any): void {
    this.selectedFile = event.target.files[0]; // Handle the file input and save it
  }
}
