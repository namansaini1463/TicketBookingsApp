import { Component, OnInit } from '@angular/core';
import { CreateBookingDTO, PaymentStatus } from '../../models/Booking';
import { BookingService } from '../../services/booking/booking.service';
import { CartService } from '../../services/cart/cart.service';
import { UserService } from '../../services/user/user.service';
import { Router } from '@angular/router';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItem, CartItemDTO } from '../../models/Cart';
import { mapToEvent } from '../../models/Event';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  cart: CartItemDTO[] = [];
  userID: string | null = null;
  totalAmount: number = 0;
  discount: number = 0;
  finalAmount: number = 0;
  paymentStatus: PaymentStatus = PaymentStatus.Paid;
  couponError: string | null = null;

  constructor(
    private fb: FormBuilder,
    private bookingService: BookingService,
    private cartService: CartService,
    private userService: UserService,
    private router: Router
  ) {
    this.checkoutForm = this.fb.group({
      couponCode: [''],
      paymentMethod: ['', Validators.required],
      upiId: ['', this.upiValidator()], // UPI ID field without required validator by default
      cardNumber: ['', Validators.pattern(/^\d{16}$/)],
      cardName: ['', Validators.minLength(3)],
      expiryDate: ['', Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/)],
      cvv: ['', Validators.pattern(/^\d{3}$/)],
    });
  }

  ngOnInit(): void {
    this.userID = this.userService.getUserProfile()?.userID ?? null;
    if (!this.userID) {
      alert('User not logged in. Please log in to proceed with booking.');
      this.router.navigate(['/login']);
    } else {
      this.loadCartDetails();
    }

    // Subscribe to changes in paymentMethod to update validators dynamically
    this.checkoutForm.get('paymentMethod')?.valueChanges.subscribe((method) => {
      this.onPaymentMethodChange(method);
    });
  }

  // Update validators based on the selected payment method
  onPaymentMethodChange(method: string): void {
    const upiIdControl = this.checkoutForm.get('upiId');
    const cardControls = ['cardNumber', 'cardName', 'expiryDate', 'cvv'].map(
      (field) => this.checkoutForm.get(field)
    );

    // Clear all validators initially
    upiIdControl?.clearValidators();
    cardControls.forEach((control) => control?.clearValidators());

    // Set validators based on payment method
    if (method === 'UPI') {
      upiIdControl?.setValidators([Validators.required, this.upiValidator()]);
    } else if (method === 'Credit Card') {
      cardControls.forEach((control) =>
        control?.setValidators(Validators.required)
      );
    }

    // Update the form controls' validity
    upiIdControl?.updateValueAndValidity();
    cardControls.forEach((control) => control?.updateValueAndValidity());

    // Clear non-relevant fields
    this.checkoutForm.patchValue({
      upiId: '',
      cardNumber: '',
      cardName: '',
      expiryDate: '',
      cvv: '',
    });
  }

  loadCartDetails(): void {
    this.cartService.getCart(this.userID!).subscribe(
      (response) => {
        const resolvedResponse = this.resolveRefsOnce(response);
        this.cart = resolvedResponse.cartItems.$values.map((item: any) => ({
          cartID: item.cartID,
          cartItemID: item.cartItemID,
          quantity: item.quantity,
          dateAdded: item.dateAdded,
          ticketType: item.ticketType,
          event: mapToEvent(item.ticketType.event),
        }));
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
    function resolveDirectRefs(obj: any): any {
      if (obj && obj.cartItems && obj.cartItems.$values) {
        obj.cartItems.$values = obj.cartItems.$values.map((item: any) => {
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

  calculateTotalAmount(): void {
    this.totalAmount = this.cart.reduce(
      (sum, item) => sum + item.ticketType.price * item.quantity,
      0
    );
    this.finalAmount = this.totalAmount - this.discount;
  }

  applyCouponDiscount(): void {
    const couponCode = this.checkoutForm.value.couponCode;
    if (couponCode) {
      this.cartService.validateCoupon(couponCode).subscribe(
        (data) => {
          this.couponError = null;
          this.discount = data.isPercentage
            ? (this.totalAmount * data.discountValue) / 100
            : data.discountValue;
          this.finalAmount = this.totalAmount - this.discount;
        },
        (error) => {
          this.discount = 0;
          this.finalAmount = this.totalAmount;
          this.couponError = 'Invalid or expired coupon code.';
        }
      );
    } else {
      this.discount = 0;
      this.finalAmount = this.totalAmount;
    }
  }

  private upiValidator() {
    return (control: AbstractControl) => {
      const value = control.value || '';
      return value.includes('@') ? null : { invalidUpiId: true };
    };
  }

  onCheckout(): void {
    if (this.checkoutForm.invalid || !this.userID) {
      alert('Please fill in all required fields correctly.');
      return;
    }

    const bookingData: CreateBookingDTO = {
      userID: this.userID,
      couponCode: this.checkoutForm.value.couponCode || undefined,
      paymentMethod: this.checkoutForm.value.paymentMethod,
      paymentStatus: this.paymentStatus,
    };

    this.bookingService.createBooking(bookingData).subscribe(
      (response) => {
        alert(response.message);
        this.router.navigate(['/bookings']);
      },
      (error) => {
        alert('Failed to complete booking. Please try again.');
        console.error('Booking error:', error);
      }
    );
  }
}
