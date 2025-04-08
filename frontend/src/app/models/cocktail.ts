export class Cocktail {
    Id: number;
    drink: CocktailApiDrink;
    CreatedByUserId?: number;
    FavoriteByUsers: string[]; // Array di ID o nomi degli utenti che hanno aggiunto il cocktail ai preferiti
  
    constructor(init?: Partial<Cocktail>) {
      this.Id = init?.Id || 0;
      this.drink = init?.drink || new CocktailApiDrink();
      this.CreatedByUserId = init?.CreatedByUserId;
      this.FavoriteByUsers = init?.FavoriteByUsers || [];
    }
  }
  
  export class CocktailApiDrink {
    IdDrink?: string;
    StrDrink?: string;
    StrDrinkAlternate?: string;
    StrTags?: string;
    StrVideo?: string;
    StrCategory?: string;
    StrIBA?: string;
    StrAlcoholic?: string;
    StrGlass?: string;
    StrInstructions?: string;
    StrInstructionsES?: string;
    StrInstructionsDE?: string;
    StrInstructionsFR?: string;
    StrInstructionsIT?: string;
    StrInstructionsZH_HANS?: string;
    StrInstructionsZH_HANT?: string;
    StrDrinkThumb?: string;
    StrIngredient1?: string;
    StrIngredient2?: string;
    StrIngredient3?: string;
    StrIngredient4?: string;
    StrIngredient5?: string;
    StrIngredient6?: string;
    StrIngredient7?: string;
    StrIngredient8?: string;
    StrIngredient9?: string;
    StrIngredient10?: string;
    StrIngredient11?: string;
    StrIngredient12?: string;
    StrIngredient13?: string;
    StrIngredient14?: string;
    StrIngredient15?: string;
    StrMeasure1?: string;
    StrMeasure2?: string;
    StrMeasure3?: string;
    StrMeasure4?: string;
    StrMeasure5?: string;
    StrMeasure6?: string;
    StrMeasure7?: string;
    StrMeasure8?: string;
    StrMeasure9?: string;
    StrMeasure10?: string;
    StrMeasure11?: string;
    StrMeasure12?: string;
    StrMeasure13?: string;
    StrMeasure14?: string;
    StrMeasure15?: string;
    StrImageSource?: string;
    StrImageAttribution?: string;
    StrCreativeCommonsConfirmed?: string;
    DateModified?: string;
  
    constructor(init?: Partial<CocktailApiDrink>) {
      Object.assign(this, init);
    }
  }