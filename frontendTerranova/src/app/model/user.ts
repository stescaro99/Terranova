export class User {
    name = '';
    username = '';
    password = '';
    email = '';
    birthDate = '';
    city = '';
    country = '';
    imageUrl: string | ArrayBuffer | null | undefined = null;
    favoriteCocktails: string[] = [];
    createdCocktails: number[] = [];
    canDrinkAlcohol = false; 
    appPermissions = false;  
    language = '';
  }

export interface user {
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