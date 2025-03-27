import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../user';
import { HttpClient } from '@angular/common/http'

@Component({
  selector: 'app-sigin',
  imports: [FormsModule, HttpClient],
  template: `
    <h2>Sign In</h2>
    <br>
    <form (ngSubmit)="submitForm()">
     <p>name*:    <input type="text" [(ngModel)]="user.name"  name="name"/></p>
     <p>Username*: <input type="text" [(ngModel)]="user.username" name="username"></p>
      <p>Password*: <input type="text" [(ngModel)]="user.password" name="password"></p>
      <p>Email*: <input type="email" [(ngModel)]="user.email" name="email"></p>
     <p>
       Data di nascita*:  
       <input type="date" [(ngModel)]="birthDate"  [(ngModel)]="user.birthDate" name="birthDate" (change)="calculateAge()" />
     </p>
      <p>Address: <input type="text" [(ngModel)]="user.address" name="address"></p>
      <p>City: <input type="text" [(ngModel)]="user.city" name="city"></p>
      <p><input type="checkbox" [(ngModel)]="user.accept" name="accept"> Accept Terms</p>
      <p><input type="checkbox" [(ngModel)]="user.astemi" name="astemi"> modalita senza alcool</p>
      <label for="imgUpload">Upload Image:</label>
      <input type="file" id="imgUpload" (change)="onFileSelected($event)" accept="image/*" />

      <img *ngIf="user.imgUrl" [src]="user.imgUrl" alt="Uploaded Image" />

      <button type="submit" [disabled]="!user.name || !user.username || !user.password || !user.email || !user.birthDate">Submit</button>
    </form>
    `,
  styles: [``]

})
export class SiginComponent {
  user: User = {
    name: '',
    email: '',
    password: '',
    birthDate: '',
    country: '',
    city: '',
    canDrinkAlcohol: false,
    appPermissions: false,
    imageUrl: '',
    favoriteCocktails: [],
    createdCocktails: []
  };
  birthDate: string = '';
  age: number | null = null;

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
    console.log(this.user);  // Gestisci la logica del form qui
  }
  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
      const file = fileInput.files[0];
      const reader = new FileReader();

      reader.onload = (e: ProgressEvent<FileReader>) => {
        this.user.imageUrl = e.target?.result;  // Salva l'URL dell'immagine nel modello
      };

      reader.readAsDataURL(file); // Legge il file come URL base64
    }
  }
}