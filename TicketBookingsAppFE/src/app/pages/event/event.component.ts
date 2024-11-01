import { Component } from '@angular/core';
import { Event } from '../../models/Event';
import { ActivatedRoute } from '@angular/router';
import { EventsService } from '../../services/events/events.service';
import { CommonModule } from '@angular/common';
import { CarouselComponent } from '../../components/ui/carousel/carousel.component';
import { mapToEvent } from '../../models/Event';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [CommonModule, CarouselComponent, MatIconModule],
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css'],
})
export class EventComponent {
  eventID: string | null = null;
  event!: Event | null;

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService,
    private authService: AuthService,
    private router: Router
  ) {}

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

  ngOnInit(): void {
    this.eventID = this.route.snapshot.paramMap.get('eventId');
    console.log(this.eventID);

    if (this.eventID) {
      this.loadEventData(this.eventID);
    }
  }

  loadEventData(eventID: string): void {
    this.eventsService.getEventByID(eventID).subscribe(
      (data: any) => {
        console.log('API Response:', data);
        if (data) {
          this.event = mapToEvent(data);
        } else {
          this.event = null;
        }
      },
      (error) => {
        console.error('Error fetching event data:', error);
        this.event = null;
      }
    );
  }

  bookEvent(ticketID: string): void {
    console.log(this.event?.eventID);

    if (this.authService.isLoggedIn()) {
      // Navigate to the booking page with the event ID
      this.router.navigate([`/book/${this.event?.eventID}`, ticketID]);
    } else {
      // If the user is not logged in, redirect them to the login page
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: `/book/${this.event?.eventID}/${ticketID}` },
      });
    }
  }
}
