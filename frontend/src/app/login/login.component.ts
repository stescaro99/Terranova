import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { Router } from '@angular/router';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  template: `
    <h1> login</h1>
    <br>
      <input type="text" placeholder="username"  [(ngModel)]="username"/>
      <input type="password" placeholder="password" [(ngModel)]="password" />
      <button (click)="loginRoute()">login</button>
    <br>
    <br>
    <br>
      <button (click)="sigInButton()">iscriviti</button>
    <br>
    <br>
    <br>
      <button (click)="notRegister()">continua enza accedere</button>
  `,
  styles: ``
})
export class LoginComponent {
  username = '';
  password = '';
  constructor(private router: Router, private userService: UserService) {}
  user: User = new User();
  
  loginRoute() {
    console.log(this.username, this.password);
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
