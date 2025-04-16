import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { CocktailService } from '../services/cocktail.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search',
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})

export class SearchComponent implements OnInit {
  searchControl = new FormControl('');
  searchResults: { strDrink: string; idDrink: string }[] = [];
  selectedCocktail: { strDrink: string; idDrink: string } | null = null;

  constructor(private cocktailService: CocktailService, private router: Router) {}

  ngOnInit() {
    this.searchControl.valueChanges.pipe(
    ).subscribe((value) => {
      const searchValue = value ?? '';
      console.log('Valore di ricerca:', searchValue);
      if (searchValue.trim() !== '') {
        this.cocktailService.searchCocktailByName(searchValue).subscribe(
          (response) => {
            this.searchResults = [];
            console.log('Risultato della ricerca:', response);
            for (let i = 0; i < response.length; i++)
            {
              if (response[i].drink) {
                this.searchResults.push(response[i].drink);
              }
            }
          },
          (error) => {
            console.error('Errore durante la ricerca:', error);
            this.searchResults = [];
          }
        );
      } else {
        this.searchResults = []; 
      }
    });
  }

  onCocktailSelect(cocktail: { strDrink: string; idDrink: string }) {
    console.log('Cocktail selezionato:', cocktail);
    this.router.navigate(['/cocktail', cocktail.idDrink]);
    this.searchResults = []; 
  }
}