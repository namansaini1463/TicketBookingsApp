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
import { UserService } from '../../services/user/user.service';

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
    private router: Router,
    private userService: UserService
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

  // Handle form submission
  onSubmit(): void {
    if (this.profileForm.valid) {
      const { userName, oldPassword, newPassword } = this.profileForm.value;
      const userID = localStorage.getItem('userID'); // Assuming userID is stored in localStorage

      if (userID) {
        // Compare the current username (from the observable) with the new one
        const currentUsername = this.userService.getUserName(); // This should return the current username from the service

        if (currentUsername === userName && !newPassword && !oldPassword) {
          alert(
            'The new username must be different from the current username!'
          );
          return;
        }

        if (oldPassword && !newPassword) {
          alert('Please enter a new password');
          return;
        }

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
