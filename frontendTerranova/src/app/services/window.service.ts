import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WindowService {

	private widthSubject = new BehaviorSubject<number>(window.innerWidth);

	width$ = this.widthSubject.asObservable();

	constructor() {
		window.addEventListener('resize', () => {
			this.widthSubject.next(window.innerWidth);
		});
	}
	getWidth(): number {
		return this.widthSubject.value;
	  }
	
	  get isMobile(): boolean {
		return this.getWidth() <= 767;
	  }
	
	  get isTablet(): boolean {
		return this.getWidth() > 767 && this.getWidth() <= 1024;
	  }
	
	  get isDesktop(): boolean {
		return this.getWidth() > 1024;
	  }
	  
	  getRecommendedDrinkCount(): number {
		  if (this.isMobile) {
			  return 6;
			} else if (this.isTablet) {
				return 9;
			} else {
				return 20;
			}
		}
	
	getListVisibility(): boolean {
		if (this.isMobile) {
			return false;
		} else if (this.isTablet) {
			return true;
		} else {
			return true;
		}
	}
}
