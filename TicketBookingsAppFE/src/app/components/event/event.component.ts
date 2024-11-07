import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { BookingEvent } from '../../models/Event';
import { CommonModule } from '@angular/common';
import { CarouselComponent } from '../ui/carousel/carousel.component';
import { getFormattedTimeWithTimezone } from '../../utils/getFormattedTimeWithTimeZone';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [CommonModule, CarouselComponent, MatIconModule],
  templateUrl: './event.component.html',
  styleUrl: './event.component.css',
})
export class EventComponent {
  @Input() event: BookingEvent | null = null;
  getFormattedTimeWithTimezone = getFormattedTimeWithTimezone;
  currentSlide = 0;

  constructor(private router: Router, private authService: AuthService) {}

  getTotalTicketsAvailable(): number {
    var totalTicketsAvailable = 0;

    this.event?.ticketTypes.forEach(
      (ticketType) => (totalTicketsAvailable += ticketType.quantityAvailable)
    );

    return totalTicketsAvailable;
  }

  goToEvent(eventId: string) {
    this.router.navigate([`/event`, eventId]);
  }

  // Method to handle "Book Now" button click
  bookEvent(eventId: string): void {
    if (this.authService.isLoggedIn()) {
      // Navigate to the booking page with the event ID
      this.router.navigate([`/event`, eventId]);
    } else {
      // If the user is not logged in, redirect them to the login page
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: `/event/${eventId}` },
      });
    }
  }
}
