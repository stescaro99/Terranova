import { Component } from '@angular/core';
import { User } from '../user/user.model';
import { UserService } from '../services/user.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule],
  templateUrl: './home-page.component.html',
  styles: ``
})
export class HomePageComponent {
  user: User;
  isAuthenticated: boolean;
  isDropdownOpen: boolean = false;

  constructor(private userService: UserService, private router: Router) {
    this.user = this.userService.getUser();
    console.log('Valore di user.ImgUrl:', this.user.ImgUrl);
    this.isAuthenticated = !!sessionStorage.getItem('authToken');
    if (!this.user.ImgUrl) {
      console.log('Nessuna immagine trovata, verrà mostrato il cerchio grigio.');
    }
  }

  onUserPhotoClick() {
    console.log('User photo clicked!');
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  goToSettings() {
    console.log('Navigazione alle impostazioni...');
    this.router.navigate(['/settings']);
  }

  logout() {  
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('authToken');
    this.userService.setUser(new User());
    window.location.reload();
  }

}
