import {AfterViewInit, Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {HomeService} from "../../Services/home.service";
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit{
showHeader: string="- New User";
messageToToggle: string="";
  constructor(private router: Router,
              public homeService:DataService)
{

  }

  ngOnInit(): void {
    this.homeService.chooseComponent=0;
    this.messageToToggle="Already have an account?";
    }




    // State to toggle between Login and Login2

  toggleLogin(): void {
    //this.homeService.showLogin2 = !this.homeService.showLogin2;  // Toggle the state

    if (this.homeService.chooseComponent==0)
      this.homeService.chooseComponent=1;
    else if (this.homeService.chooseComponent==1)
      this.homeService.chooseComponent=0;



    switch (this.homeService.chooseComponent)
    {
      case 0:
        this.showHeader = "- New User";
        this.messageToToggle="Already have an account?";
        break;

      case 1:
        this.showHeader = "- Login";
        this.messageToToggle="Create An Account";
        break;

      case 2:
        this.showHeader = "- Login Success";
        this.messageToToggle="";
        break;

      case 3:
        this.showHeader = "- Update User";
        this.messageToToggle="";
        break;
    }


    // if (this.homeService.loginResponse!="Success")
    // {
    //   if (this.homeService.showLogin2)
    //     this.showHeader="- Login"
    //   else
    //   {
    //
    //
    //   }
    // }
    //   else
    //   this.showHeader="- Login information"
    //

    //if (this.homeService.newOrEditUser=="Update")
    //  this.showHeader = "- Update user";

  }

}
