import { Component } from '@angular/core';
import { User } from '../../model/user';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-language-button',
  imports: [CommonModule],
  templateUrl: './language-button.component.html',
  styleUrl: './language-button.component.css'
})
export class LanguageButtonComponent {
  user: User;
  isDropdownOpen: boolean = false;
  availableLanguages: { code: string, name: string, flag: string }[] = [
    { code: 'en', name: 'English', flag: '🇬🇧' },
    { code: 'it', name: 'Italiano', flag: '🇮🇹' },
    { code: 'es', name: 'Español', flag: '🇪🇸' },
    { code: 'fr', name: 'Français', flag: '🇫🇷' },
    { code: 'de', name: 'Deutsch', flag: '🇩🇪' },
    { code: 'zh', name: '中文', flag: '🇨🇳' },
    { code: 'ja', name: '日本語', flag: '🇯🇵' }
  ];

  constructor(private userService: UserService) {
    this.user = this.userService.getUser() || new User();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  selectLanguage(languageCode: string): void {
    this.user.language = languageCode;
    this.userService.setUser(this.user);
	this.userService.updateUser(this.user.id, this.user.language, languageCode).subscribe(
		(value: any) =>{
			console.log('language change', value);
		},
		(error) =>
		{
			console.error(error);
		}
	);
    this.isDropdownOpen = false;
  }

  getCurrentLanguageFlag(): string {
    const currentLanguage = this.availableLanguages.find(lang => lang.code === this.user.language);
    return currentLanguage ? currentLanguage.flag : '🌐';
  }
}
