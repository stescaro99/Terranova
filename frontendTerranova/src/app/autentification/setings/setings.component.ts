import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BackgroundComponent } from '../../background/background.component';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { CountryComponent } from '../country/country.component';
import { AgeComponent } from '../age/age.component';
import { UsernameComponent } from '../username/username.component';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-setings',
  imports: [BackgroundComponent, CountryComponent, AgeComponent, UsernameComponent, CommonModule],
  templateUrl: './setings.component.html',
  styleUrl: './setings.component.css'
})
export class SetingsComponent {
	newUserdata: User = new User();
	user: User;
	emailErrorMessage: string = '';
	selectedFile: File | null = null;

	constructor(private userService: UserService, private router: Router){
		this.user = this.userService.getUser() || new User();
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

	submitForm(){
		for( const key in this.newUserdata){
			if (key === 'id')
				continue;
			const value = this.newUserdata[key as keyof User]
			if (typeof value === 'string') {
            	if (value.trim() === '')
					continue;
				if (value !== this.user[key as keyof User])
					this.userService.updateUser(this.user.id, key, value).subscribe(
						(value: any)=>{
							console.log('change ${key}', value);
						},
						(error)=>{
							console.error(error);
						}
					);
    		}
		}
		this.router.navigate(['/home']);
 	}
}
