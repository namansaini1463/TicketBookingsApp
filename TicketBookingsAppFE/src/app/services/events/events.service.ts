import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, of, throwError } from 'rxjs';
import { EventCategory } from '../../models/Event';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  // http = inject(HttpClient);
  constructor(private http: HttpClient) {} //` Injecting the HttpClient in to the constructor (Dependency Injection)

  baseRequestUrlHttps = 'https://localhost:7290/api/events';
  baseRequestUrlHttp = 'http://localhost:5027/api/events';

  getAllEvents(
    searchTerm: string = '',
    sortBy: string = 'name',
    sortOrder: 'asc' | 'desc' = 'asc',
    startDate?: string,
    endDate?: string,
    category?: EventCategory
  ): Observable<Event[]> {
    let params = new HttpParams();

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (sortBy) {
      params = params.set('sortBy', sortBy);
    }
    if (sortOrder) {
      params = params.set('sortOrder', sortOrder);
    }
    if (startDate) {
      params = params.set('startDate', startDate);
    }
    if (endDate) {
      params = params.set('endDate', endDate);
    }
    if (category) {
      params = params.set('eventCategory', category); // Pass category to the API
    }

    return this.http.get<Event[]>(this.baseRequestUrlHttp, { params }).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          // Handle 404 error by returning an empty array or a specific value
          console.error('No events found:', error.message);
          return of([]); // Return an empty array
        } else {
          // Re-throw other errors to be handled elsewhere
          console.error('An error occurred:', error.message);
          return throwError(error);
        }
      })
    );
  }

  getEventByID(eventID: string): Observable<Event> {
    return this.http.get<Event>(`${this.baseRequestUrlHttp}/${eventID}`);
  }
}
