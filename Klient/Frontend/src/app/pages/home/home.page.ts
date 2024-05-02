import { Component } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
showHeader: string="- New User";

  constructor(private router: Router) {
  }

  showLogin2 = false;  // State to toggle between Login and Login2

  toggleLogin(): void {
    this.showLogin2 = !this.showLogin2;  // Toggle the state

    if (this.showLogin2)
      this.showHeader="- Login"
    else
      this.showHeader="- New user"
  }
}
