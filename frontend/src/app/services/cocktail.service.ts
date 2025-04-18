import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CocktailService {
  private apiUrl = `${environment.baseUrl}cocktail`;
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

}
