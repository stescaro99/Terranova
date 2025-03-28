import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../user/user.model';

@Injectable({
  providedIn: 'root'
})
  export class UserService {

    private apiUrl = `${environment.baseUrl}users`; // URL per il backend
  
    constructor(private http: HttpClient) {}
  
    // Funzione per inviare i dati del form al backend
    createUser(user: User): Observable<User> {
      return this.http.post<User>(this.apiUrl, user);
    }
    registerUser(user: User): Observable<any> {
      return this.http.post<any>(this.apiUrl, user);
    }
  }
