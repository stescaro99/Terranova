import { Component, EventEmitter,  Output} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-age',
  imports: [FormsModule, CommonModule],
  standalone: true,
  templateUrl: './age.component.html',
  styleUrl: './age.component.scss'
})
export class AgeComponent {
  @Output() DateSelected = new EventEmitter<string>();
  dateErrorMessage: string = '';
  birthDate: string = '';
  
  calculateAge() {
    if (this.birthDate) {
      const today = new Date();
      const birthDateObj = new Date(this.birthDate);
  
      const yesterday = new Date();
      yesterday.setDate(today.getDate() - 1);
  
      if (birthDateObj > yesterday) {
        this.dateErrorMessage = 'La data di nascita non può essere futura o il giorno corrente.';
        return;
      } else {
        this.dateErrorMessage = '';
        this.DateSelected.emit(this.birthDate);
      }
    }
  }
}
