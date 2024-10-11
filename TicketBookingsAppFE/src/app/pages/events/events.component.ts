import { Component, OnInit } from '@angular/core';
import { EventComponent } from '../../components/event/event.component';
import { EventsService } from '../../services/events/events.service';
import { Event } from '../../components/event/event.component'; //? This is the Event interface
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [EventComponent, FormsModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.css',
})
export class EventsComponent implements OnInit {
  events: Event[] = [];

  constructor(private eventsSerice: EventsService) {}

  ngOnInit(): void {
    this.eventsSerice.getAllEvents().subscribe(
      (data: any[]) => {
        this.events = data.map((item) => ({
          eventID: item.id,
          name: item.name,
          date: item.date,
          time: item.time,
          venue: item.venue,
          state: item.state,
          ticketPrice: item.ticketPrice,
          availableTickets: item.availableTickets,
          eventDescription: item.eventDescription || '',
        }));
      },
      (error) => {
        console.error('Error fetching events', error); // Handle any errors
      }
    );
  }
}
