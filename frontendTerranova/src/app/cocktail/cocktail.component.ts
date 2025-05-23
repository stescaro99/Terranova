import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Cocktail, CocktailApiDrink } from '../model/cocktail';
import { CocktailService } from '../services/cocktail.service';
import { User } from '../model/user';
import { UserService } from '../services/user.service';
import { StarButtonComponent } from '../button/star-button/star-button.component';
import { TranslateService } from './../services/translate.service';
import { Subscription } from 'rxjs';
import { ChangeDetectorRef } from '@angular/core';
import { BackgroundComponent } from '../background/background.component';
import { CreateButtonComponent} from '../button/create-button/create-button.component';
import { DeleteButtonComponent } from '../button/delete-button/delete-button.component';

@Component({
  selector: 'app-cocktail',
  imports: [CommonModule, StarButtonComponent, BackgroundComponent, CreateButtonComponent, DeleteButtonComponent],
  templateUrl: './cocktail.component.html',
  styleUrl: './cocktail.component.css'
})
export class CocktailComponent {
  cocktailId: string | null = null;
  cocktail: CocktailApiDrink | null = null;
  user: User ;
  name: string = '';
  ingredients: string[] = [];

  
originalString: string [] = [
	'Cocktail',
	'Category',
	'Glass',
	'Ingredients',
	'Instructions',
	'add to favorites',
	'create your cocktail starting from this',
  'delete cocktail',
];
  
  translatedString: string [] = [];
  private languageChangeSubscription!: Subscription;
  
  constructor(private route: ActivatedRoute, private translateService: TranslateService ,private cocktailService: CocktailService, private userService: UserService, private cdr: ChangeDetectorRef) {
    this.user =  this.userService.getUser()|| new User();
    console.log('username', this.name);
    this.cocktailId = this.route.snapshot.paramMap.get('id');
    if (this.cocktailId) {
      console.log(this.cocktailId);
      this.cocktailService.takeCocktailById(this.cocktailId).subscribe(
        (response) => {
          this.cocktail = response.drink;
          this.takeIngredients(this.cocktail);
          console.log('cocktailYYYYYYYYY', this.cocktail?.strInstructions);
        }
      );
    }
  }
  
  ngOnInit(): void {
    console.log('user', this.user);
    this.name = this.user.username;
    
    this.languageChangeSubscription = this.userService.getLanguageChangeObservable().subscribe((language) => {
      this.updateTranslations(this.user.language);
    });
    const currentLanguage = this.user.language || 'en';
    setTimeout(() => {
      this.updateTranslations(currentLanguage);
    }, 500);
  }
  
  private takeIngredients(cocktail: any): void {
    this.ingredients = [];
    let j = 1; 
    for (let i = 1; i <= 15; i++) {
      const ingredient = cocktail[`strIngredient${i}`];
      const measure = cocktail[`strMeasure${j}`];
      if (ingredient == 'Ice')
      {
        this.ingredients.push('Ice - to taste');
        continue;
      }
      if (ingredient && measure) {
        this.ingredients.push(`${ingredient} - ${measure || ''}`);
      }
      else if (ingredient) {
        this.ingredients.push(ingredient);
      }
      else {
        break;
      }
      j++;
    }
  }
  
  isGuestUser(): boolean {
    if (localStorage.getItem('guestToken') === 'true') {
      return true;
    }
    else
    return false;
  }

  updateTranslations(language: string): void {
    console.log(`Aggiornamento traduzioni per la lingua: ${language}`);
    let toChange: boolean = false;
     this.translatedString = [];
    this.originalString.forEach((text, index) => {
      this.translateService.translateText(text, this.user.language).subscribe(
        (response) => {
          this.translatedString[index] = response.translatedText;
          this.cdr.detectChanges();
        },
        (error) => {
          console.error(`Errore durante la traduzione di "${text}":`, error);
        }
      );
    });

    if (this.cocktail) {
      const cocktailElements = [
        { key: 'strAlcoholic', value: this.cocktail.strAlcoholic },
        { key: 'strInstructions', value: this.cocktail.strInstructions },
        { key: 'strInstructionsZH_HANS', value: this.cocktail.strInstructionsZH_HANS }
      ];
  
      cocktailElements.forEach((element) => {
        if (element.value) {
          this.translateService.translateText(element.value, this.user.language).subscribe(
            (response) => {
              if (element.key == 'strInstructions') {
                switch (this.user.language)
                {
                  case 'it':
                    if (this.cocktail!.strInstructionsIT && this.cocktail!.strInstructionsIT.trim() !== '') {
                      this.cocktail!.strInstructions = this.cocktail!.strInstructionsIT;
                      toChange = true;
                    }
                  break;
                  case 'es':
                    if (this.cocktail!.strInstructionsES && this.cocktail!.strInstructionsES.trim() !== '') {
                      this.cocktail!.strInstructions = this.cocktail!.strInstructionsES;
                      toChange = true;
                    }
                    break;
                  case 'fr':
                    if (this.cocktail!.strInstructionsFR && this.cocktail!.strInstructionsFR.trim() !== '') {
                      this.cocktail!.strInstructions = this.cocktail!.strInstructionsFR;
                      toChange = true;
                    }
                    break;
                  case 'de':
                    if (this.cocktail!.strInstructionsDE && this.cocktail!.strInstructionsDE.trim() !== '') {
                      this.cocktail!.strInstructions = this.cocktail!.strInstructionsDE;
                      toChange = true;
                    }
                    break;
                  default:
                    break;
                }
              }
              if (!toChange)
               (this.cocktail as any)[element.key] = response.translatedText;
              this.cdr.detectChanges();
            },
            (error) => {
              console.error(`Errore durante la traduzione di "${element.value}":`, error);
            }
          );
        }
      });

    if (this.ingredients.length > 0) {
      const translatedIngredients: string[] = [];
      this.ingredients.forEach((ingredient, index) => {
        this.translateService.translateText(ingredient, language).subscribe(
          (response) => {
            translatedIngredients[index] = response.translatedText;
            console.log(`Tradotto ingrediente "${ingredient}" in "${response.translatedText}"`);
            if (translatedIngredients.length === this.ingredients.length) {
              this.ingredients = translatedIngredients; // Aggiorna l'array degli ingredienti
              this.cdr.detectChanges(); // Forza il change detection
            }
          },
          (error) => {
            console.error(`Errore durante la traduzione dell'ingrediente "${ingredient}":`, error);
          }
        );
      });
    }
  }
}

isMyCocktail(id: string): boolean {

  if (!id)
    return false;
  if(this.user.createdCocktails.includes( Number(id)))
    return true;
  return false;
}

  ngOnDestroy(): void {
    if (this.languageChangeSubscription) {
      this.languageChangeSubscription.unsubscribe();
    }
  }
}
