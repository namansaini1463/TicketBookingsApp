import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CartItem, UpdateCartItem } from '../../models/Cart';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private baseRequestUrlHttps = 'https://localhost:7290/api/cart';
  private baseRequestUrlHttp = 'http://localhost:5027/api/cart';

  constructor(private http: HttpClient) {}

  //` Add item to cart
  addToCart(cartItem: CartItem): Observable<any> {
    return this.http.post(`${this.baseRequestUrlHttp}/AddItem`, cartItem);
  }

  //` Get user's cart
  getCart(userId: string): Observable<any> {
    return this.http.get(`${this.baseRequestUrlHttp}/${userId}`);
  }

  //` Update cart item
  updateCartItem(updateCartItem: UpdateCartItem): Observable<any> {
    return this.http.put(
      `${this.baseRequestUrlHttp}/UpdateItem`,
      updateCartItem
    );
  }

  //` Remove item from cart
  removeCartItem(cartItemId: string): Observable<any> {
    return this.http.delete(
      `${this.baseRequestUrlHttp}/RemoveItem/${cartItemId}`
    );
  }

  //` Validate Coupon
  validateCoupon(
    couponCode: string
  ): Observable<{ discountValue: number; isPercentage: boolean }> {
    return this.http.get<{ discountValue: number; isPercentage: boolean }>(
      `http://localhost:5027/api/Coupon/Get/${couponCode}`
    );
  }
}
