import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Cocktail, CocktailApiDrink } from '../model/cocktail';
import { Input } from '@angular/core';
import { UserService } from '../services/user.service';
import { CocktailService } from '../services/cocktail.service';
import { User } from '../model/user';
import { IngredientListComponent } from '../search/ingredient-list/ingredient-list.component';
import { ingredientColors, glasses, categories } from '../model/ingredient';
import { BackgroundComponent } from '../background/background.component';
import  html2canvas  from 'html2canvas'
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { TranslateService } from '../services/translate.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-cocktail-crate',
  imports: [CommonModule, IngredientListComponent, BackgroundComponent, FormsModule],
  templateUrl: './cocktail-crate.component.html',
  styleUrl: './cocktail-crate.component.css'
})
export class CocktailCrateComponent {
	user : User ;
	idCocktail: string = '';
	newcocktail: Cocktail = new Cocktail();
	oldcocktail?: CocktailApiDrink;
	useRealimg: boolean = true;
	ingredients: string[] = [];
	ingredientColors: { [key: string]: string } = {};
	numberOfIngredients: number = 1;
	mode: 'mix' | 'shake' = 'mix';
	open: boolean = true;
	img: File | null = null;
	usedefaultimg: boolean = true;
	imgPreview: string | null = null;
	quantity: string[] = [];
	drinknameMessage: string = '';
	drinknameAvailable: boolean = true;
	glasses: string[] = glasses;
	categories: string[] = categories;
	originalText: string[] = [
		'public',
		'private',
		'automatic image generation',
		'Upload cocktail image',
		'Mix',
		'Shake',
		'Click to upload',
		'Select the ingredients:',
		'Add ingredient',
		'Drink Name:',
		'The drink name cannot be empty.',
		'The drink name is already in use.',
		'The drink name is available.',
		'Error during name verification. Please try again later.',
		'Category:',
		'Alcoholic',
		'Non alcoholic',
		'Optional alcohol',
		'Glass type:',
		'Instructions:',
		'Save cocktail'
	];
	translatedText: string[] = [];
	private languageChangeSubscription!: Subscription

	constructor(
		private userservice: UserService,
		private cocktailservice: CocktailService,
		private router: Router,
		private route: ActivatedRoute,
		private cdr: ChangeDetectorRef,
		private translateService: TranslateService,
	) {
		this.user = userservice.getUser() || new User();
		this.idCocktail = this.route.snapshot.paramMap.get('id') || '';
		if (this.idCocktail) {
		  this.cocktailservice.takeCocktailById(this.idCocktail).subscribe(
			(cocktail: Cocktail) => {
			  this.oldcocktail = cocktail.drink;
				for (let i = 1; i <= 15; i++) {
					const ingredient = this.oldcocktail[`strIngredient${i}` as keyof CocktailApiDrink];
					const measure = this.oldcocktail[`strMeasure${i}` as keyof CocktailApiDrink];
					
					if (ingredient && ingredient.trim() !== '') {
						this.ingredients.push(ingredient.trim());
						this.quantity.push(measure?.trim() || '');
						const color = ingredientColors[ingredient.trim()] || '#000000';
   						this.ingredientColors[ingredient.trim()] = color;
					}
				}
				this.newcocktail.drink.strAlcoholic = cocktail.drink.strAlcoholic ;
				this.newcocktail.drink.strDrink = cocktail.drink.strDrink + ' by ' + this.user.username;
				this.newcocktail.drink.strGlass = cocktail.drink.strGlass;
				this.newcocktail.drink.strCategory = cocktail.drink.strCategory;
				this.numberOfIngredients = this.ingredients.length;
				this.newcocktail.drink.strDrinkThumb = cocktail.drink.strDrinkThumb;
				this.imgPreview = cocktail.drink.strDrinkThumb ?? null;

				this.cdr.detectChanges();
				console.log('saaa', this.ingredients);
			},
			(error) => {
			  console.error('error to find cocktail id');
			}
		  );
		}else{
			this.quantity = Array(this.numberOfIngredients).fill('to taste');
		}
	}
	ngOnInit(): void {
		
		const currentLanguage = this.user.language || 'en';
		this.updateTranslations(currentLanguage);
		this.languageChangeSubscription = this.userservice.getLanguageChangeObservable().subscribe((language) => {
			this.updateTranslations(language);
		});
	}
	setMode(mode: 'mix' | 'shake'): void {
		this.mode = mode;
	}

	setOpen(open: boolean): void {
		this.open = !open;
		console.log('open:', this.open);
	}

	setDefaultImg(usedefaultimg: boolean): void {
		this.usedefaultimg = !usedefaultimg;
	}

	checkDrinkName(name: string | undefined): void {
		if (!name || name.trim() === '') {
		this.drinknameMessage = this.translatedText[10] || this.originalText[10];
		  this.drinknameAvailable = false;
		  return;
		}
	  
		this.cocktailservice.getDrinkName(name).subscribe(
		  (response: any) => {
			console.log('Risposta dal server:', response); 
			if (response.exists) { 
			  this.drinknameMessage = this.translatedText[11] || this.originalText[11];
			  this.drinknameAvailable = false;
			} else {
			  this.drinknameMessage = this.translatedText[12] || this.originalText[12];
			  this.drinknameAvailable = true;
			}
		  },
		  (error) => {
			console.error('Errore durante la verifica del nome del drink:', error);
			this.drinknameMessage = this.translatedText[13] || this.originalText[13];
			this.drinknameAvailable = false;
		  }
		);
	  }

	  updateTranslations(language: string): void {
		if (language !== 'en') {
			this.translatedText = [];
			this.originalText.forEach((text, index) => {
				this.translateService.translateText(text, language).subscribe(
					(response: any) => {
						this.translatedText[index] = response.translatedText;
						this.cdr.detectChanges(); // Forza il change detection
					},
					(error) => {
						console.error('Errore durante la traduzione:', error);
						this.translatedText[index] = text;
					}
				);
			});
		} else {
			this.translatedText = [...this.originalText];
		}
	}

	  onBlurQuantity(index: number): void {
		if (this.quantity[index].trim() === '' || this.quantity[index].trim().toLowerCase() === 'to taste' || this.quantity[index].trim().toLowerCase() === '0') {
		  this.quantity[index] = 'to taste';
		}
		else {
		  this.quantity[index] = this.quantity[index].trim(); 
		}
	  }

	onIngredientSelected(event: { name: string; color: string }, index: number): void {
		const { name, color } = event;
		if (this.ingredients[index]) {
		  const previousIngredient = this.ingredients[index];
		  console.log(`Rimuovendo ingrediente precedente: ${previousIngredient}`);
		  delete this.ingredientColors[previousIngredient];
		}
	  
		this.ingredients[index] = name;
		this.ingredientColors[name] = color;
	  
		console.log('Ingredienti aggiornati:', this.ingredients);
		console.log('Colori aggiornati:', this.ingredientColors);
	  }
	
	getGradient(): string {
		if (this.ingredients.length === 0) {
			return '#000000';
		}
		
		const colors = this.ingredients
			.map(ingredient => this.ingredientColors[ingredient] || '#000000')
			.filter(color => color); 
		
		if (colors.length === 0) {
			return '#000000';
		}
		

		return `linear-gradient(to bottom, ${colors.join(', ')})`;
	}
	getMixedColor(): string {
		const colors = this.ingredients.map(ingredient => this.ingredientColors[ingredient] || 'rgba(0, 0, 0, 1)');
		console.log('Colori degli ingredienti:', colors);
	  
		if (colors.length === 0) {
		  return 'rgba(0, 0, 0, 1)';
		}
	  
		let r = 0, g = 0, b = 0, a = 0;
	  
		colors.forEach(color => {
		  const rgba = this.rgbaToComponents(color);
		  if (rgba) {
			r += rgba.r;
			g += rgba.g;
			b += rgba.b;
			a += rgba.a;
		  }
		});
	  

		r = Math.round(r / colors.length);
		g = Math.round(g / colors.length);
		b = Math.round(b / colors.length);
		a = Math.round((a / colors.length) * 100) / 100;
	  

		return `rgba(${r}, ${g}, ${b}, ${a})`;
	  }
	
	  addIngredient(): void {
		if (this.numberOfIngredients < 15) {
		  this.numberOfIngredients++;
		  this.quantity.push('to taste');
		  this.ingredients.push('');
		} else {
		  console.log('Hai raggiunto il numero massimo di ingredienti.');
		}
	  }

	  removeIngredient(index: number): void{
		if (index <= this.numberOfIngredients){
			this.numberOfIngredients--;
			this.ingredients.splice(index, 1);
			this.quantity.splice(index, 1);
		}

	  }

	  isLastIngredientValid(): boolean {
		if (this.numberOfIngredients == 0)
			return true;
		if (this.numberOfIngredients === 1 && this.ingredients.length === 0) {
		  return false;
		}
		if (this.ingredients.length !== 0){
			for (let i = 0; i < this.ingredients.length; i++) {
				if (this.ingredients[i] === '') {
					return false; 
				}
			}
		}
	  
		const lastIndex = this.numberOfIngredients - 1;
		const lastQuantity = this.quantity[lastIndex]?.trim();
	  
		return typeof lastQuantity === 'string' && (lastQuantity.toLowerCase() === 'to taste' || lastQuantity !== '');
	  }

	  isValid(): boolean {
		if (this.newcocktail.drink.strDrink && this.newcocktail.drink.strDrink.trim() !== '' && this.newcocktail.drink.strDrink.length < 50) {
			if (this.newcocktail.drink.strInstructionsZH_HANS && this.newcocktail.drink.strInstructionsZH_HANS.trim() !== '' && this.newcocktail.drink.strInstructionsZH_HANS.length < 500) {
				if (this.isLastIngredientValid()&& this.ingredients.length > 0 && this.drinknameAvailable && this.newcocktail.drink.strGlass && this.newcocktail.drink.strGlass.trim() !== '' && this.newcocktail.drink.strAlcoholic
					&& this.newcocktail.drink.strAlcoholic.trim() !== '' && this.newcocktail.drink.strCategory && this.newcocktail.drink.strCategory.trim() !== '') {
				  return true; 
				}
			}
		}
		return false; 

	  }

	rgbaToComponents(rgba: string): { r: number; g: number; b: number; a: number } | null {
		const match = rgba.match(/^rgba?\((\d+),\s*(\d+),\s*(\d+),?\s*([\d.]+)?\)$/);
		if (!match) {
		  console.log(`Colore non valido: ${rgba}`);
		  return null;
		}
		return {
		  r: parseInt(match[1], 10),
		  g: parseInt(match[2], 10),
		  b: parseInt(match[3], 10),
		  a: match[4] ? parseFloat(match[4]) : 1 
		};
	  }

	captureImage(): Promise<void> {
		return new Promise((resolve, reject) => {
			const element = document.querySelector('.color-display') as HTMLElement;

			if (!element) {
			console.error('Elemento non trovato per la cattura.');
			return reject('Elemento non trovato');
			}

			html2canvas(element, { backgroundColor: null }).then((canvas: HTMLCanvasElement) => {
			canvas.toBlob((blob: Blob | null) => {
				if (!blob) {
				return reject('Errore durante la conversione in Blob.');
				}

				const fileName = `${this.newcocktail.drink.strDrink}_generated.png`;
				const file = new File([blob], fileName, { type: 'image/png' });

				this.userservice.uploadFile(file.name, file).subscribe({
				next: (response) => {
					this.newcocktail.drink.strDrinkThumb = response.imageUrl;
					console.log('URL immagine generata:', response.imageUrl);
					resolve();
				},
				error: (err) => {
					console.error('Errore durante l\'upload:', err);
					reject(err);
				}
				});
			}, 'image/png');
			}).catch((err: unknown) => {
			console.error('Errore durante la cattura dell\'immagine:', err);
			reject(err);
			});
		});
	}

	onFileSelected(event: Event): void {
		const input = event.target as HTMLInputElement;
		if (input.files && input.files.length > 0) {
			this.img = input.files[0];
		}
		if (this.img !== null)
		{
			const imgName = this.img.name;
			this.userservice.uploadFile(imgName, this.img).subscribe(
				(response: any) => {
					this.newcocktail.drink.strDrinkThumb = response.imageUrl;
					this.imgPreview = response.imageUrl;
					console.log('URL immagine salvato:', this.newcocktail.drink.strDrinkThumb);
				},
				(error) => {
					console.error('Errore durante il caricamento dell\'immagine:', error);
				}
			);

		}
	}

	  async saveCocktail(): Promise<void> {
		if (this.usedefaultimg) {
		  await this.captureImage(); 
		}
	  
		for (let i = 0; i < this.ingredients.length; i++) {
		  (this.newcocktail.drink as any)[`strIngredient${i + 1}`] = this.ingredients[i];
		  (this.newcocktail.drink as any)[`strMeasure${i + 1}`] = this.quantity[i];
		}
	  
		console.log('Cocktail da salvare:', this.newcocktail); 
		this.newcocktail.drink.idDrink = (Math.random() + 200000).toString();
		console.log('Cocktail da salvare:', this.newcocktail);
		this.cocktailservice.createNewCocktail(this.newcocktail.drink, this.user.username, !this.open).subscribe(
			(response: any) =>{
				console.log('cocktail creato con successo');
				this.router.navigate(['/home']);
			},
			(error) => {
				console.error('errore nella creazione del cocktail', error);
			}
		);
	  }

	ngOnDestroy(): void {
		if (this.languageChangeSubscription) {
			this.languageChangeSubscription.unsubscribe();
		}
	}
}
