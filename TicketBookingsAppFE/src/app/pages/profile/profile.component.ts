import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { updateRequestDTO } from '../../models/Auth';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule], // Import ReactiveFormsModule here
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup; // Form group for the profile form
  usernameSubscription!: Subscription; // Subscription to handle the username observable

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Initialize the form
    this.profileForm = this.fb.group({
      userName: ['', [Validators.required]],
      oldPassword: ['', Validators.minLength(4)], // Optional for password change
      newPassword: ['', Validators.minLength(4)], // Optional for password change
    });

    // Subscribe to the username observable
    this.usernameSubscription = this.authService.username$.subscribe(
      (username) => {
        if (username) {
          this.profileForm.get('userName')?.setValue(username); // Dynamically update the username form control
        }
      }
    );
  }

  // Unsubscribe from the username observable when the component is destroyed
  ngOnDestroy(): void {
    if (this.usernameSubscription) {
      this.usernameSubscription.unsubscribe();
    }
  }

  // Handle form submission
  onSubmit(): void {
    if (this.profileForm.valid) {
      const { userName, oldPassword, newPassword } = this.profileForm.value;
      const userID = localStorage.getItem('userID'); // Assuming userID is stored in localStorage

      if (userID) {
        var updateDTO: updateRequestDTO = {
          userID: userID,
          username: userName,
          oldPassword: oldPassword,
          newPassword: newPassword,
        };
        this.authService.updateUser(updateDTO).subscribe(
          () => {
            this.router.navigate(['/login']); // Navigate to login after logout
          },
          (error) => {
            console.error('Profile update failed', error);
            alert('Failed to update profile');
          }
        );
      }
    } else {
      alert('Please enter valid details');
    }
  }
}
