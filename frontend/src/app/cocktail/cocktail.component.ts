import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Cocktail, CocktailApiDrink } from '../models/cocktail';
import { CocktailService } from '../services/cocktail.service';
import { BackgroundComponent } from '../background/background.component';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-cocktail',
  imports: [CommonModule, BackgroundComponent],
  templateUrl: './cocktail.component.html',
  styleUrl: './cocktail.component.scss'
})
export class CocktailComponent {
  cocktailId: string | null = null;
  cocktail: CocktailApiDrink | null = null;
  user: User ;
  name: string = '';
  ingredients: string[] = [];

  constructor(private route: ActivatedRoute, private cocktailService: CocktailService, userService: UserService) {
    this.user = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '') : new User();
  }
  
  ngOnInit(): void {
    console.log('user', this.user);
    this.name = this.user.username;
    console.log('username', this.name);
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

  isFavorite(): boolean {
    if (this.user?.favoriteCocktails.find((id: string) => id === this.cocktailId)) {
      return this.user.favoriteCocktails.includes(this.cocktailId || '');
    } else {
      return false;
    }
  }
  
  toggleFavorite(): void {
    console.log('CocktailId:', this.cocktailId);
    console.log('Username:', this.name);
    if (!this.user?.username || !this.cocktailId) {
      console.error('Username o CocktailId non disponibile.');
      return;
    }
    const request = {
      Username: this.user.username,
      CocktailId: this.cocktailId
    };
  
    this.cocktailService.setFavorite(request).subscribe(
      (response: any) => {
        console.log(response.Message);
  
        if (this.user?.favoriteCocktails.includes(this.cocktailId)) {
          this.user.favoriteCocktails = this.user.favoriteCocktails.filter(
            (id: string) => id !== this.cocktailId
          );
        } else {
          this.user?.favoriteCocktails.push(this.cocktailId);
        }
      },
      (error) => {
        console.error('Errore durante l\'aggiornamento dei preferiti:', error);
      }
    );
  }

  isGuestUser(): boolean {
      if (localStorage.getItem('guestToken') === 'true') {
        return true;
    }
    else
    return false;
  }
}
