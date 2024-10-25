import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { EventImage } from '../../../models/Event';

@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css',
})
export class CarouselComponent {
  @Input() images: EventImage[] = [];
  currentIndex = 0;

  nextSlide() {
    this.currentIndex = (this.currentIndex + 1) % this.images.length;
  }
  prevSlide() {
    this.currentIndex =
      (this.currentIndex - 1 + this.images.length) % this.images.length;
  }

  goTo(index: number) {
    if (index >= 0 || index < this.images.length) {
      this.currentIndex = index;
    }
  }
}
