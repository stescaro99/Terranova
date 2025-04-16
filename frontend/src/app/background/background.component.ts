import { Component, LOCALE_ID } from '@angular/core';
import { User } from '../user/user.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-background',
  imports: [CommonModule],
  templateUrl: './background.component.html',
  styleUrl: './background.component.scss'
})

export class BackgroundComponent {
  images: { src: string; top: string; left: string; animationDelay: string; size: string; rotation: string }[] = [];
  isPopupVisible: boolean = false; // Proprietà per controllare la visibilità del pop-up
  user: User = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '') : new User();
  isDropdownOpen: boolean = false;

  constructor(private userService: UserService, private router: Router) {
    this.generateImages(20); 
  }

  generateImages(count: number) {
    const imagePaths = [
      'assets/background/ice.png',
      'assets/background/ice_two.png',
      'assets/background/ice_boar.png',
      'assets/background/image_tree.png',
      'assets/background/mint.png',
      'assets/background/orange.png',
    ];
  
    for (let i = 0; i < count; i++) {
      const randomImage = imagePaths[Math.floor(Math.random() * imagePaths.length)];
      const top = '100vh'; // Inizia sempre fuori dal fondo dello schermo
      const left = Math.random() * 100 + '%'; // Posizione orizzontale casuale
      const animationDelay = Math.random() * 10 + 's'; // Ritardo casuale fino a 10 secondi
      const size = Math.random() * 100 + 50 + 'px'; // Dimensione casuale tra 50px e 150px
      const rotation = Math.random() * 360 + 'deg'; // Rotazione casuale tra 0° e 360°
  
      this.images.push({ src: randomImage, top, left, animationDelay, size, rotation });
    }
  }

  togglePopup() {
    this.isPopupVisible = !this.isPopupVisible; 
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
    localStorage.removeItem('user');
    localStorage.removeItem('guestToken');
    localStorage.removeItem('authToken');
    this.userService.setUser(new User());
    window.location.reload();
  }

}