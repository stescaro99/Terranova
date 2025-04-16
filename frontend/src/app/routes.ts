import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SiginComponent } from './sigin/sigin.component';
import { HomePageComponent } from './home-page/home-page.component';
import { authGuard } from './auth.guard';
import { CocktailComponent } from './cocktail/cocktail.component';

export const routeConfig: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomePageComponent,
    canActivate: [authGuard],
  },
  {
    path: 'login',
    component: LoginComponent, 
  },
  {
    path: 'sigIn',
    component: SiginComponent, 
  },
  {
    path: 'cocktail/:id',
    component: CocktailComponent,
  }
];