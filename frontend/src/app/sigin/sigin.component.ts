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
  templateUrl: './sigin.component.html',
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