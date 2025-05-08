import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { routes } from '../app.routes';


@Component({
  selector: 'app-title',
  imports: [RouterModule],
  templateUrl: './title.component.html',
  styleUrl: './title.component.css'
})
export class TitleComponent {

}
