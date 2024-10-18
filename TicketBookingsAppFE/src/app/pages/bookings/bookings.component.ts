import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/booking/booking.service';
import { UserService } from '../../services/user/user.service';
import { BookingDTO } from '../../models/Booking';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css',
})
export class BookingsComponent implements OnInit {
  userBookings: BookingDTO[] = [];
  userID: string | null = null;

  constructor(
    private bookingService: BookingService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userID = this.userService.getUserProfile()?.userID ?? null; // Fetch the userID from the local storage

    if (this.userID) {
      // Fetching the user bookings if the userID is available
      this.bookingService.getUserBookings(this.userID).subscribe(
        (response) => {
          this.userBookings = response; // Store the bookings in the component's state
        },
        (error) => {
          if (this.userBookings.length != 0) {
            console.error('Error fetching user bookings:', error.error);
          }
        }
      );
    } else {
      alert('User not found. Please log in again.');
    }
  }

  onClick(): void {
    this.router.navigate(['/events']);
  }

  onDeleteBooking(bookingID: string): void {
    if (confirm('Are you sure you want to delete this booking?')) {
      this.bookingService.deleteBooking(bookingID).subscribe(
        (response) => {
          // After successful deletion, remove the booking from the UI
          this.userBookings = this.userBookings.filter(
            (booking) => booking.bookingID !== bookingID
          );
          console.log(`Booking with ID ${bookingID} deleted.`);
        },
        (error) => {
          console.error('Error deleting booking:', error);
        }
      );
    }
  }
}
