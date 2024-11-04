import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateBookingDTO } from '../../models/Booking';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private readonly baseRequestUrlHttps = 'https://localhost:7290/api/Bookings';
  private readonly baseRequestUrlHttp = 'http://localhost:5027/api/Bookings';

  constructor(private http: HttpClient) {}

  // Create a booking from the cart
  createBooking(createBookingDTO: CreateBookingDTO): Observable<any> {
    return this.http.post(
      `${this.baseRequestUrlHttp}/Create`,
      createBookingDTO
    );
  }

  // Cancel an existing booking
  cancelBooking(bookingId: string): Observable<any> {
    return this.http.put(
      `${this.baseRequestUrlHttp}/Cancel/${bookingId}`,
      null
    );
  }

  // Get a specific booking by its ID
  getBookingById(bookingId: string): Observable<any> {
    return this.http.get(`${this.baseRequestUrlHttp}/${bookingId}`);
  }

  // Get all bookings for a specific user
  getUserBookings(userId: string): Observable<any> {
    return this.http.get(`${this.baseRequestUrlHttp}/User/${userId}`);
  }
}
