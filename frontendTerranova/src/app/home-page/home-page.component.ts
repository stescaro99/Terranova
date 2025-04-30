import { Component, OnInit, OnDestroy } from '@angular/core';
import { User } from '../model/user';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BackgroundComponent } from '../background/background.component';
import { CocktailService } from '../services/cocktail.service';
import { Cocktail, CocktailApiDrink } from '../model/cocktail';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchComponent } from '../search/search.component';
import { StarButtonComponent } from '../button/star-button/star-button.component';
import { TranslateService } from '../services/translate.service';
import { Subscription } from 'rxjs';
import { WindowService } from '../services/window.service';
import { InteractiveListComponent } from '../interactive-list/interactive-list.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, SearchComponent, InteractiveListComponent],
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
  
  stringsToTranslate: string[] = [
    'Benvenuto nella Home Page',
    'Cocktail',
    'Cocktail preferiti',
    'Non hai ancora aggiunto cocktail ai preferiti.'
  ];
  translatedText: string[] = [];
  private languageChangeSubscription!: Subscription;

  constructor(private userService: UserService, private translateService: TranslateService,
		 private router: Router, private cocktailService: CocktailService, private cdr: ChangeDetectorRef,
			private windowService: WindowService) {
    this.user = userService.getUser() || new User();
    
    console.log('Valore di user.ImgUrl:', this.user.imgUrl);
    console.log('authToken:', localStorage.getItem('authToken'));
    this.isAuthenticated = !!localStorage.getItem('authToken');
    if (!this.user.imgUrl) {
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
    console.log('user :', this.user);
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

      this.languageChangeSubscription = this.userService.getLanguageChangeObservable().subscribe((language) => {
        this.updateTranslations(language);
      });
      const currentLanguage = this.userService.getUser()?.language || 'en';
      this.updateTranslations(currentLanguage);
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
    if (this.user.language !== 'it') {
        
      this.translatedText = [];
      this.stringsToTranslate.forEach((text, index) => {
        this.translateService.translateText(text, language).subscribe(
          (response) => {
            this.translatedText[index] = response.translatedText;
            this.cdr.detectChanges(); // Forza il change detection
          },
          (error) => {
          }
        );
      });
    }
  }
  ngOnDestroy(): void {
  }
}
