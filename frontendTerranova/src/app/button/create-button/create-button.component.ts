import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-button',
  imports: [CommonModule],
  templateUrl: './create-button.component.html',
  styleUrl: './create-button.component.css'
})
export class CreateButtonComponent {
	@Input() text: string = '';
	@Input() cocktailId: string = '';
	constructor(private router: Router) {}
	click() {
	  console.log('cocktailId:', this.cocktailId);
	  if (this.cocktailId && this.cocktailId.trim() !== '') {
	    this.router.navigate(['/cocktail-create', this.cocktailId], { queryParams: { id: this.cocktailId } });
	  } else {
	    this.router.navigate(['/cocktail-create']);
	  }
	}
	isGuestUser(): boolean {
		if (localStorage.getItem('guestToken') === 'true') {
			return true;
		}
		else
		return false;
    }
}
