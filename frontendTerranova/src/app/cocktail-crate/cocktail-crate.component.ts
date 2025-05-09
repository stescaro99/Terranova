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
	img: string | ArrayBuffer | null | undefined = null;
	usedefaultimg: boolean = true;
	imgPreview: string | null = null;
	quantity: string[] = Array(this.numberOfIngredients).fill('to taste');
	drinknameMessage: string = '';
	drinknameAvailable: boolean = true;
	glasses: string[] = glasses;
	categories: string[] = categories;

	constructor(private userservice: UserService, private cocktailservice: CocktailService) {
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
			const element = document.querySelector('.glass-mask') as HTMLElement;
			if (element) {
			html2canvas(element).then(canvas => {
				canvas.toBlob(blob => {
				if (blob) {
					const formData = new FormData();
					formData.append('Image', blob, 'image.png'); // Aggiungi il blob al FormData
		
					const request = {
					FileName: `${this.user.username}_generated.png`, // Nome file generato
					Image: formData
					};
		
					this.userservice.uploadFile(request).subscribe(
					(response: { imageUrl: string }) => {
						this.newcocktail.drink.strDrinkThumb = response.imageUrl; // Salva l'URL restituito
						console.log('URL dell\'immagine:', this.newcocktail.drink.strDrinkThumb);
						resolve();
					},
					(error) => {
						console.error('Errore durante il caricamento dell\'immagine:', error);
						reject(error);
					}
					);
				} else {
					reject('Errore durante la conversione in Blob.');
				}
				}, 'image/png');
			}).catch(error => {
				console.error('Errore durante la cattura dell\'immagine:', error);
				reject(error);
			});
			} else {
			console.error('Elemento non trovato per la cattura.');
			reject('Elemento non trovato');
			}
		});
	}

	onFileSelected(event: Event): void {
		const input = event.target as HTMLInputElement;
	
		if (input.files && input.files.length > 0) {
			const file = input.files[0];
	

			this.imgPreview = URL.createObjectURL(file);
	
			const formData = new FormData();
			formData.append('FileName', file.name); 
			formData.append('Image', file);
	

			const request = {
				FileName: file.name,
				Image: formData
			};
	
			console.log('Richiesta di caricamento:', request); // Aggiungi questa riga per il debug
			this.userservice.uploadFile(request).subscribe(
				(response: any) => {
					// Assegna l'URL restituito dal backend a drink.strDrinkThumb
					this.newcocktail.drink.strDrinkThumb = response.imageUrl;
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
	  
		console.log('Cocktail da salvare:', this.newcocktail); // Ora viene eseguito dopo
	  }
}
