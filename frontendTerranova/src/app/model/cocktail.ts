export class Cocktail {
    Id: number;
    drink: CocktailApiDrink;
    CreatedByUserId?: number;
    FavoriteByUsers: string[]; 
	isPrivate: boolean;

    constructor(init?: Partial<Cocktail>) {
      this.Id = init?.Id || 0;
      this.drink = init?.drink || new CocktailApiDrink();
      this.CreatedByUserId = init?.CreatedByUserId;
      this.FavoriteByUsers = init?.FavoriteByUsers || [];
	  this.isPrivate = false;
    }
  }
  
  export class CocktailApiDrink {
    idDrink?: string;
    strDrink?: string;
    strDrinkAlternate?: string;
    strTags?: string;
    strVideo?: string;
    strCategory?: string;
    strIBA?: string;
    strAlcoholic?: string;
    strGlass?: string;
    strInstructions?: string;
    strInstructionsES?: string;
    strInstructionsDE?: string;
    strInstructionsFR?: string;
    strInstructionsIT?: string;
    strInstructionsZH_HANS?: string;
    strInstructionsZH_HANT?: string;
    strDrinkThumb?: string;
    strIngredient1?: string;
    strIngredient2?: string;
    strIngredient3?: string;
    strIngredient4?: string;
    strIngredient5?: string;
    strIngredient6?: string;
    strIngredient7?: string;
    strIngredient8?: string;
    strIngredient9?: string;
    strIngredient10?: string;
    strIngredient11?: string;
    strIngredient12?: string;
    strIngredient13?: string;
    strIngredient14?: string;
    strIngredient15?: string;
    strMeasure1?: string;
    strMeasure2?: string;
    strMeasure3?: string;
    strMeasure4?: string;
    strMeasure5?: string;
    strMeasure6?: string;
    strMeasure7?: string;
    strMeasure8?: string;
    strMeasure9?: string;
    strMeasure10?: string;
    strMeasure11?: string;
    strMeasure12?: string;
    strMeasure13?: string;
    strMeasure14?: string;
    strMeasure15?: string;
    strImageSource?: string;
    strImageAttribution?: string;
    strCreativeCommonsConfirmed?: string;
    dateModified?: string;
  
    constructor(init?: Partial<CocktailApiDrink>) {
      Object.assign(this, init);
    }
  }