import { Event } from './Event';

export enum PaymentStatus {
  Pending = 'Pending',
  Paid = 'Paid',
  Failed = 'Failed',
  Refunded = 'Refunded',
}

export interface CreateBookingDTO {
  userID: string;
  couponCode: string | null; // Optional coupon code
  paymentMethod: string;
  paymentStatus: PaymentStatus;
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
