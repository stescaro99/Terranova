import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { Router } from '@angular/router';

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
      <button (click)="sigInButton()">sig-in</button>
  `,
  styles: ``
})
export class LoginComponent {
  username = '';
  password = '';
  constructor(private router: Router) {}
  
  loginRoute() {
    console.log(this.username, this.password);
  }
  sigInButton() {
    this.router.navigate(['/sigIn']);
  }
}
