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
import { image } from 'html2canvas/dist/types/css/types/image';
import { Router } from '@angular/router';
import { response } from 'express';

@Component({
  selector: 'app-cocktail-crate',
  imports: [CommonModule, IngredientListComponent, BackgroundComponent, FormsModule],
  templateUrl: './cocktail-crate.component.html',
  styleUrl: './cocktail-crate.component.css'
})
export class CocktailCrateComponent {
	@Input() idCocktail! : string | null ;
	user : User ;
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
	quantity: string[] = Array(this.numberOfIngredients).fill('to taste');
	drinknameMessage: string = '';
	drinknameAvailable: boolean = true;
	glasses: string[] = glasses;
	categories: string[] = categories;

	constructor(private userservice: UserService, private cocktailservice: CocktailService, private router: Router) {
		this.user = userservice.getUser() || new User();
		if (this.idCocktail) {
		  this.cocktailservice.takeCocktailById(this.idCocktail).subscribe(
			(cocktail: CocktailApiDrink) => {
			  this.oldcocktail = cocktail;
			  for(const object in  this.oldcocktail)
			  {
				if (this.oldcocktail.hasOwnProperty(object))
				{
					if (object === 'strInstructions')
					{
						this.newcocktail.drink.strInstructionsZH_HANS = (this.oldcocktail as any)[object];
						continue;
					}
					(this.newcocktail.drink as any)[object] = (this.oldcocktail as any)[object];
					
				}
			  }
			},
			(error) => {
			  console.error('error to find cocktail id');
			}
		  );
		}
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
		this.drinknameMessage = 'The drink name cannot be empty.';
		  this.drinknameAvailable = false;
		  return;
		}
	  
		this.cocktailservice.getDrinkName(name).subscribe(
		  (response: any) => {
			console.log('Risposta dal server:', response); // Verifica la risposta
			if (response.exists) { // Mappa correttamente la proprietà Exists
			  this.drinknameMessage = 'The drink name is already in use.';
			  this.drinknameAvailable = false;
			} else {
			  this.drinknameMessage = 'The drink name is available.';
			  this.drinknameAvailable = true;
			}
		  },
		  (error) => {
			console.error('Errore durante la verifica del nome del drink:', error);
			this.drinknameMessage = 'Error during name verification. Please try again later.';
			this.drinknameAvailable = false;
		  }
		);
	  }

	  onBlurQuantity(index: number): void {
		// Se il valore è vuoto o "to taste", imposta "to taste"
		if (this.quantity[index].trim() === '' || this.quantity[index].trim().toLowerCase() === 'to taste' || this.quantity[index].trim().toLowerCase() === '0') {
		  this.quantity[index] = 'to taste';
		}
		else {
		  this.quantity[index] = this.quantity[index].trim(); 
		}
	  }

	onIngredientSelected(event: { name: string; color: string }, index: number): void {
		const { name, color } = event;
	  
		// Rimuovi l'ingrediente precedente dallo slot
		if (this.ingredients[index]) {
		  const previousIngredient = this.ingredients[index];
		  console.log(`Rimuovendo ingrediente precedente: ${previousIngredient}`);
		  delete this.ingredientColors[previousIngredient];
		}
	  
		// Aggiorna l'ingrediente nello slot corrente
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
			.filter(color => color); // Filtra eventuali valori nulli o undefined
		
		if (colors.length === 0) {
			return '#000000';
		}
		
		// Crea un gradiente lineare con i colori disponibili
		return `linear-gradient(to bottom, ${colors.join(', ')})`;
	}
	getMixedColor(): string {
		const colors = this.ingredients.map(ingredient => this.ingredientColors[ingredient] || 'rgba(0, 0, 0, 1)');
		console.log('Colori degli ingredienti:', colors);
	  
		if (colors.length === 0) {
		  return 'rgba(0, 0, 0, 1)'; // Colore di fallback
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
	  
		// Calcola la media dei valori RGBA
		r = Math.round(r / colors.length);
		g = Math.round(g / colors.length);
		b = Math.round(b / colors.length);
		a = Math.round((a / colors.length) * 100) / 100; // Mantieni due decimali per l'alpha
	  
		// Restituisci il colore medio in formato RGBA
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

	  isLastIngredientValid(): boolean {
		if (this.numberOfIngredients === 1 && this.ingredients.length === 0) {
		  return false;
		}
		if (this.ingredients.length !== 0){
			for (let i = 0; i < this.ingredients.length; i++) {
				if (this.ingredients[i] === '') {
					return false; // Se c'è un ingrediente vuoto, non è valido
				}
			}
		}
	  
		const lastIndex = this.numberOfIngredients - 1;
		const lastQuantity = this.quantity[lastIndex]?.trim();
	  
		// Controlla che ci sia almeno un ingrediente e che la quantità sia valida
		return typeof lastQuantity === 'string' && (lastQuantity.toLowerCase() === 'to taste' || lastQuantity !== '');
	  }

	  isValid(): boolean {
		if (this.newcocktail.drink.strDrink && this.newcocktail.drink.strDrink.trim() !== '' && this.newcocktail.drink.strDrink.length < 50) {
			if (this.newcocktail.drink.strInstructionsZH_HANS && this.newcocktail.drink.strInstructionsZH_HANS.trim() !== '' && this.newcocktail.drink.strInstructionsZH_HANS.length < 500) {
				if (this.isLastIngredientValid() && this.drinknameAvailable && this.newcocktail.drink.strGlass && this.newcocktail.drink.strGlass.trim() !== '' && this.newcocktail.drink.strAlcoholic
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
		  a: match[4] ? parseFloat(match[4]) : 1 // Alpha predefinito a 1 se non specificato
		};
	  }

	captureImage(): Promise<void> {
		return new Promise((resolve, reject) => {
			const element = document.querySelector('.color-display') as HTMLElement;

			if (!element) {
			console.error('Elemento non trovato per la cattura.');
			return reject('Elemento non trovato');
			}

			html2canvas(element, { backgroundColor: null }).then(canvas => {
			canvas.toBlob(blob => {
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
			}).catch(err => {
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
		  await this.captureImage(); // Aspetta che l'immagine venga catturata
		}
	  
		for (let i = 0; i < this.ingredients.length; i++) {
		  (this.newcocktail.drink as any)[`strIngredient${i + 1}`] = this.ingredients[i];
		  (this.newcocktail.drink as any)[`strMeasure${i + 1}`] = this.quantity[i];
		}
	  
		console.log('Cocktail da salvare:', this.newcocktail); 
		this.newcocktail.drink.idDrink = (Math.random() + 200000).toString();
		this.cocktailservice.createNewCocktail(this.newcocktail.drink, this.user.username, this.open).subscribe(
			(response: any) =>{
				console.log('cocktail creato con successo');
				this.router.navigate(['/home']);
			},
			(error) => {
				console.error('errore nella creazione del cocktail', error);
			}
		);
	  }
}
