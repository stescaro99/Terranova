import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { Router } from '@angular/router';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { BackgroundComponent } from '../../background/background.component';


@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, BackgroundComponent],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';
  constructor(private router: Router, private userService: UserService) {}
  user: User = new User();
  
  loginRoute() {
    console.log(this.username, this.password);
    this.userService.isUserRegistered(this.username, this.password).subscribe(
      (response: User | null) => {
        if (response) {
          console.log('Login effettuato con successo!');
          this.user = response;
          console.log('Utente:', response);
          localStorage.setItem('user', JSON.stringify(this.user));
          localStorage.setItem('authToken', 'true');
          localStorage.setItem('guestToken', 'false');
          this.userService.setUser(this.user);
          this.router.navigate(['/home']);
        } else {
          console.log('Username o password errati!');
          this.errorMessage = 'Wrong Username or Password!';
        }
      },
      (error) => {
        console.error('Errore durante il login:', error);
        this.errorMessage = 'An error occurred during login. Please try again later.';
      }
    );

  }
  sigInButton() {
    this.router.navigate(['/sigIn']);
  }
  notRegister() {
    const guestUser = new User();
    guestUser.username = 'Guest';
    guestUser.language = 'en';
    this.userService.setUser(guestUser);
    localStorage.setItem('user', JSON.stringify(guestUser));
    localStorage.setItem('authToken', 'true');
    localStorage.setItem('guestToken', 'true');
    console.log('Accesso come ospite effettuato con successo!');
    this.router.navigate(['/home']);
  }
}
