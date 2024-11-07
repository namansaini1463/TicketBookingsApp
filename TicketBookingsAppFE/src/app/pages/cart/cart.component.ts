import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart/cart.service';
import { CartItem, CartItemDTO } from '../../models/Cart';
import { UserService } from '../../services/user/user.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { mapToEvent } from '../../models/Event';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  userID: string | null = null;
  totalAmount: number = 0;
  cart: CartItemDTO[] = [];

  constructor(
    private cartService: CartService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userID = this.userService.getUserProfile()?.userID ?? null;
    if (this.userID) {
      this.loadCart();
    } else {
      alert('User not logged in.');
    }
  }

  // Load cart items for the user
  loadCart(): void {
    this.cartService.getCart(this.userID!).subscribe(
      (response) => {
        console.log(response);
        const resolvedResponse = this.resolveRefsOnce(response);

        // Process the resolved response to extract cart items
        this.cart = resolvedResponse.cartItems.$values.map((item: any) => ({
          cartID: item.cartID,
          cartItemID: item.cartItemID,
          quantity: item.quantity,
          dateAdded: item.dateAdded,
          ticketType: item.ticketType,
          event: mapToEvent(item.ticketType.event),
        }));

        console.log(this.cart);
        this.calculateTotalAmount();
      },
      (error) => {
        console.error('Error loading cart:', error);
        alert('Failed to load cart items. Please try again.');
      }
    );
  }

  private resolveRefsOnce(data: any): any {
    const refMap: { [key: string]: any } = {};

    // First, map all objects with $id in the initial data
    function mapIds(obj: any) {
      const queue = [obj];
      while (queue.length > 0) {
        const current = queue.shift();
        if (current && typeof current === 'object') {
          if (current.$id) refMap[current.$id] = current;
          for (const key in current) {
            if (current.hasOwnProperty(key)) queue.push(current[key]);
          }
        }
      }
    }

    // Directly replace only top-level references in cartItems without recursion
    function resolveDirectRefs(obj: any): any {
      if (obj && obj.cartItems && obj.cartItems.$values) {
        obj.cartItems.$values = obj.cartItems.$values.map((item: any) => {
          // Resolve ticketType and event references only if they have $ref
          if (item.ticketType && item.ticketType.$ref) {
            item.ticketType = refMap[item.ticketType.$ref] || item.ticketType;
          }
          if (
            item.ticketType &&
            item.ticketType.event &&
            item.ticketType.event.$ref
          ) {
            item.ticketType.event =
              refMap[item.ticketType.event.$ref] || item.ticketType.event;
          }
          return item;
        });
      }
      return obj;
    }

    mapIds(data);
    return resolveDirectRefs(data);
  }

  // Calculate total amount
  calculateTotalAmount(): void {
    this.totalAmount = this.cart.reduce(
      (sum, item) => sum + item.ticketType.price * item.quantity,
      0
    );
  }

  // Remove item from cart
  removeCartItem(cartItemID: string): void {
    this.cartService.removeCartItem(cartItemID).subscribe(
      () => {
        this.cart = this.cart.filter((item) => item.cartItemID !== cartItemID);
        this.calculateTotalAmount();
      },
      (error) => {
        console.error('Error removing cart item:', error);
        alert('Failed to remove cart item. Please try again.');
      }
    );
  }

  proceedToCheckout(): void {
    this.router.navigate(['/checkout']);
  }
}
