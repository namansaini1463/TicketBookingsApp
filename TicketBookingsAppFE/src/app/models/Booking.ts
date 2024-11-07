import { CartItem, CartItemDTO } from './Cart';
import { BookingEvent } from './Event';

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
  bookingDate: string;
  bookingStatus: string;
  totalAmount: number;
  bookingItems: CartItemDTO[];
  bookingEvent: BookingEvent;
}
