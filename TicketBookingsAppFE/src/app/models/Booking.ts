import { Event } from './Event';

export interface AddBookingDTO {
  userID: string | null;
  ticketTypeID: string | null;
  numberOfTickets: number;
}

export interface BookingDTO {
  bookingID: string;
  eventID: string;
  userID: string | null;
  bookingDate: string;
  numberOfTickets: number;
  amount: number;

  bookingEvent: Event;
}
