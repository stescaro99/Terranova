import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-background',
  imports: [CommonModule],
  templateUrl: './background.component.html',
  styleUrl: './background.component.scss'
})
export class BackgroundComponent {
  cubes: { top: string; left: string; borderRadius: string, animationDelay: string  }[] = [];

  constructor() {
    this.generateNonOverlappingCubes(20); 
  }

  generateNonOverlappingCubes(count: number) {
    const cubeSize = 70; 
    const margin = 10;
    const positions: { top: number; left: number }[] = [];
  
    for (let i = 0; i < count; i++) {
      let top: number;
      let left: number;
      let isOverlapping: boolean;
  
      do {
        isOverlapping = false;
        top = Math.random() * (window.innerHeight - cubeSize);
        left = Math.random() * (window.innerWidth - cubeSize);
  
        for (const pos of positions) {
          const distance = Math.sqrt(
            Math.pow(pos.top - top, 2) + Math.pow(pos.left - left, 2)
          );
          if (distance < cubeSize + margin) {
            isOverlapping = true;
            break;
          }
        }
      } while (isOverlapping);
  
      positions.push({ top, left });
      this.cubes.push({
        top: `${top}px`,
        left: `${left}px`,
        borderRadius: `${Math.random() * 30}px`,
        animationDelay: `${Math.random() * 5}s`, // Ritardo casuale per l'animazione
      });
    }
  }
}