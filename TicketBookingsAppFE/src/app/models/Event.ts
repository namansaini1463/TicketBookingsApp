export interface Event {
  eventID: string; // Guid in the backend is mapped to a string in TypeScript
  name: string;
  dateAndTime: string; // DateTimeOffset maps to string (ISO 8601 format)
  venue: Venue; // Updated to reference the VenueDTO structure
  eventDescription?: string; // Optional field
  categories: string[]; // Assuming categories are strings or enums, adjust as needed
  ticketTypes: TicketType[]; // Updated to reflect the list of TicketTypeDTO
  images: EventImage[]; // Updated to reflect the list of EventImageDTO
  organizer: Organizer; // Updated to reflect the OrganizerDTO
  availableTickets: number; // This is a calculated field, no change needed
  totalTicketPrice: number; // This is a calculated field, no change needed
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
  type: string; // Type of ticket (e.g., VIP, General Admission)
  price: number; // Price of the ticket
  quantityAvailable: number; // Available tickets for this type
}

// EventImage DTO structure
export interface EventImage {
  imageUrl: string; // URL of the event image
}

// Organizer DTO structure
export interface Organizer {
  name: string; // Name of the organizer
  contactEmail: string; // Email of the organizer
  phoneNumber: string; // Phone number of the organizer
}
