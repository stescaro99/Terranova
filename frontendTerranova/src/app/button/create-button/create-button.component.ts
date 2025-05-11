import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-button',
  imports: [],
  templateUrl: './create-button.component.html',
  styleUrl: './create-button.component.css'
})
export class CreateButtonComponent {
	@Input() text: string = '';
	@Input() cocktailId: string = '';
	constructor(private router: Router) {}
	click(){
		if (this.cocktailId !== ''){
			this.router.navigate(['/cocktail-create'], { queryParams: { id: this.cocktailId } })
		}
    this.router.navigate(['/cocktail-create']);
  }
}
