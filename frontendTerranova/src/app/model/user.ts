export class User {
	id = ''
    name = '';
    username = '';
    password = '';
    email = '';
    birthDate = '';
    city = '';
    country = '';
    imageUrl: string | ArrayBuffer | null | undefined = null;
    favoriteCocktails: string[] = [];
    createdCocktails: string[] = [];
    canDrinkAlcohol = false; 
    appPermissions = false;  
    language = '';
  }

export interface user {
	id: string;
    name: string;
    username: string;
    email: string;
    password: string;
    birthDate: string;
    country: string;
    city: string;
    canDrinkAlcohol: boolean;
    appPermissions: boolean;
    imageUrl: string;
    favoriteCocktails: number[];
    createdCocktails: string[];
    language: string;
  }