import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-button',
  imports: [],
  templateUrl: './create-button.component.html',
  styleUrl: './create-button.component.css'
})
export class CreateButtonComponent {
  constructor(private router: Router) {}
  click(){
    this.router.navigate(['/cocktail-create']);
  }
}
