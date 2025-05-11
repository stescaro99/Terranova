import { Routes } from '@angular/router';
import { LoginComponent } from './autentification/login/login.component';
import { SiginComponent } from './autentification/sigin/sigin.component';
import { HomePageComponent } from './home-page/home-page.component';
import { authGuard } from './guard/auth.guard';
import { CocktailComponent } from './cocktail/cocktail.component';
import { CocktailCrateComponent } from './cocktail-crate/cocktail-crate.component'

export const routes: Routes = [
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
	},
	{
	path: 'cocktail-create',
	component: CocktailCrateComponent,
	},
	{
	path:'cocktail-create/:id',
	component: CocktailCrateComponent,
	}
];
