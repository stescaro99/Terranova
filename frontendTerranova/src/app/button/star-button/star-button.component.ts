import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CocktailService } from '../../services/cocktail.service';
import { UserService } from '../../services/user.service';
import { User } from '../../model/user';

@Component({
  selector: 'app-star-button',
  imports: [CommonModule],
  templateUrl: './star-button.component.html',
  styleUrl: './star-button.component.css'
})
export class StarButtonComponent {
  @Input() cocktailId!: string | null;
  @Input() user!: User;
  @Output() favoriteChanged = new EventEmitter<void>();

  constructor(private cocktailService: CocktailService, private userService: UserService) {
  }

  isFavorite(): boolean {
      if (!this.cocktailId || !this.user?.favoriteCocktails) {
        console.log('CocktailId o favoriteCocktails non disponibile.');
        return false;
      }
    
      const isFavorite = this.user.favoriteCocktails.some(
        (id: string | number) => id.toString() === this.cocktailId
      );
    
      return isFavorite;
    }
    
    toggleFavorite(): void {
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
    
          if (this.cocktailId && this.user?.favoriteCocktails.includes(this.cocktailId)) {
            this.user.favoriteCocktails = this.user.favoriteCocktails.filter(
              (id: string) => id !== this.cocktailId
            );
          } else if (this.cocktailId) {
            this.user?.favoriteCocktails.push(this.cocktailId);
          }
          this.favoriteChanged.emit();
        },
        (error) => {
          console.error('Errore durante l\'aggiornamento dei preferiti:', error);
        }
      );
      this.userService.getUserbyUsername(this.user.username).subscribe(
        (user: User) => {
          this.userService.setUser(user);
          localStorage.removeItem('user');
          localStorage.setItem('user', JSON.stringify(user));
        },
        (error) => {
          console.error('Errore durante il recupero dell\'utente:', error);
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
