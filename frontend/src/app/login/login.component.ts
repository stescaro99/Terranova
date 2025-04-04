import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { Router } from '@angular/router';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styles: ``
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
          sessionStorage.setItem('user', JSON.stringify(this.user));
          sessionStorage.setItem('authToken', 'true');
          this.router.navigate(['/home']);
        } else {
          console.log('Username o password errati!');
          this.errorMessage = 'Username o password errati!';
        }
      },
      (error) => {
        console.error('Errore durante il login:', error);
        this.errorMessage = 'Si è verificato un errore durante il login. Riprova più tardi.';
      }
    );

  }
  sigInButton() {
    this.router.navigate(['/sigIn']);
  }
  notRegister() {
    const guestUser = new User();
    guestUser.Username = 'Guest';
    this.userService.setUser(guestUser);
    sessionStorage.setItem('username', 'Guest');
    sessionStorage.setItem('authToken', 'true');
    this.router.navigate(['/home']);
  }
}
