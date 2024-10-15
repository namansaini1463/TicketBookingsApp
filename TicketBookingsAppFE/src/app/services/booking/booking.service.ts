import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddBookingDTO, BookingDTO } from '../../models/Booking';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private readonly baseRequestUrlHttps = 'https://localhost:7290/api/Bookings';
  private readonly baseRequestUrlHttp = 'http://localhost:5027/api/Bookings';

  constructor(private http: HttpClient) {}

  // Book an event by ID
  bookEvent(
    eventId: string,
    addBookingDTO: AddBookingDTO
  ): Observable<BookingDTO> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<BookingDTO>(
      `${this.baseRequestUrlHttp}/book/${eventId}`,
      addBookingDTO,
      { headers }
    );
  }

  // Get all bookings
  getAllBookings(): Observable<BookingDTO[]> {
    return this.http.get<BookingDTO[]>(`${this.baseRequestUrlHttp}/all`);
  }

  // Get all bookings by a user
  getUserBookings(userId: string): Observable<BookingDTO[]> {
    return this.http.get<BookingDTO[]>(`${this.baseRequestUrlHttp}/${userId}`);
  }

  // Delete a booking by booking ID
  deleteBooking(bookingId: string): Observable<BookingDTO> {
    return this.http.delete<BookingDTO>(
      `${this.baseRequestUrlHttp}/delete/${bookingId}`
    );
  }
}
