import { Component } from '@angular/core';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BackgroundComponent } from '../background/background.component';
import { CocktailService } from '../services/cocktail.service';
import { Cocktail, CocktailApiDrink } from '../models/cocktail';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, BackgroundComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent {
  user: User;
  isAuthenticated: boolean;
  isDropdownOpen: boolean = false;
  
  cocktails: CocktailApiDrink[] = []; 
  
  constructor(private userService: UserService, private route: ActivatedRoute, private router: Router, private cocktailService: CocktailService, private cdr: ChangeDetectorRef) {
    this.user = this.userService.getUser();
    console.log('Valore di user.ImgUrl:', this.user.ImgUrl);
    this.isAuthenticated = !!sessionStorage.getItem('authToken');
    if (!this.user.ImgUrl) {
      console.log('Nessuna immagine trovata, verrà mostrato il cerchio grigio.');
    }
  }

  onCocktailClick(cocktail: CocktailApiDrink) {
    console.log('Cocktail selezionato:', cocktail);
  this.router.navigate(['/cocktail', cocktail.idDrink], { queryParams: { id: cocktail.idDrink } });
  }

  ngOnInit(): void {
    this.takeCocktails(); 
  }

  takeCocktails() {
    this.cocktailService.takeCocktailOfDay(10, true).subscribe(
      (response: any) => {
        if (response && response.length > 0) {
          this.cocktails = response.map((item: any)=> item.drink);
          console.log('Cocktail del giorno:', this.cocktails);
          this.cdr.detectChanges();
        } else {
          console.error('La proprietà "drink" non è presente nella risposta:', response);
        }
      },
      (error) => {
        console.error('Errore durante il recupero del cocktail del giorno:', error);
      }
    );
  }

  
}
