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

  takeCocktailOfDay(): Observable<any> {
    const url = `${this.apiUrl}/RandomCocktails`;
    return this.http.get<any>(url);
  }

}
