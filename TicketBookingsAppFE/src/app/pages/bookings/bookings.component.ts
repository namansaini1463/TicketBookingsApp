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
export class BookingsComponent {}
