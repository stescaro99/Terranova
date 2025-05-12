import { Component, Input } from '@angular/core';
import { CocktailService } from '../../services/cocktail.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delete-button',
  imports: [],
  templateUrl: './delete-button.component.html',
  styleUrl: './delete-button.component.css'
})
export class DeleteButtonComponent {
  @Input() text: string = '';
  @Input() cocktailId: string = '';
  constructor(private cocktailService: CocktailService, private route: Router) {
    console.log('DeleteButtonComponent initialized');
  }
  onDelete(cocktailId: string) {
    this.cocktailService.deleteCocktail(cocktailId).subscribe(
      (response: any) => {
        console.log('Cocktail eliminato con successo:', response);
         console.log('Navigazione verso /home');
        this.route.navigate(['/home']);
      }, 
      (error) => {
        console.error('Errore durante l\'eliminazione del cocktail:', error);
        this.route.navigate(['/home']);
      }
    );
  }
}
