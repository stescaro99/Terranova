import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../user/user.model';


@Injectable({
  providedIn: 'root'
})
  export class UserService {
    private currentUser: User = new User();
    private apiUrl = `${environment.baseUrl}user`;
    constructor(private http: HttpClient) {}

    getUser(): User {
      return this.currentUser;
    }
    
    setUser(user: User): void {
      this.currentUser = user;
    }
    createUser(user: User): Observable<User> {
      const url = `${this.apiUrl}`;
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
