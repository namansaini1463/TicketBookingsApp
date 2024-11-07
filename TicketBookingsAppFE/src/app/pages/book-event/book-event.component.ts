import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventsService } from '../../services/events/events.service';
import { BookingEvent, mapToEvent, TicketType } from '../../models/Event';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user/user.service';
import { CartItem } from '../../models/Cart';
import { CartService } from '../../services/cart/cart.service';
import { getFormattedTimeWithTimezone } from '../../utils/getFormattedTimeWithTimeZone';

@Component({
  selector: 'app-book-event',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './book-event.component.html',
  styleUrls: ['./book-event.component.css'],
})
export class BookEventComponent implements OnInit {
  event!: BookingEvent;
  numTickets: number = 1;
  totalPrice: number = 0;
  selectedTicketType: TicketType | null = null;
  getFormattedTimeWithTimezone = getFormattedTimeWithTimezone;

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService,
    private userService: UserService,
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('eventId')!;
    const ticketId = this.route.snapshot.paramMap.get('ticketId');
    this.fetchEvent(eventId, ticketId);
  }

  // Fetch event details and initialize the selected ticket type if ticketId is provided
  fetchEvent(eventId: string, ticketId: string | null): void {
    this.eventsService.getEventByID(eventId).subscribe(
      (data) => {
        this.event = mapToEvent(data);
        if (ticketId) {
          this.selectedTicketType =
            this.event.ticketTypes.find(
              (ticket) => ticket.ticketTypeID === ticketId
            ) || null;
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

  // Add item to cart
  addToCart(): void {
    const userID = this.userService.getUserProfile()?.userID;
    if (!userID || !this.selectedTicketType) {
      alert('Please select a valid ticket type and ensure you are logged in.');
      return;
    }

    const cartItem: CartItem = {
      userID: userID,
      quantity: this.numTickets,
      ticketTypeID: this.selectedTicketType.ticketTypeID,
    };

    this.cartService.addToCart(cartItem).subscribe(
      () => {
        alert(
          `Added ${this.numTickets} tickets to cart for ${this.event.name}!`
        );
        this.router.navigate(['/cart']);
      },
      (error) => {
        let message = 'An error occurred. Please try again later.';
        if (error.status === 400)
          message =
            'Failed to add to cart: Not enough tickets or invalid data.';
        else if (error.status === 404) message = 'Event or ticket not found!';
        alert(message);
      }
    );
  }

  // Calculate total price based on selected ticket type and quantity
  calculateTotalPrice(): void {
    this.totalPrice = this.numTickets * (this.selectedTicketType?.price ?? 0);
  }

  // Adjust ticket quantity and recalculate price
  decreaseTickets(): void {
    if (this.numTickets > 1) {
      this.numTickets--;
      this.calculateTotalPrice();
    }
  }

  increaseTickets(): void {
    if (this.numTickets < (this.selectedTicketType?.quantityAvailable ?? 8)) {
      this.numTickets++;
      this.calculateTotalPrice();
    }
  }
}
