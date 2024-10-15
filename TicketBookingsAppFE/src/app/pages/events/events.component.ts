import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EventComponent } from '../../components/event/event.component';
import { EventsService } from '../../services/events/events.service';
import { Event } from '../../models/Event'; // Update this model to match your DTO

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [EventComponent, FormsModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.css',
})
export class EventsComponent implements OnInit {
  events: Event[] = [];

  constructor(private eventsService: EventsService) {}

  ngOnInit(): void {
    console.log('Fetching events...');

    // Subscribe to the events service and handle the response
    this.eventsService.getAllEvents().subscribe(
      (data: any) => {
        // Log the response to the console for debugging
        console.log('API Response:', data);
        console.log(data.$values[0].categories);

        // Handle the $values structure (if present) or map directly if flat structure
        if (data && data.$values) {
          this.events = data.$values.map((item: any) => this.mapToEvent(item));
        } else {
          this.events = data.map((item: any) => this.mapToEvent(item));
        }
      },
      (error) => {
        console.error('Error fetching events', error); // Handle errors
      }
    );
  }

  // Function to map the API response to Event model
  private mapToEvent(item: any): Event {
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
      categories: item.categories.$values ? item.categories.$values : [], // Assuming this is an array of enums or strings
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
