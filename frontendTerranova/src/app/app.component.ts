import {Component, HostListener} from '@angular/core';
import {RouterModule} from '@angular/router';
import { TitleComponent } from './title/title.component';


@Component({
  selector: 'app-root',
  imports: [ RouterModule],
  template: `
  <main>
    <section>
      <router-outlet></router-outlet>
    </section>
    </main>
  `,
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'homes';
  @HostListener('window:beforeunload', ['$event'])
  clearSessionStorage(event: Event) {
    if (sessionStorage.getItem('guestToken') || localStorage.getItem('guestToken')) {
      sessionStorage.removeItem('authToken');
      sessionStorage.removeItem('username');
      sessionStorage.removeItem('guestToken');
      localStorage.removeItem('authToken');
      localStorage.removeItem('username');
      localStorage.removeItem('guestToken');
      localStorage.removeItem('user');
      console.log('Dati dell\'ospite rimossi.');
    } else {
      console.log('Utente non ospite, nessuna azione eseguita.');
    }
  }
}