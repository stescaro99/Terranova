import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class TranslateService {

  url = 'http://localhost:5000';
  constructor(private http: HttpClient) { }

  translateText(text: string, targetLanguage: string): Observable<any> {
    const body = {
      q: text,
      source: "auto",
      target: targetLanguage,
      format: "text"
    };
    return this.http.post(`${this.url}/translate`, body, {
      headers: { "Content-Type": "application/json" }
    });
  }
}
