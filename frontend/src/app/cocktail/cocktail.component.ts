import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Cocktail, CocktailApiDrink } from '../models/cocktail';
import { CocktailService } from '../services/cocktail.service';
import { BackgroundComponent } from '../background/background.component';

@Component({
  selector: 'app-cocktail',
  imports: [CommonModule, BackgroundComponent],
  templateUrl: './cocktail.component.html',
  styleUrl: './cocktail.component.scss'
})
export class CocktailComponent {
  cocktailId: string | null = null;
  cocktail: CocktailApiDrink | null = null;
  ingredients: string[] = [];

  constructor(private route: ActivatedRoute, private cocktailService: CocktailService) {}

  ngOnInit(): void {
    this.cocktailId = this.route.snapshot.paramMap.get('id');
    if (this.cocktailId) {
      console.log(this.cocktailId);
      this.cocktailService.takeCocktailById(this.cocktailId).subscribe(
        (response) => {
          this.cocktail = response.drink;
          console.log(this.cocktail);
          this.takeIngredients(this.cocktail);
        }
      );
    }
  }
  private takeIngredients(cocktail: any): void {
    this.ingredients = [];
    let j = 1; 
    for (let i = 1; i <= 15; i++) {
      const ingredient = cocktail[`strIngredient${i}`];
      const measure = cocktail[`strMeasure${j}`];
      if (ingredient == 'Ice')
      {
        this.ingredients.push('Ice - to taste');
        continue;
      }
      if (ingredient) {
        this.ingredients.push(`${ingredient} - ${measure || ''}`);
      }
      j++;
    }
  }
  
  addFavorite(cocktail: CocktailApiDrink): void {
    console.log('ciao');
  }
}
