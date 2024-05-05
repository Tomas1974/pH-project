import {AfterViewInit, Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {HomeService} from "../../Services/home.service";
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements AfterViewInit {
showHeader: string="- New User";

  constructor(private router: Router,
              public homeService:DataService)
{

  }

  ngAfterViewInit(): void {

    }





  showLogin2 = false;  // State to toggle between Login and Login2

  toggleLogin(): void {
    this.showLogin2 = !this.showLogin2;  // Toggle the state

    console.log(this.homeService.loginResponse);

    if (this.homeService.loginResponse!="Success")
    {
      if (this.showLogin2)
        this.showHeader="- Login"
      else
        this.showHeader="- New user"
    }
      else
      this.showHeader="- Login information"


  }

}
