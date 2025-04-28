export class User {
    name = '';
    username = '';
    password = '';
    email = '';
    birthDate = '';
    city = '';
    country = '';
    imgUrl: string | ArrayBuffer | null | undefined = null;
    favoriteCocktails: string[] = [];
    createdCocktails: string[] = [];
    canDrinkAlcohol = false; 
    appPermissions = false;  
    language = ''; // Lingua predefinita
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
  }