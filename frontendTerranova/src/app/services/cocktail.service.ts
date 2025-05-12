import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CocktailApiDrink } from '../model/cocktail';

@Injectable({
  providedIn: 'root'
})
export class CocktailService {
  private apiUrl = `${environment.baseUrl}Cocktail`;
  constructor(private http: HttpClient) { }

  takeCocktailOfDay(num: number , alcool: boolean): Observable<any> {
    const url = `${this.apiUrl}/RandomCocktails?number=${num}&alcohol=${alcool}`;
    console.log('URL chiamato:', url);
    return this.http.get<any>(url);
  }

  takeCocktailById(id: string): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }
  searchCocktailByName(name: string): Observable<any> {
    const url = `${this.apiUrl}/Search?str=${name}`;
    return this.http.get<any>(url);
  }

  setFavorite(request: { Username: string; CocktailId: string }) {
    const url = `${this.apiUrl}/favorite`;
    return this.http.patch(url, request);
  }

  getDrinkName(drinkName: string): Observable<any> {
    const url = `${this.apiUrl}/CheckCocktailName?name=${drinkName}`;
    return this.http.get<{ Exists: boolean }>(url);
  }

  createNewCocktail(newdrink: CocktailApiDrink, name:string, prv:  boolean): Observable<any>{
    const url = `${this.apiUrl}`;
    const request = {
      drink: newdrink,
      username: name,
      private: prv,
      instructions: newdrink.strInstructionsZH_HANS,
    }
    return this.http.post<any>(url, request);
  }

  deleteCocktail(id: string): Observable<any> {
    console.log('cocktailId', id);
    const url = `${this.apiUrl}/DeleteCocktail?id=${id}`;
    return this.http.delete<any>(url);
  }

}
