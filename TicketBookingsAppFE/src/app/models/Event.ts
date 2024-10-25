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
  ticketTypeID: string;
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

export enum EventCategory {
  Music = 'Music',
  Sports = 'Sports',
  Technology = 'Technology',
  Education = 'Education',
  Business = 'Business',
  Arts = 'Arts',
  Health = 'Health',
  Entertainment = 'Entertainment',
  FoodAndDrink = 'FoodAndDrink',
  Fashion = 'Fashion',
  Charity = 'Charity',
  Comedy = 'Comedy',
  FilmAndTheater = 'FilmAndTheater',
  Literature = 'Literature',
  Networking = 'Networking',
  Science = 'Science',
  TravelAndAdventure = 'TravelAndAdventure',
  Workshop = 'Workshop',
  Conference = 'Conference',
  Festival = 'Festival',
  Religious = 'Religious',
  Fitness = 'Fitness',
  Family = 'Family',
  Gaming = 'Gaming',
  History = 'History',
  Politics = 'Politics',
  Spirituality = 'Spirituality',
  Environment = 'Environment',
  Photography = 'Photography',
  PetsAndAnimals = 'PetsAndAnimals',
}

//` Function to map the API response to Event model
export function mapToEvent(item: any): Event {
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
      ticketTypeID: tt.ticketTypeID,
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
