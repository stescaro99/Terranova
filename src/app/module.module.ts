import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { routeConfig } from './routes';
import { LoginComponent } from './login/login.component';



@NgModule({
  declarations: [AppComponent,
    LoginComponent],
  imports: [
    CommonModule, BrowserModule, RouterModule, FormsModule,
    RouterModule.forRoot(routeConfig)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule  { }
