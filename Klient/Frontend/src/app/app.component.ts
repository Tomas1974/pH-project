import { Component } from '@angular/core';
import {DataService} from "./Services/Data.service";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {



  constructor(public dataservice: DataService) {}
}
