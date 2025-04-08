import { Component } from '@angular/core';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BackgroundComponent } from '../background/background.component';
import { CocktailService } from '../services/cocktail.service';
import { Cocktail, CocktailApiDrink } from '../models/cocktail';


@Component({
  selector: 'app-home-page',
  imports: [CommonModule, BackgroundComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent {
  user: User;
  isAuthenticated: boolean;
  isDropdownOpen: boolean = false;
  
  cocktail: CocktailApiDrink | null = null; 
  
  constructor(private userService: UserService, private router: Router, private cocktailService: CocktailService) {
    this.user = this.userService.getUser();
    console.log('Valore di user.ImgUrl:', this.user.ImgUrl);
    this.isAuthenticated = !!sessionStorage.getItem('authToken');
    if (!this.user.ImgUrl) {
      console.log('Nessuna immagine trovata, verrà mostrato il cerchio grigio.');
    }
  }

  ngOnInit(): void {
    this.takeCocktail(); 
  }

  takeCocktail() {
    this.cocktailService.takeCocktailOfDay(1, true).subscribe(
      (response: any) => {
        if (response && response[0].drink) {
          this.cocktail = response[0].drink;
          console.log('Cocktail del giorno:', this.cocktail);
        } else {
          console.error('La proprietà "drink" non è presente nella risposta:', response);
        }
      },
      (error) => {
        console.error('Errore durante il recupero del cocktail del giorno:', error);
      }
    );
  }

  onUserPhotoClick() {
    console.log('User photo clicked!');
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  goToSettings() {
    console.log('Navigazione alle impostazioni...');
    this.router.navigate(['/settings']);
  }

  logout() {  
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('authToken');
    this.userService.setUser(new User());
    window.location.reload();
  }

}
