import { TicketType } from './Event';
import { Event } from './Event';

export interface CartItem {
  userID: string;
  ticketTypeID: string;
  quantity: number;
}

export interface CartItemDTO {
  cartID: string;
  cartItemID: string;
  ticketType: TicketType;
  event: Event;
  quantity: number;
  dateAdded: string;
}

export interface UpdateCartItem {
  cartItemID: string;
  quantity: number;
}
