import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventsService } from '../../services/events/events.service';
import { Event, mapToEvent, TicketType } from '../../models/Event';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BookingService } from '../../services/booking/booking.service';
import { AddBookingDTO } from '../../models/Booking';
import { UserService } from '../../services/user/user.service';
import { getFormattedTimeWithTimezone } from '../../utils/getFormattedTimeWithTimeZone';

@Component({
  selector: 'app-book-event',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './book-event.component.html',
  styleUrls: ['./book-event.component.css'], // Corrected from styleUrl to styleUrls
})
export class BookEventComponent {
  responseEvent: any; // Corrected variable name
  event!: Event;
  numTickets: number = 1;
  totalPrice: number = 0;
  bookingResponse: string | undefined;
  getFormattedTimeWithTimezone = getFormattedTimeWithTimezone;
  selectedTicketType: TicketType | null = null;
  ticketId: string | null = null; // Store ticketId here

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService,
    private bookingService: BookingService,
    private userService: UserService,
    private router: Router
  ) {}

  // Fetch event details when component initializes
  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('eventId')!;
    this.ticketId = this.route.snapshot.paramMap.get('ticketId');
    this.fetchEventFromDB(eventId);
  }

  // Fetch event details from the backend
  fetchEventFromDB(eventId: string): void {
    this.eventsService.getEventByID(eventId).subscribe(
      (data) => {
        this.responseEvent = data;
        this.event = this.responseEvent;
        this.event = mapToEvent(this.event);

        // Now that the event is loaded, fetch the selected ticket type
        if (this.ticketId) {
          this.selectedTicketType = this.fetchTicketTypeFromEvent(
            this.ticketId
          );
          if (this.selectedTicketType) {
            this.calculateTotalPrice();
          } else {
            alert('Invalid ticket type selected.');
          }
        }
      },
      (error) => {
        console.error('Error fetching event:', error);
      }
    );
  }

  // Get the ticketType from the event object
  fetchTicketTypeFromEvent(ticketId: string): TicketType | null {
    console.log(this.event.ticketTypes);

    if (!this.event || !this.event.ticketTypes) {
      return null;
    }
    const ticket = this.event.ticketTypes.find(
      (ticket) => ticket.ticketTypeID === ticketId
    );
    return ticket ?? null;
  }

  // Handle form submission
  onSubmit(): void {
    const userID = this.userService.getUserProfile()?.userID ?? null;
    const eventId = this.event.eventID;

    if (!this.selectedTicketType) {
      alert('Please select a ticket type.');
      return;
    }

    const bookingRequest: AddBookingDTO = {
      userID: userID,
      numberOfTickets: this.numTickets,
      ticketTypeID: this.selectedTicketType.ticketTypeID, // Included ticketTypeID
    };

    // Call the booking service to book the event
    this.bookingService.bookEvent(eventId, bookingRequest).subscribe(
      (response) => {
        // Handle success
        this.bookingResponse = `Successfully booked ${this.numTickets} tickets for ${this.event.name}!`;
        // Redirect to a success page or booking details page
        this.router.navigate(['/bookings']);
      },
      (error) => {
        // Handle error
        if (error.status === 400) {
          this.bookingResponse =
            'Booking failed: Not enough tickets or invalid data.';
        } else if (error.status === 404) {
          this.bookingResponse = 'Event not found!';
        } else {
          this.bookingResponse = 'An error occurred. Please try again later.';
        }
        console.error('Error booking the event:', error);
      }
    );
    alert(`Booked ${this.numTickets} tickets for event: ${this.event.name}`);
  }

  // Calculate total price based on the number of tickets and event ticket price
  calculateTotalPrice(): void {
    if (this.selectedTicketType) {
      this.totalPrice = this.numTickets * this.selectedTicketType.price;
    } else {
      this.totalPrice = 0;
    }
  }

  // Method to decrease the number of tickets
  decreaseTickets(): void {
    if (this.numTickets > 1) {
      this.numTickets--;
      this.calculateTotalPrice(); // Recalculate price
    }
  }

  // Method to increase the number of tickets
  increaseTickets(): void {
    if (this.numTickets < (this.selectedTicketType?.quantityAvailable ?? 8)) {
      this.numTickets++;
      this.calculateTotalPrice(); // Recalculate price
    }
    console.log(this.selectedTicketType);
  }
}
