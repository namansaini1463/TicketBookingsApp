import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { EventsComponent } from './pages/events/events.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookEventComponent } from './pages/book-event/book-event.component';
import { AuthGuard } from './guards/auth.guard';
import { BookingsComponent } from './pages/bookings/bookings.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { EventComponent } from './pages/event/event.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'events', component: EventsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'event/:eventId', component: EventComponent },
  {
    path: 'book/:eventId/:ticketId',
    component: BookEventComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bookings',
    component: BookingsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard],
  },
  //Wildcard route to handle invalid URLs
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
