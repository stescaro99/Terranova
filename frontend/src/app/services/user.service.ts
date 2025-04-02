import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../user/user.model';

@Injectable({
  providedIn: 'root'
})
  export class UserService {

    private apiUrl = `${environment.baseUrl}user`; // URL per il backend
    constructor(private http: HttpClient) {}
  
    // Funzione per inviare i dati del form al backend
    createUser(user: User): Observable<User> {
      const url = `${this.apiUrl}`; // Usa l'endpoint base per creare un utente
      return this.http.post<User>(url, user);
    }
    registerUser(user: User): Observable<any> {
      return this.http.post<any>(this.apiUrl, user);
    }
    getUserByUsername(username: string): Observable<any> {
      const url = `${this.apiUrl}/CheckUsername?username=${encodeURIComponent(username)}`;
      console.log('URL chiamato:', url)
      return this.http.get<any>(url);
    }
  }
