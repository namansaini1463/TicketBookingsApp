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
  selectedFile: File | null = null; // Variable to hold the selected file
  profilePictureUrl!: string | undefined;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    // Initialize the form with the fields in the HTML form
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]], // First Name
      lastName: ['', [Validators.required, Validators.minLength(2)]], // Last Name
      username: ['', [Validators.required, Validators.minLength(4)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.pattern('^[0-9]{10,15}$')], // Optional phone number pattern
      profilePictureUrl: [''], // Optional profile picture
      preferredLanguage: [''], // Optional preferred language
      preferredCurrency: [''], // Optional preferred currency
      oldPassword: ['', Validators.minLength(8)], // Optional for password change
      newPassword: ['', Validators.minLength(8)], // Optional for password change
    });

    // Fetch user data and populate the form
    const user = this.userService.getUserProfile();
    if (user) {
      this.profilePictureUrl = user.profilePictureUrl;
      this.profileForm.patchValue({
        firstName: user.firstName,
        lastName: user.lastName,
        username: user.username,
        email: user.email,
        phoneNumber: user.phoneNumber,
        profilePictureUrl: user.profilePictureUrl,
        preferredLanguage: user.preferredLanguage,
        preferredCurrency: user.preferredCurrency,
      });
    } else {
      console.error('No user profile found in local storage');
    }
  }

  // Handle form submission
  onSubmit(): void {
    if (this.profileForm.valid) {
      const {
        firstName,
        lastName,
        username,
        email,
        phoneNumber,
        preferredLanguage,
        preferredCurrency,
        oldPassword,
        newPassword,
      } = this.profileForm.value;

      // Retrieve the user profile from local storage and parse it
      const storedUser = localStorage.getItem('userProfile');
      if (storedUser) {
        const user = JSON.parse(storedUser);

        // Extract the userID from the parsed user object
        const userID = user.userID;

        // If oldPassword or newPassword is provided, make sure both are filled in
        if (oldPassword || newPassword) {
          if (!oldPassword || !newPassword) {
            alert(
              'Please provide both old and new password to change the password.'
            );
            return;
          }
        }

        // Create the DTO object to send with the data and optional profile picture
        let updateDTO: updateRequestDTO = {
          userID: userID,
          firstName: firstName,
          lastName: lastName,
          username: username,
          email: email,
          phoneNumber: phoneNumber?.length === 0 ? null : phoneNumber,
          preferredLanguage: preferredLanguage,
          preferredCurrency: preferredCurrency,
          oldPassword: oldPassword || undefined, // Send undefined if not changing password
          newPassword: newPassword || undefined, // Send undefined if not changing password
          profilePicture: this.selectedFile, // File selected through input
        };

        console.log(updateDTO);

        // Call the service to update the user profile
        this.authService.updateUser(updateDTO).subscribe(
          () => {
            alert('Profile updated successfully!');
            this.router.navigate(['/login']); // Optionally, reload the profile page
          },
          (error) => {
            console.error('Profile update failed', error);
            alert(`Failed to update profile : ${error.error.errors[0]}`);
          }
        );
      } else {
        console.error('No user profile found in local storage');
      }
    } else {
      alert('Please fill in all required fields correctly.');
    }
  }

  // Handle file input change event
  onFileChange(event: any): void {
    this.selectedFile = event.target.files[0]; // Save the selected file to the variable
  }

  // Unsubscribe when the component is destroyed
  ngOnDestroy(): void {
    if (this.usernameSubscription) {
      this.usernameSubscription.unsubscribe();
    }
  }
}
