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
  selectedFile: File | null = null;
   
  constructor(private userService: UserService, private router: Router) {
  }

  validateEmail() {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Regex per validare l'email
    if (!this.user.email) {
      this.emailErrorMessage = 'Email field cannot be empty.';
    } else if (!emailRegex.test(this.user.email)) {
      this.emailErrorMessage = 'Please enter a valid email (example@email.com).';
    } else {
      this.emailErrorMessage = ''; // Nessun errore
    }
  }

  submitForm() {
      if (this.usernameAvailable) {
        this.user.language = 'en';
          this.userService.createUser(this.user).subscribe(
              (response: User) => {
                  console.log('Utente creato con successo:', response);
                  alert('User created successfully!');
                  localStorage.setItem('username', this.user.username);
                  localStorage.setItem('authToken', 'true');
                  localStorage.setItem('user', JSON.stringify(this.user));
                  this.userService.setUser(this.user);
                  this.router.navigate(['/home']);
              },
              (error: HttpErrorResponse) => {
                  console.error('Errore durante la creazione dell\'utente:', error);
                    alert('Error occurred while creating the user.');
              }
          );
      } else {
          alert('Username is not available. Please choose another one.');
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
        this.selectedFile = input.files[0];
    }
    if (this.selectedFile !== null){
      const fileName = this.selectedFile.name;
      this.userService.uploadFile(fileName, this.selectedFile).subscribe(
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
  
}