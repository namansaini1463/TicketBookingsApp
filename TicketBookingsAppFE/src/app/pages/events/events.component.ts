import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EventComponent } from '../../components/event/event.component';
import { EventsService } from '../../services/events/events.service';
import { BookingEvent, EventCategory } from '../../models/Event'; // Update this model to match your DTO
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [EventComponent, FormsModule, CommonModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.css',
})
export class EventsComponent implements OnInit {
  events: BookingEvent[] = [];

  // Filter properties
  searchTerm: string = '';
  sortBy: string = 'name'; // Default sort by name
  sortOrder: 'asc' | 'desc' = 'asc'; // Default to ascending order
  startDate: string = ''; // To store the selected start date
  endDate: string = ''; // To store the selected end date
  selectedCategory: EventCategory | null = null; // Selected category for filtering
  eventCategories: EventCategory[] = Object.values(EventCategory); // List of categories from enum

  // Toggle state for the filter section
  showFilters: boolean = true;
  // Toggle filter visibility
  toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  constructor(private eventsService: EventsService) {}

  ngOnInit(): void {
    console.log('Fetching events...');
    this.loadEvents(); // Initial load
  }

  // Load events from the backend with the current filters
  loadEvents(): void {
    this.eventsService
      .getAllEvents(
        this.searchTerm, // Pass the searchTerm to the backend API
        this.sortBy,
        this.sortOrder,
        this.startDate,
        this.endDate
      )
      .subscribe(
        (data: any) => {
          // Handle the $values structure (if present) or map directly if flat structure
          if (data && data.$values) {
            this.events = data.$values.map((item: any) =>
              this.mapToEvent(item)
            );
          } else {
            this.events = data.map((item: any) => this.mapToEvent(item));
          }
        },
        (error) => {
          console.error('Error fetching events', error); // Handle errors
        }
      );
  }

  // Search function triggered when user types into the search bar
  onSearch(): void {
    this.loadEvents(); // Reload events when search term changes
  }

  // Apply the filter changes (start date, end date, etc.)
  onFilterChange(): void {
    this.loadEvents();
  }

  // Remove filters
  onFilterRemove(): void {
    this.startDate = '';
    this.endDate = '';
    this.loadEvents();
  }

  // Handle category change
  onCategoryChange(category: EventCategory | null): void {
    this.selectedCategory = category;
    this.filterEventsByCategory();
  }

  // Filter events by category
  filterEventsByCategory(): void {
    this.eventsService
      .getAllEvents(
        this.searchTerm, // Pass searchTerm with category filter
        'name',
        'asc',
        '',
        '',
        this.selectedCategory ? this.selectedCategory : undefined
      )
      .subscribe(
        (data: any) => {
          if (data && data.$values) {
            this.events = data.$values.map((item: any) =>
              this.mapToEvent(item)
            );
          } else {
            this.events = data.map((item: any) => this.mapToEvent(item));
          }
        },
        (error) => {
          console.error('Error fetching events', error);
        }
      );
  }

  // Change sorting order and field
  onSortChange(field: string, order: 'asc' | 'desc'): void {
    this.sortBy = field;
    this.sortOrder = order;
    this.loadEvents(); // Reload events with the new sorting criteria
  }

  // Map the API response to the Event model
  private mapToEvent(item: any): BookingEvent {
    return {
      eventID: item.eventID,
      name: item.name,
      dateAndTime: item.dateAndTime,
      venue: {
        name: item.venue.name,
        address: item.venue.address,
        state: item.venue.state,
        capacity: item.venue.capacity,
      },
      eventDescription: item.eventDescription || '',
      categories: item.categories.$values ? item.categories.$values : [],
      ticketTypes: item.ticketTypes.$values.map((tt: any) => ({
        type: tt.type,
        price: tt.price,
        quantityAvailable: tt.quantityAvailable,
      })),
      images: item.images.$values.map((img: any) => ({
        imageUrl: img.imageUrl,
      })),
      organizer: {
        name: item.organizer.name,
        contactEmail: item.organizer.contactEmail,
        phoneNumber: item.organizer.phoneNumber,
      },
      availableTickets: item.availableTickets,
      totalTicketPrice: item.totalTicketPrice,
    };
  }
}
