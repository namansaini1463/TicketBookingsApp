import { Component, NgModule } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventsService } from '../../services/events/events.service';
import { Event } from '../../models/Event';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService
  ) {}

  // Fetch event details when component initializes
  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('eventId')!;
    this.fetchEvent(eventId);
  }

  // Fetch event details from the backend
  fetchEvent(eventId: string): void {
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

  // Handle form submission
  onSubmit(): void {
    alert(`Booked ${this.numTickets} tickets for event: ${this.event.name}`);
    // Implement booking functionality
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
