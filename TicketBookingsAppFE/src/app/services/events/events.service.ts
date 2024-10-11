import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  allEvents: Event[] = [];

  // http = inject(HttpClient);
  constructor(private http: HttpClient) {} //` Injecting the HttpClient in to the constructor (Dependency Injection)

  baseRequestUrlHttps = 'https://localhost:7290/api/events';
  baseRequestUrlHttp = 'http://localhost:5027/api/events';

  getAllEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(this.baseRequestUrlHttp);
  }
}
