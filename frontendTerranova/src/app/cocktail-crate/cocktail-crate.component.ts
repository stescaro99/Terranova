import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Cocktail, CocktailApiDrink } from '../model/cocktail';
import { Input } from '@angular/core';
import { UserService } from '../services/user.service';
import { CocktailService } from '../services/cocktail.service';
import { User } from '../model/user';


@Component({
  selector: 'app-cocktail-crate',
  imports: [CommonModule],
  templateUrl: './cocktail-crate.component.html',
  styleUrl: './cocktail-crate.component.css'
})
export class CocktailCrateComponent {
	@Input() idCocktail! : string | null ;
	user : User ;
	newcocktail: Cocktail = new Cocktail();
	oldcocktail?: CocktailApiDrink;
	useRealimg: boolean = true;

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
}
