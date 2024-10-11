import { Component, Input } from '@angular/core';

export interface Event {
  eventID: string; // Guid maps to string in TypeScript
  name: string;
  date: string; // DateOnly doesn't have a direct equivalent, using ISO date string
  time: string; // TimeOnly, similarly, can be represented as a string (e.g., 'HH:mm:ss')
  venue: string;
  state: string;
  ticketPrice: number; // decimal maps to number in TypeScript
  availableTickets: number;
  eventDescription?: string; // '?' makes the property optional
}

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [],
  templateUrl: './event.component.html',
  styleUrl: './event.component.css',
})
export class EventComponent {
  @Input() event!: Event;
}
