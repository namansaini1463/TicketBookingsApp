import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/booking/booking.service';
import { BookingDTO } from '../../models/Booking';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user/user.service';
import { AuthService } from '../../services/auth/auth.service';

import { BookingComponent } from '../../components/booking/booking.component';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule, BookingComponent, MatIconModule, RouterModule],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css',
})
export class BookingsComponent implements OnInit {
  userID: string | null = null;
  bookings: any[] = [];
  activeBookings: any[] = [];
  showActiveBookings: boolean = true;
  bookingItems: any[] = [];
  errorMessage: string = '';

  constructor(
    private userService: UserService,
    private bookingService: BookingService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userID = this.userService.getUserProfile()?.userID ?? null;
    if (this.userID) {
      this.loadUserBookings();
    } else {
      this.router.navigate(['/login']); // Redirect if not logged in
    }
  }

  // Load all bookings for the user
  loadUserBookings(): void {
    if (!this.userID) return; // Ensure userID is valid before loading
    this.bookingService.getUserBookings(this.userID).subscribe(
      (data) => {
        this.bookings = data.$values;
        this.filterActiveBookings();
        console.log(this.activeBookings);

        // Resolve references in booking items
        this.resolveReferences(this.bookings);

        // Assign resolved booking items
        this.bookingItems = this.bookings;
        console.log(this.bookingItems);
      },
      (error) => {
        console.error(error);
        this.errorMessage = 'Failed to load bookings.';
      }
    );
  }

  // Remove a deleted booking from the list
  handleBookingCancelled(bookingID: string): void {
    // const eventDeleteBookingID = bookingID as unknown as string;
    console.log('Received bookingID in handleBookingCancelled:', bookingID); // Log the received bookingID
    this.bookings = this.bookings.filter(
      (booking) => booking.bookingID !== bookingID
    );
    this.filterActiveBookings(); // Update active bookings as well
    this.loadUserBookings();
  }

  private filterActiveBookings(): void {
    this.activeBookings = this.bookings.filter(
      (booking) => booking.bookingStatus === 'Confirmed'
    );
  }

  /**
   * Resolves $ref references in the booking items for ticketType and event.
   * @param bookings - The array of booking objects to resolve references for.
   */
  private resolveReferences(bookings: any[]): void {
    const refMap: { [key: string]: any } = {};

    // Step 1: Collect all objects with $id in the bookings data
    bookings.forEach((booking) => this.collectIds(booking, refMap));

    // Step 2: Resolve references for ticketType and event in each booking item
    bookings.forEach((booking) => this.resolveDirectRefs(booking, refMap));
  }

  /**
   * Collect all objects with $id and store them in refMap.
   * @param obj - The object to collect $id fields from.
   * @param refMap - The map storing objects by their $id.
   */
  private collectIds(obj: any, refMap: { [key: string]: any }): void {
    const queue = [obj];
    while (queue.length > 0) {
      const current = queue.shift();
      if (current && typeof current === 'object') {
        if (current.$id) refMap[current.$id] = current;
        Object.values(current).forEach((value) => {
          if (value && typeof value === 'object') queue.push(value);
        });
      }
    }
  }

  /**
   * Resolve top-level references for ticketType and nested event in booking items.
   * @param booking - The booking object to resolve references for.
   * @param refMap - The map of all objects by their $id for quick lookup.
   */
  private resolveDirectRefs(
    booking: any,
    refMap: { [key: string]: any }
  ): void {
    if (booking.bookingItems && booking.bookingItems.$values) {
      booking.bookingItems.$values.forEach((item: any) => {
        // Resolve ticketType if it has a $ref
        if (item.ticketType && item.ticketType.$ref) {
          item.ticketType = refMap[item.ticketType.$ref] || item.ticketType;
        }

        // Resolve nested event in ticketType if it has a $ref
        if (
          item.ticketType &&
          item.ticketType.event &&
          item.ticketType.event.$ref
        ) {
          item.ticketType.event =
            refMap[item.ticketType.event.$ref] || item.ticketType.event;
        }
      });
    }
  }
}
