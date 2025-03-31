import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common'; 


@Component({
  selector: 'app-sigin',
  imports: [FormsModule, CommonModule],
  providers: [],
  template: `
    <h2>Sign In</h2>
    <br>
    <form (ngSubmit)="submitForm()">
      <p>Nome*: <input type="text" [(ngModel)]="user.name" name="name" required /></p>
      <p>
        Username*: 
        <input 
          type="text" 
          [(ngModel)]="user.username" 
          name="username" 
          required 
          (blur)="checkUsername()" 
        />
        <span *ngIf="usernameMessage" [style.color]="usernameAvailable ? 'green' : 'red'">
          {{ usernameMessage }}
        </span>
      </p>
      <p>Password*: <input type="password" [(ngModel)]="user.password" name="password" required></p>
      <p>Email*: <input type="email" [(ngModel)]="user.email" name="email" required></p>
      <p>
        Data di nascita*:  
        <input type="date" [(ngModel)]="user.birthDate" name="birthDate" (change)="calculateAge()" required />
      </p>
      <p>Nazione: <input type="text" [(ngModel)]="user.address" name="address"></p>
      <p>Città: <input type="text" [(ngModel)]="user.city" name="city"></p>
      
      <label for="imgUpload">Upload Image:</label>
      <input type="file" id="imgUpload" (change)="onFileSelected($event)" accept="image/*" />
      
      <img *ngIf="user.imgUrl" [src]="user.imgUrl" alt="Uploaded Image" />
      
      <p><input type="checkbox" [(ngModel)]="user.accept" name="accept"> Accetta Termini</p>
      <p><input type="checkbox" [(ngModel)]="user.astemi" name="astemi"> Modalità senza alcool</p>
      
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
  usernameAvailable: boolean = false;

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

  checkUsername() {
    if (!this.user.username) {
      this.usernameMessage = 'Username non può essere vuoto.';
      this.usernameAvailable = false;
      return;
    }

    this.userService.getUserByUsername(this.user.username).subscribe(
      (response: any) => {
        this.usernameMessage = `L'username "${this.user.username}" è già utilizzato.`;
        this.usernameAvailable = false;
      },
      (error: HttpErrorResponse) => {
        if (error.status === 404) {
          this.usernameMessage = `L'username "${this.user.username}" è disponibile.`;
          this.usernameAvailable = true;
        } else {
          this.usernameMessage = 'Errore durante la verifica dell\'username.';
          this.usernameAvailable = false;
        }
      }
    );
  }

  submitForm() {
    if (this.usernameAvailable) {
      this.userService.createUser(this.user).subscribe(
        (response: User) => {
          console.log('Utente creato con successo:', response);
          alert('Utente creato con successo!');
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
    return (
      !!this.user.name && // Verifica che sia una stringa non vuota
      !!this.user.username && // Verifica che sia una stringa non vuota
      !!this.user.password && // Verifica che sia una stringa non vuota
      !!this.user.email && // Verifica che sia una stringa non vuota
      !!this.user.birthDate && // Verifica che sia una stringa non vuota
      this.user.accept && // Deve essere true
      this.usernameAvailable // Deve essere true
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