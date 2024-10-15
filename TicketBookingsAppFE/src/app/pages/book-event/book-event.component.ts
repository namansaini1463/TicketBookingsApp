import { Component, NgModule } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventsService } from '../../services/events/events.service';
import { Event } from '../../models/Event';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BookingService } from '../../services/booking/booking.service';
import { AddBookingDTO } from '../../models/Booking'; // Import your AddBookingDTO
import { Router } from '@angular/router'; // Import Router to redirect after booking
import { UserService } from '../../services/user/user.service';

@Component({
  selector: 'app-book-event',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './book-event.component.html',
  styleUrl: './book-event.component.css',
})
export class BookEventComponent {
  reponseEvent: any;
  event!: Event;
  numTickets: number = 1;
  totalPrice: number = 0;
  bookingResponse: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService,
    private bookingService: BookingService,
    private userService: UserService,
    private router: Router // To redirect after booking
  ) {}

  // Fetch event details from the backend
  fetchEventFromDB(eventId: string): void {
    this.eventsService.getEventByID(eventId).subscribe(
      (data) => {
        this.reponseEvent = data;
        this.event = this.reponseEvent;
        this.calculateTotalPrice(); // Calculate the price when the event is loaded
      },
      (error) => {
        console.error('Error fetching event:', error);
      }
    );
  }

  // Fetch event details when component initializes
  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('eventId')!;
    this.fetchEventFromDB(eventId);
  }

  // Handle form submission
  onSubmit(): void {
    const userID = this.userService.getUserID();
    const eventId = this.event.eventID;

    const bookingRequest: AddBookingDTO = {
      userID: userID, // Logged in user ID
      numberOfTickets: this.numTickets, // Number of tickets from the form
    };

    // Call the booking service to book the event
    this.bookingService.bookEvent(eventId, bookingRequest).subscribe(
      (response) => {
        // Handle success
        this.bookingResponse = `Successfully booked ${this.numTickets} tickets for ${this.event.name}!`;
        // Optionally, redirect to a success page or booking details page
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
    if (this.event) {
      this.totalPrice = this.numTickets * this.event.ticketPrice;
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
    if (this.numTickets < 8) {
      this.numTickets++;
      this.calculateTotalPrice(); // Recalculate price
    }
  }
}
