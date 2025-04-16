export class User {
    name = '';
    username = '';
    password = '';
    email = '';
    birthDate = '';
    city = '';
    country = '';
    imgUrl: string | ArrayBuffer | null | undefined = null;
    favoriteCocktails: any = null;
    createdCocktails: any = null;
    canDrinkAlcohol = false; 
    appPermissions = false;  
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
    favoriteCocktails: string[];
    createdCocktails: string[];
  }