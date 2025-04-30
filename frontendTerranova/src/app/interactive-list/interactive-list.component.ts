import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CocktailApiDrink } from '../model/cocktail';
import { Router } from '@angular/router';
import { StarButtonComponent } from '../button/star-button/star-button.component';
import { User } from '../model/user';
import { Form } from '@angular/forms';

@Component({
  selector: 'app-interactive-list',
  imports: [CommonModule, StarButtonComponent],
  templateUrl: './interactive-list.component.html',
  styleUrl: './interactive-list.component.css'
})
export class InteractiveListComponent {
  @Input() phrase: string = '';
  @Input() cocktails: any[] = [];
  @Input() isListVisible: boolean = true;
  @Input() canBeFavorite: boolean = true;
  @Input() user!: User ;
  @Output() favoritChanged = new EventEmitter<void>();

  constructor(private router: Router) {}

  toggleListVisibility(): void {
    this.isListVisible = !this.isListVisible;
  }

  onCocktailClick(cocktail: CocktailApiDrink): void {
    console.log('Cocktail selezionato:', cocktail);
    this.router.navigate(['/cocktail', cocktail.idDrink], { queryParams: { id: cocktail.idDrink } });
  }
  onFavoriteChanged(): void {
    this.favoritChanged.emit();
  }
}
