import { TicketType } from './Event';
import { BookingEvent } from './Event';

export interface CartItem {
  userID: string;
  ticketTypeID: string;
  quantity: number;
}

export interface CartItemDTO {
  cartID: string;
  cartItemID: string;
  ticketType: TicketType;
  event: BookingEvent;
  quantity: number;
  dateAdded: string;
}

export interface UpdateCartItem {
  cartItemID: string;
  quantity: number;
}
