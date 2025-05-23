import { Component, OnInit, OnDestroy } from '@angular/core';
import { User } from '../model/user';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CocktailService } from '../services/cocktail.service';
import { Cocktail, CocktailApiDrink } from '../model/cocktail';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchComponent } from '../search/search.component';
import { TranslateService } from '../services/translate.service';
import { Subscription } from 'rxjs';
import { WindowService } from '../services/window.service';
import { InteractiveListComponent } from '../interactive-list/interactive-list.component';
import { CreateButtonComponent } from '../button/create-button/create-button.component';
import { BackgroundComponent } from '../background/background.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, SearchComponent, InteractiveListComponent, CreateButtonComponent, BackgroundComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit, OnDestroy{
  user: User;
  isAuthenticated: boolean;
  isDropdownOpen: boolean = false;
  isListVisible: boolean = true;
  isGuest: boolean = false;
  nuberOfDrink: number = 0;
  cocktails: CocktailApiDrink[] = [];
  favoriteCocktails: CocktailApiDrink[] = [];
  recommendedCocktails: CocktailApiDrink[] = [];
  yourCocktails: CocktailApiDrink[] = [];
  
  stringsToTranslate: string[] = [
    'Welcome to the Home Page',
    'Cocktail',
    'Favorite Cocktails',
    'You have not added any cocktails to your favorites yet.',
    'Recommended Cocktails for you',
	  'Make your cocktail',
    'search cocktail:',
    'Your Cocktails:',
  ];
  translatedText: string[] = [];
  private languageChangeSubscription!: Subscription;

  constructor(private userService: UserService, private translateService: TranslateService,
		 private router: Router, private cocktailService: CocktailService, private cdr: ChangeDetectorRef,
			private windowService: WindowService) {
    this.user = userService.getUser() || new User();
    
    console.log('Valore di user.ImageUrl:', this.user.imageUrl);
    this.isAuthenticated = !!localStorage.getItem('authToken');
    if (!this.user.imageUrl) {
      console.log('Nessuna immagine trovata, verrà mostrato il cerchio grigio.');
    }
	this.nuberOfDrink = windowService.getRecommendedDrinkCount();
  }
  
  onCocktailClick(cocktail: CocktailApiDrink): void {
    console.log('Cocktail selezionato:', cocktail);
    this.router.navigate(['/cocktail', cocktail.idDrink], { queryParams: { id: cocktail.idDrink } });
  }
  
  ngOnInit(): void {
    if (localStorage.getItem('guestToken') === 'true') {
      this.isGuest = true;
    }
    this.isListVisible = this.windowService.getListVisibility();
    this.takeCocktails();
  
    this.userService.getCocktailsFavorite(this.user.username).subscribe(
      (response: any) => {
        for(let i = 0; i < response.length; i++) {
          this.favoriteCocktails.push(response[i].drink);
        }
        console.log('Cocktail preferiti:', this.favoriteCocktails);
      });
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
      this.userService.getRecommendedtails(this.user.username).subscribe(
        (response: any) => {
          for (let i = 0; i < response.length; i++) {
            this.recommendedCocktails.push(response[i].drink);
          }
          console.log('Cocktail raccomandati:', this.recommendedCocktails);
        });
      this.languageChangeSubscription = this.userService.getLanguageChangeObservable().subscribe((language) => {
        this.updateTranslations(this.user.language);
      });
      this.userService.getUserCocktails(this.user.username).subscribe(
        (response: any) => { 
          for (let i = 0; i < response.length; i++) {
            this.yourCocktails.push(response[i].drink);
          }
          console.log('I tuoi cocktail:', this.yourCocktails);
        },
        (error) => {
          console.error('Errore durante il recupero dei cocktail dell\'utente:', error);
        }
      );
      const currentLanguage = this.userService.getUser()?.language || 'en';
      this.updateTranslations(currentLanguage);
      console.log('user', this.user);
  }

  takeCocktails() {
    this.cocktailService.takeCocktailOfDay(this.nuberOfDrink, !this.user.canDrinkAlcohol).subscribe(
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

  onFavoriteChanged() {
    console.log('Lo stato dei preferiti è cambiato.');
  this.favoriteCocktails = [];
  this.userService.getCocktailsFavorite(this.user.username).subscribe(
    (response: any) => {
      for (let i = 0; i < response.length; i++) {
        this.favoriteCocktails.push(response[i].drink);
      }
    },
    (error) => {
    }
  );    
  }
  updateTranslations(language: string): void {
    if (this.user.language !== 'en') {
        
      this.translatedText = [];
      this.stringsToTranslate.forEach((text, index) => {
        this.translateService.translateText(text, this.user.language).subscribe(
          (response) => {
            this.translatedText[index] = response.translatedText;
            this.cdr.detectChanges();
          },
          (error) => {
            console.error('Errore durante la traduzione:', error);
          }
        );
      });
    }
  }
  ngOnDestroy(): void {
  }
}
