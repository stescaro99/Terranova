import {Component} from '@angular/core';
import {RouterModule} from '@angular/router';
import { TitleComponent } from './title/title.component';

@Component({
  selector: 'app-root',
  imports: [ RouterModule, TitleComponent],
  template: `
  <main>
    <app-title></app-title>
    <section>
      <router-outlet></router-outlet>
    </section>
    </main>
  `,
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'homes';
}
