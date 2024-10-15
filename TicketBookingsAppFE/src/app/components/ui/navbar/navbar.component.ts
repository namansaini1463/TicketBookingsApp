import { NgModule, OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router'; // Import Router and NavigationEnd
import { AuthService } from '../../../services/auth/auth.service';
import { filter } from 'rxjs/operators'; // Import filter for filtering events
import { Subscription } from 'rxjs'; // Import Subscription for cleanup

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  isLoggedIn: boolean = false; // Variable to track login state
  username: string | null = null; // Variable to store the username

  private usernameSubscription!: Subscription; // Subscription for username observable
  private loggedInSubscription!: Subscription; // Subscription for login observable

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    // Subscribe to login state changes from AuthService
    this.loggedInSubscription = this.authService.LoggedIn$.subscribe(
      (loggedIn) => {
        this.isLoggedIn = loggedIn; // Update the local state when login state changes
      }
    );

    // Subscribe to the username observable to get the current username
    this.usernameSubscription = this.authService.username$.subscribe(
      (username) => {
        this.username = username; // Update the username when it changes
      }
    );

    // Listen to router events to detect page navigation and update login state
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd)) // Filter for only NavigationEnd events
      .subscribe(() => {
        // Update isLoggedIn based on current login state after navigation
        this.isLoggedIn = this.authService.isLoggedIn(); // Check the login status
      });
  }

  // Handle logout
  logout(): void {
    if (confirm('Are you sure you want to log out ?')) {
      this.authService.logout(); // Call the logout method from AuthService
      this.router.navigate(['/login']); // Optionally navigate to login page after logging out
      alert('You were logged out!');
    }
  }
}
