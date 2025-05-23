import {  Component, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-username',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './username.component.html',
  styleUrl: './username.component.css'
})
export class UsernameComponent {
	@Input() user! : User;
	@Output() userSelected = new EventEmitter<boolean>();
	constructor(private userService : UserService) {};
	usernameMessage = '';
	usernameAvailable: boolean = false;

	checkUsername() {
		if (!this.user.username) {
		this.usernameMessage = 'Username cannot be empty.';
		this.usernameAvailable = false;
		return;
		}

		this.userService.getUserByUsername(this.user.username).subscribe(
		(isAvailable: boolean) => {
			if (isAvailable) {
			this.usernameMessage = `The Username "${this.user.username}" is available.`;
			this.usernameAvailable = true;
			this.userSelected.emit(this.usernameAvailable)
			} else {
			this.usernameMessage = `The Username "${this.user.username}" is already in use.`;
			this.usernameAvailable = false;
			this.userSelected.emit(this.usernameAvailable)
			}
		},
		(error: HttpErrorResponse) => {
			console.error('Errore durante il controllo dell\'username:', error);
			this.usernameMessage = 'Error while checking the Username.';
			this.usernameAvailable = false;
		}
		);
	}

}
