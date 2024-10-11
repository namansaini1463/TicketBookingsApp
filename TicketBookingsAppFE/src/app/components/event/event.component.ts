import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { Event } from '../../models/Event';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [],
  templateUrl: './event.component.html',
  styleUrl: './event.component.css',
})
export class EventComponent {
  @Input() event: Event | null = null;

  constructor(private router: Router, private authService: AuthService) {}

  // Method to handle "Book Now" button click
  bookEvent(eventId: string): void {
    if (this.authService.isLoggedIn()) {
      // Navigate to the booking page with the event ID
      this.router.navigate([`/book`, eventId]);
    } else {
      // If the user is not logged in, redirect them to the login page
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: `/book/${eventId}` },
      });
    }
  }
}
