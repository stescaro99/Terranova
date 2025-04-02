import { Component } from '@angular/core';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-home-page',
  imports: [],
  template: `
      <h3>HOME PAGE</h3>
      <h4>Welcome to the Aper project! {{user.Username}}</h4>
      <br>
      <h1>list of cocktails of the day</h1>
      <br>
      <h2>list of your favorite cocktails</h2>
      <br>
      <button (click)="logout()">Logout</button>
  `,
  styles: ``
})
export class HomePageComponent {
  user: User;

  constructor(private userService: UserService) {
    this.user = this.userService.getUser();
  }
  logout() {  
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('authToken');
    this.userService.setUser(new User());
    window.location.reload();
  }

}
