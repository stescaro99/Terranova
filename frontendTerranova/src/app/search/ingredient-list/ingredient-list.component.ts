import { Component } from '@angular/core';
import { ingredientColors } from '../../model/ingredient'
import { Input } from '@angular/core';

@Component({
  selector: 'app-ingredient-list',
  imports: [],
  templateUrl: './ingredient-list.component.html',
  styleUrl: './ingredient-list.component.css'
})
export class IngredientListComponent {
	@Input() limitIngredients: string[] = [] ;
	availableIngredients: string[] = [];
	selectedIngredient: string = '';

	ngOnInit() {
	this.availableIngredients = Object.keys(ingredientColors).filter(
		ingredient => !this.limitIngredients.includes(ingredient)
	);
	}

	onIngredientSelect(event: Event) {
	const input = event.target as HTMLInputElement;
	this.selectedIngredient = input.value;
	console.log('Ingrediente selezionato:', this.selectedIngredient);
	}
}
