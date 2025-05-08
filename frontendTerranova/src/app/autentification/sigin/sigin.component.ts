import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CountryComponent } from '../country/country.component';
import { AgeComponent } from '../age/age.component';
import { UsernameComponent } from '../username/username.component';
import { SlideButtonComponent } from '../../button/slide-button/slide-button.component';
import { BackgroundComponent } from '../../background/background.component';

@Component({
  selector: 'app-sigin',
  imports: [FormsModule, CommonModule, CountryComponent, AgeComponent, UsernameComponent, BackgroundComponent ,SlideButtonComponent],
  standalone: true,
  providers: [],
  templateUrl: './sigin.component.html',
  styleUrl: './sigin.component.css',

})
export class SiginComponent {
  
  user: User = new User();
  birthDate: string = '';
  age: number | null = null;
  message: string = '';
  emailErrorMessage: string = '';
  usernameAvailable: boolean = false;
   
  constructor(private userService: UserService, private router: Router) {
  }

  validateEmail() {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Regex per validare l'email
    if (!this.user.email) {
      this.emailErrorMessage = 'Il campo email è obbligatorio.';
    } else if (!emailRegex.test(this.user.email)) {
      this.emailErrorMessage = 'Inserisci un\'email valida (esempio@email.com).';
    } else {
      this.emailErrorMessage = ''; // Nessun errore
    }
  }

  submitForm() {
      if (this.usernameAvailable) {
        this.user.language = 'en';
        console.log('UtenteaaaaaaaaaaaaaaaAAAAAAAAAAAA :', this.user);

          this.userService.createUser(this.user).subscribe(
              (response: User) => {
                  console.log('Utente creato con successo:', response);
                  alert('Utente creato con successo!');
                  localStorage.setItem('username', this.user.username);
                  localStorage.setItem('authToken', 'true');
                  localStorage.setItem('user', JSON.stringify(this.user));
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
      !!this.user.name && 
      !!this.user.username && 
      !!this.user.password && 
      !!this.user.email &&
      !!this.user.birthDate &&
      this.user.appPermissions && 
      this.usernameAvailable 
    );
  }
  
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.user.imageUrl = URL.createObjectURL(file); // Converte il file in un URL temporaneo
      console.log('URL immagine generato:', this.user.imageUrl);
    }
    const formData = new FormData();
    formData.append('file', this.user.imageUrl as any);
    const request ={
      FileName: File.name,
      Image: formData
    };
    this.userService.uploadFile(request).subscribe(
      (response: any) => {
        console.log('Immagine caricata con successo:', response.imageUrl);
        this.user.imageUrl = response.imageUrl; // Aggiorna l'URL dell'immagine dell'utente
      },
      (error: HttpErrorResponse) => {
        console.error('Errore durante il caricamento dell\'immagine:', error);
      }
    );

  }
  
}