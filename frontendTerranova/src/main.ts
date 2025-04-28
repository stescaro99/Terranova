import {bootstrapApplication, provideProtractorTestingSupport} from '@angular/platform-browser';
import {AppComponent} from './app/app.component';
import {provideRouter} from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';


bootstrapApplication(AppComponent, {providers: [provideProtractorTestingSupport(), provideHttpClient(withInterceptorsFromDi()) , provideRouter(routes)]}).catch((err) =>
  console.error(err),
);
