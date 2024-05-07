import {AfterViewInit, Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {HomeService} from "../../Services/home.service";
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage  {
showHeader: string="- New User";

  constructor(private router: Router,
              public homeService:DataService)
{

  }




    // State to toggle between Login and Login2

  toggleLogin(): void {
    this.homeService.showLogin2 = !this.homeService.showLogin2;  // Toggle the state


    console.log("Test mig "+this.homeService.newOrEditUser)

        

    if (this.homeService.loginResponse!="Success")
    {
      if (this.homeService.showLogin2)
        this.showHeader="- Login"
      else
      {

          this.showHeader = "- New user";
      }
    }
      else
      this.showHeader="- Login information"


    //if (this.homeService.newOrEditUser=="Update")
    //  this.showHeader = "- Update user";

  }

}
