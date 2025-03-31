import { Routes } from '@angular/router';
import { DetailsComponent } from './details/details.component';
import { LoginComponent } from './login/login.component';
import { SiginComponent } from './sigin/sigin.component'; // Assicurati che questo componente esista

export const routeConfig: Routes = [
  {
    path: '',
    component: LoginComponent,
    title: 'homePage',
  },
  {
    path: 'details/:id',
    component: DetailsComponent, // Aggiungi il componente associato alla rotta
  },
  {
    path: 'sigIn',
    component: SiginComponent, // Assicurati che SigninComponent esista
  }
];