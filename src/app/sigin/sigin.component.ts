import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-sigin',
  imports: [FormsModule],
  providers: [],
  template: `
    <h2>Sign In</h2>
    <br>
    <form (ngSubmit)="submitForm()">
      <p>Nome*: <input type="text" [(ngModel)]="user.name" name="name" required /></p>
      <p>Username*: <input type="text" [(ngModel)]="user.username" name="username" required></p>
      <p>Password*: <input type="password" [(ngModel)]="user.password" name="password" required></p>
      <p>Email*: <input type="email" [(ngModel)]="user.email" name="email" required></p>
      <p>
        Data di nascita*:  
        <input type="date" [(ngModel)]="user.birthDate" name="birthDate" (change)="calculateAge()" required />
      </p>
      <p>Indirizzo: <input type="text" [(ngModel)]="user.address" name="address"></p>
      <p>Città: <input type="text" [(ngModel)]="user.city" name="city"></p>
      
      <label for="imgUpload">Upload Image:</label>
      <input type="file" id="imgUpload" (change)="onFileSelected($event)" accept="image/*" />
      
      <img *ngIf="user.imgUrl" [src]="user.imgUrl" alt="Uploaded Image" />
      
      <p><input type="checkbox" [(ngModel)]="user.accept" name="accept"> Accetta Termini</p>
      <p><input type="checkbox" [(ngModel)]="user.astemi" name="astemi"> Modalità senza alcool</p>
      
      <button type="submit" [disabled]="!user.name || !user.username || !user.password || !user.email || !user.birthDate">Invia</button>
    </form>

    <p *ngIf="message">{{ message }}</p>
    `,
  styles: [``]

})
export class SiginComponent {
  
  user: User = new User();
  birthDate: string = '';
  age: number | null = null;
  message: string = '';

  constructor(private userService: UserService) {}

  calculateAge() {
    if (this.birthDate) {
      const today = new Date();
      const birthDateObj = new Date(this.birthDate);
      let age = today.getFullYear() - birthDateObj.getFullYear();
      
      // Controlla se il compleanno è già passato quest'anno
      if (
        today.getMonth() < birthDateObj.getMonth() ||
        (today.getMonth() === birthDateObj.getMonth() && today.getDate() < birthDateObj.getDate())
      ) {
        age--;
      }
      
      this.age = age;
    }
  }
  submitForm() {
    this.userService.registerUser(this.user).subscribe(
      (response: any) => {  // Tipizzazione esplicita di 'response'
        this.message = response.message;
        console.log('User registered:', response);
      },
      (error: HttpErrorResponse) => {  // Tipizzazione esplicita di 'error'
        this.message = "Errore durante la registrazione!";
        console.error('Error:', error);
      }
    );
  }
  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
      const file = fileInput.files[0];
      const reader = new FileReader();

      reader.onload = (e: ProgressEvent<FileReader>) => {
        this.user.imgUrl = e.target?.result;  // Salva l'URL dell'immagine nel modello
      };

      reader.readAsDataURL(file); // Legge il file come URL base64
    }
  }
  
}