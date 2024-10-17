import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { Event } from '../../models/Event';
import { CommonModule } from '@angular/common';
import { CarouselComponent } from '../ui/carousel/carousel.component';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [CommonModule, CarouselComponent],
  templateUrl: './event.component.html',
  styleUrl: './event.component.css',
})
export class EventComponent {
  @Input() event: Event | null = null;
  currentSlide = 0;

  constructor(private router: Router, private authService: AuthService) {}

  getFormattedTimeWithTimezone(dateString: string): string {
    const date = new Date(dateString);

    // Get the full time string with long timezone name (e.g., "India Standard Time")
    const timeWithLongTimezone = date.toLocaleTimeString('en-US', {
      timeZoneName: 'long',
    });

    // Get the time string with short timezone name (e.g., "GMT+5:30")
    const timeWithShortTimezone = date.toLocaleTimeString('en-US', {
      timeZoneName: 'short',
    });

    // Extract just the short timezone (e.g., "GMT+5:30") from the second string
    const shortTimezone = timeWithShortTimezone.slice(
      timeWithShortTimezone.indexOf('GMT')
    );

    return `${timeWithLongTimezone} ${shortTimezone}`;
  }

  //` Method for showing the previous slide in the carousel
  prevSlide(): void {
    if (this.event && this.event.images.length > 1) {
      this.currentSlide =
        this.currentSlide > 0
          ? this.currentSlide - 1
          : this.event.images.length - 1;
    }
  }

  //` Method for showing the next slide in the carousel
  nextSlide(): void {
    if (this.event && this.event.images.length > 1) {
      this.currentSlide =
        this.currentSlide < this.event.images.length - 1
          ? this.currentSlide + 1
          : 0;
    }
  }

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
