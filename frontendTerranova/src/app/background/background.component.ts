import { Component, LOCALE_ID } from '@angular/core';
import { User } from '../model/user';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { LanguageButtonComponent } from '../button/language-button/language-button.component';
import { TitleComponent } from '../title/title.component';


@Component({
  selector: 'app-background',
  imports: [CommonModule, LanguageButtonComponent, TitleComponent],
  templateUrl: './background.component.html',
  styleUrl: './background.component.css'
})

export class BackgroundComponent {
  images: { src: string; top: string; left: string; animationDelay: string; size: string; rotation: string }[] = [];
  isPopupVisible: boolean = false;
  user: User = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '') : new User();
  isDropdownOpen: boolean = false;

  constructor(private userService: UserService, private router: Router) {
    this.generateImages(20); 
  }

  generateImages(count: number) {
    const imagePaths = [
      '/assets/background/ice.png',
      '/assets/background/ice_two.png',
      '/assets/background/ice_boar.png',
      '/assets/background/image_tree.png',
      '/assets/background/mint.png',
      '/assets/background/orange.png',
    ];
//	frontendTerranova/src/assets/background/ice.png
  
    for (let i = 0; i < count; i++) {
      const randomImage = imagePaths[Math.floor(Math.random() * imagePaths.length)];
      const top = '100vh'; 
      const left = Math.random() * 100 + '%'; 
      const animationDelay = Math.random() * 10 + 's';
      const size = Math.random() * 100 + 50 + 'px';
      const rotation = Math.random() * 360 + 'deg';
  
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
  delete(){
	localStorage.removeItem('user');
    localStorage.removeItem('guestToken');
    localStorage.removeItem('authToken');
	this.userService.deleteUser(this.user.id).subscribe(
		(value: any)=>{
			console.log('user delete', value);
		},
		(error)=>{
			console.error(error);
		}
	)
	this.router.navigate(['login']);
  }

}