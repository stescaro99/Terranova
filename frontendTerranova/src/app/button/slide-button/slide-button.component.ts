import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-slide-button',
  imports: [ CommonModule ],
  templateUrl: './slide-button.component.html',
  styleUrl: './slide-button.component.css'
})
export class SlideButtonComponent {
	@Input() isActive: boolean = false;
	@Output() isActiveChange = new EventEmitter<boolean>();

	toggle() {
		this.isActive = !this.isActive;
		this.isActiveChange.emit(this.isActive);
	} 
}
