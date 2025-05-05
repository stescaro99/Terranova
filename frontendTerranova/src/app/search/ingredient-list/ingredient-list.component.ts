import { Component, Input, Output, EventEmitter  } from '@angular/core';
import { ingredientColors } from '../../model/ingredient'
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ingredient-list',
  imports: [CommonModule],
  templateUrl: './ingredient-list.component.html',
  styleUrl: './ingredient-list.component.css'
})
export class IngredientListComponent {
	@Input() limitIngredients: string[] = [] ;
	@Input() selectedIngredient: string = '';
	@Output() ingredientSelected = new EventEmitter<{ name: string; color: string }>();

  get availableIngredients(): string[] {
    return Object.keys(ingredientColors).filter(
      ingredient => !this.limitIngredients.includes(ingredient)
    );
  }

  ngOnInit() {
    
    console.log('ingredienti da filtrare:', this.limitIngredients);
  }

  onIngredientSelect(event: Event) {
    const input = event.target as HTMLInputElement;
    const selectedValue = input.value;
    if (this.availableIngredients.includes(selectedValue)) {
      this.selectedIngredient = selectedValue;
      const color = ingredientColors[this.selectedIngredient] || '#000000';
  
      this.ingredientSelected.emit({ name: this.selectedIngredient, color });
      console.log('Ingrediente selezionato:', this.selectedIngredient, 'Colore:', color);
    }
  }
}
