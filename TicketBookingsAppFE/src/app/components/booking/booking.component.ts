import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BookingEvent, TicketType } from '../../models/Event';
import { MatIconModule } from '@angular/material/icon';
import { BookingService } from '../../services/booking/booking.service';

interface BookingItem {
  bookingID: string;
  bookingStatus: string;
  event: BookingEvent;
  eventImage: string;
  ticketType: TicketType;
  ticketQuantity: number;
  bookingValue: number;
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css',
})
export class BookingComponent implements OnInit {
  @Input() booking: any;
  @Output() bookingCancelled = new EventEmitter<string>();

  constructor(private bookingService: BookingService) {}

  bookingItem: BookingItem = {
    bookingID: '',
    bookingStatus: '',
    event: {} as BookingEvent,
    eventImage: '',
    ticketType: {} as TicketType,
    ticketQuantity: 0,
    bookingValue: 0,
  };

  ngOnInit(): void {
    this.bookingItem.bookingID = this.booking.bookingID;
    this.bookingItem.bookingStatus = this.booking.bookingStatus;
    this.bookingItem.event =
      this.booking.bookingItems.$values[0].ticketType.event;
    this.bookingItem.eventImage =
      this.booking.bookingItems.$values[0].ticketType.event.images.$values[0].imageUrl;
    this.bookingItem.ticketType =
      this.booking.bookingItems.$values[0].ticketType;
    this.bookingItem.ticketQuantity =
      this.booking.bookingItems.$values[0].quantity;
    this.bookingItem.bookingValue = this.booking.totalAmount;
  }

  deleteBooking(): void {
    if (confirm('Are you sure you want to delete this booking ?')) {
      console.log('Emitting bookingID:', typeof this.bookingItem.bookingID);

      this.bookingService.cancelBooking(this.bookingItem.bookingID).subscribe(
        () => {
          // Emit an event to notify the parent component about the deletion
          this.bookingCancelled.emit(this.bookingItem.bookingID);
        },
        (error) => {
          console.error('Failed to delete booking', error);
          alert('Failed to delete booking. Please try again later.');
        }
      );
    }
  }
}
