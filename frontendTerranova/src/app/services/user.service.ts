import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../model/user';
import { request } from 'express';


@Injectable({
  providedIn: 'root'
})
  export class UserService {
    private user: User | null = null;
    private userSubject = new BehaviorSubject<User | null>(null);
    private languageChangeSubject = new BehaviorSubject<string>('en');

    private apiUrl = `${environment.baseUrl}user`;
    constructor(private http: HttpClient) {}

	getUser(): User | null  {
		if (!this.user) {
		const userData = localStorage.getItem('user');
		this.user = userData ? JSON.parse(userData) as User : null;
		}
		return this.user;
	}

	setUser(user: User): void {
		this.userSubject.next(user);
		localStorage.setItem('user', JSON.stringify(user));
		this.languageChangeSubject.next(user.language);
	}
	getLanguageChangeObservable() {
		return this.languageChangeSubject.asObservable();
	}

	isUserValid(): boolean {
		const user = this.getUser();
		return !!user && !!user.username;
	}

	getUserObservable() {
		return this.userSubject.asObservable();
	}

	createUser(user: User): Observable<User> {
		const url = `${this.apiUrl}`;
		console.log('URL chiamato:', url);
		console.log('User:', user);
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
	isUserRegistered(username: string, password: string): Observable<any> {
		const url = `${this.apiUrl}/login?username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`;
		return this.http.get<any>(url);
	}

	getCocktailsFavorite(username: string): Observable<any> {
		const url = `${this.apiUrl}/GetUserFavorites?username=${encodeURIComponent(username)}`;
		console.log('URL chiamato:', url);
		return this.http.get<any>(url);
	}

	getUserbyUsername(username: string): Observable<any> {
		const url = `${this.apiUrl}/byusername?username=${encodeURIComponent(username)}`;
		return this.http.get<any>(url);
	}

	getRecommendedtails(username: string): Observable<any> {
		const url = `${this.apiUrl}/RecommendedCocktails?username=${encodeURIComponent(username)}`;
		return this.http.get<any>(url);
	}

    uploadFile(FileName: string, Image: File): Observable<{ imageUrl: string }> {
		const formData = new FormData;
		formData.append('nome', FileName);
		formData.append('image', Image)
		return this.http.post<{ imageUrl: string }>(`${this.apiUrl}/UploadImage`, formData);
	}

	deleteUser(id: string): Observable<any> {
		const url = `${this.apiUrl}/${id}`;
		return this.http.delete<any>(url);
 	}

	updateUser(userid: string, toChange: string, newcontent: string){
		const url = `${this.apiUrl}/UpdateUser`;
		const payload = {
			username: userid,
			field: toChange,
			value: newcontent
		}
		console.log('params', payload);
		return this.http.put<any>(url, payload);
	}

	getUserCocktails(username: string): Observable<any> {
		const url = `${this.apiUrl}/GetUserCocktails?username=${encodeURIComponent(username)}`;
		return this.http.get<any>(url);
	}

  }
