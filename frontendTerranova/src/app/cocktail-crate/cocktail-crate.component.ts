import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Cocktail, CocktailApiDrink } from '../model/cocktail';
import { Input } from '@angular/core';
import { UserService } from '../services/user.service';
import { CocktailService } from '../services/cocktail.service';
import { User } from '../model/user';
import { IngredientListComponent } from '../search/ingredient-list/ingredient-list.component';
import { ingredientColors } from '../model/ingredient';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cocktail-crate',
  imports: [CommonModule, IngredientListComponent, FormsModule],
  templateUrl: './cocktail-crate.component.html',
  styleUrl: './cocktail-crate.component.css'
})
export class CocktailCrateComponent {
	@Input() idCocktail! : string | null ;
	user : User ;
	newcocktail: Cocktail = new Cocktail();
	oldcocktail?: CocktailApiDrink;
	useRealimg: boolean = true;
	ingredients: string[] = [];
	ingredientColors: { [key: string]: string } = {};
	numberOfIngredients: number = 1;
	mode: 'mix' | 'pur' | 'shake' = 'mix';

	constructor(private userservice: UserService, private cocktailservice: CocktailService) {
		this.user = userservice.getUser() || new User();
		if (this.idCocktail) {
		  this.cocktailservice.takeCocktailById(this.idCocktail).subscribe(
			(cocktail: CocktailApiDrink) => {
			  this.oldcocktail = cocktail;
			  for(const object in  this.oldcocktail)
			  {
				if (this.oldcocktail.hasOwnProperty(object))
				{
					if (object === 'strInstructions')
					{
						this.newcocktail.drink.strInstructionsZH_HANS = (this.oldcocktail as any)[object];
						continue;
					}
					(this.newcocktail.drink as any)[object] = (this.oldcocktail as any)[object];
					
				}
			  }
			},
			(error) => {
			  console.error('error to find cocktail id');
			}
		  );
		}
	}
	setMode(mode: 'mix' | 'shake'): void {
		this.mode = mode;
	}

	onIngredientSelected(event: { name: string; color: string }, index: number): void {
		const { name, color } = event;
	  
		// Rimuovi l'ingrediente precedente dallo slot
		if (this.ingredients[index]) {
		  const previousIngredient = this.ingredients[index];
		  console.log(`Rimuovendo ingrediente precedente: ${previousIngredient}`);
		  delete this.ingredientColors[previousIngredient];
		}
	  
		// Aggiorna l'ingrediente nello slot corrente
		this.ingredients[index] = name;
		this.ingredientColors[name] = color;
	  
		console.log('Ingredienti aggiornati:', this.ingredients);
		console.log('Colori aggiornati:', this.ingredientColors);
	  }
	
	getGradient(): string {
		if (this.ingredients.length === 0) {
			return '#000000';
		}
		
		const colors = this.ingredients
			.map(ingredient => this.ingredientColors[ingredient] || '#000000')
			.filter(color => color); // Filtra eventuali valori nulli o undefined
		
		if (colors.length === 0) {
			return '#000000';
		}
		
		// Crea un gradiente lineare con i colori disponibili
		return `linear-gradient(to bottom, ${colors.join(', ')})`;
	}
	getMixedColor(): string {
		const colors = this.ingredients.map(ingredient => this.ingredientColors[ingredient] || 'rgba(0, 0, 0, 1)');
		console.log('Colori degli ingredienti:', colors);
	  
		if (colors.length === 0) {
		  return 'rgba(0, 0, 0, 1)'; // Colore di fallback
		}
	  
		let r = 0, g = 0, b = 0, a = 0;
	  
		colors.forEach(color => {
		  const rgba = this.rgbaToComponents(color);
		  if (rgba) {
			r += rgba.r;
			g += rgba.g;
			b += rgba.b;
			a += rgba.a;
		  }
		});
	  
		// Calcola la media dei valori RGBA
		r = Math.round(r / colors.length);
		g = Math.round(g / colors.length);
		b = Math.round(b / colors.length);
		a = Math.round((a / colors.length) * 100) / 100; // Mantieni due decimali per l'alpha
	  
		// Restituisci il colore medio in formato RGBA
		return `rgba(${r}, ${g}, ${b}, ${a})`;
	  }
	
	addIngredient(): void {
		if (this.numberOfIngredients < 15) {
			this.numberOfIngredients++;
		} else {
			console.log('Hai raggiunto il numero massimo di ingredienti.');
		}
	}
	rgbaToComponents(rgba: string): { r: number; g: number; b: number; a: number } | null {
		const match = rgba.match(/^rgba?\((\d+),\s*(\d+),\s*(\d+),?\s*([\d.]+)?\)$/);
		if (!match) {
		  console.log(`Colore non valido: ${rgba}`);
		  return null;
		}
		return {
		  r: parseInt(match[1], 10),
		  g: parseInt(match[2], 10),
		  b: parseInt(match[3], 10),
		  a: match[4] ? parseFloat(match[4]) : 1 // Alpha predefinito a 1 se non specificato
		};
	  }
}
