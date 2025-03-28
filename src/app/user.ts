
export interface user {
    name: string;
    email: string;
    password: string;
    birthDate: string;
    country: string;
    city: string;
    canDrinkAlcohol: boolean;
    appPermissions: boolean;
    imageUrl: string;
    favoriteCocktails: string[]; // o altro tipo in base alla tua esigenza
    createdCocktails: string[];
     // idem
  }