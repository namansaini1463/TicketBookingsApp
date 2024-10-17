export interface Event {
  eventID: string; // Guid in the backend is mapped to a string in TypeScript
  name: string;
  dateAndTime: string; // DateTimeOffset maps to string (ISO 8601 format)
  venue: Venue; // Reference the VenueDTO structure
  eventDescription?: string; // Optional
  categories: string[]; // string enum
  ticketTypes: TicketType[]; // List of TicketTypeDTO
  images: EventImage[]; // list of EventImageDTO
  organizer: Organizer; // OrganizerDTO
  availableTickets: number; // This is a calculated field
  totalTicketPrice: number; // This is a calculated field
}

// Venue DTO structure
export interface Venue {
  name: string;
  address: string;
  state: string;
  capacity: number;
}

// TicketType DTO structure
export interface TicketType {
  type: string;
  price: number;
  quantityAvailable: number;
}

// EventImage DTO structure
export interface EventImage {
  imageUrl: string;
}

// Organizer DTO structure
export interface Organizer {
  name: string;
  contactEmail: string;
  phoneNumber: string;
}
