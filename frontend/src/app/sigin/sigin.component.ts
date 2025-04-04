import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CountryComponent } from '../country/country.component';
import { AgeComponent } from '../age/age.component';

@Component({
  selector: 'app-sigin',
  imports: [FormsModule, CommonModule, CountryComponent, AgeComponent],
  standalone: true,
  providers: [],
  template: `
    <h2>Sign In</h2>
    <br>
    <form (ngSubmit)="submitForm()">
      <p>Nome*: <input type="text" [(ngModel)]="user.Name" name="name" required /></p>
      <p>
        Username*: 
        <input 
          type="text" 
          [(ngModel)]="user.Username" 
          name="username" 
          required 
          (blur)="checkUsername()" 
        />
        <span *ngIf="usernameMessage" [style.color]="usernameAvailable ? 'green' : 'red'">
          {{ usernameMessage }}
        </span>
      </p>
      <p>Password*: <input type="password" [(ngModel)]="user.Password" name="password" required></p>
      <p>
        Email*: 
       <input 
          type="email" 
          placeholder="Inserisci la tua email" 
          [(ngModel)]="user.Email" 
          name="email" 
          required 
          (input)="validateEmail()" 
          #emailInput="ngModel" 
        />
        <span *ngIf="emailInput.invalid && emailInput.touched" style="color: red;">
          Inserisci un'email valida.
        </span>
        <span *ngIf="emailErrorMessage" style="color: red;">
          {{ emailErrorMessage }}
        </span>
      </p>
      <app-age (DateSelected)="user.BirthDate = $event"></app-age>
      <app-country (countrySelected)="user.Country = $event"></app-country>
      <p>country selected: {{ user.Country }}</p>
      <p>Città: <input type="text" [(ngModel)]="user.City" name="city"></p>
      
      <label for="imgUpload">Upload Image:</label>
      <input type="file" id="imgUpload" (change)="onFileSelected($event)" accept="image/*" />
      
      <img *ngIf="user.ImgUrl" [src]="user.ImgUrl" alt="Uploaded Image" />
      
      <p><input type="checkbox" [(ngModel)]="user.AppPermissions" name="accept"> Accetta Termini</p>
      <p><input type="checkbox" [(ngModel)]="user.CanDrinkAlcohol" name="astemi"> Modalità senza alcool</p>
      
      <button type="submit" [disabled]="!formValid()">Invia</button>
    </form>
    `,
  styles: [``]

})
export class SiginComponent {
  
  user: User = new User();
  birthDate: string = '';
  age: number | null = null;
  message: string = '';
  usernameMessage: string = '';
  emailErrorMessage: string = '';
  usernameAvailable: boolean = false;
   
  constructor(private userService: UserService, private router: Router) {
  }

  
  checkUsername() {
      if (!this.user.Username) {
        this.usernameMessage = 'Username non può essere vuoto.';
        this.usernameAvailable = false;
        return;
      }

      this.userService.getUserByUsername(this.user.Username).subscribe(
        (isAvailable: boolean) => {
          if (isAvailable) {
            this.usernameMessage = `L'username "${this.user.Username}" è disponibile.`;
            this.usernameAvailable = true;
          } else {
            this.usernameMessage = `L'username "${this.user.Username}" è già utilizzato.`;
            this.usernameAvailable = false;
          }
        },
        (error: HttpErrorResponse) => {
          console.error('Errore durante il controllo dell\'username:', error);
          this.usernameMessage = 'Errore durante il controllo dell\'username.';
          this.usernameAvailable = false;
        }
      );
  }
  validateEmail() {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Regex per validare l'email
    if (!this.user.Email) {
      this.emailErrorMessage = 'Il campo email è obbligatorio.';
    } else if (!emailRegex.test(this.user.Email)) {
      this.emailErrorMessage = 'Inserisci un\'email valida (esempio@email.com).';
    } else {
      this.emailErrorMessage = ''; // Nessun errore
    }
  }

  submitForm() {
      if (this.usernameAvailable) {
          this.userService.createUser(this.user).subscribe(
              (response: User) => {
                  console.log('Utente creato con successo:', response);
                  alert('Utente creato con successo!');
                  sessionStorage.setItem('username', this.user.Username);
                  sessionStorage.setItem('authToken', 'true');
                  sessionStorage.setItem('user', JSON.stringify(this.user));
                  this.userService.setUser(this.user);
                  this.router.navigate(['/home']);
              },
              (error: HttpErrorResponse) => {
                  console.error('Errore durante la creazione dell\'utente:', error);
                  alert('Errore durante la creazione dell\'utente.');
              }
          );
      } else {
          alert('L\'username non è disponibile.');
      }
  }

  formValid(): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    console.log(this.user);
    return (
      !!this.user.Name && 
      !!this.user.Username && 
      !!this.user.Password && 
      !!this.user.Email &&
      !!this.user.BirthDate &&
      this.user.AppPermissions && 
      this.usernameAvailable 
    );
  }
  
  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
      const file = fileInput.files[0];
      const reader = new FileReader();

      reader.onload = (e: ProgressEvent<FileReader>) => {
        this.user.ImgUrl = e.target?.result;  // Salva l'URL dell'immagine nel modello
      };

      reader.readAsDataURL(file); // Legge il file come URL base64
    }
  }
  
}