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

    if (this.homeService.chooseComponent==0) {
      this.homeService.chooseComponent = 1;
      this.messageToToggle="Create An Account";
    }
    else if (this.homeService.chooseComponent==1)
    { this.homeService.chooseComponent=0;

      this.messageToToggle="Already have an account?";
    }




  }

  changeHeadline()
   {

     console.log("adj "+this.homeService.chooseComponent)

    switch (this.homeService.chooseComponent)
    {
      case 0:
        this.showHeader = "- New User";

        break;

      case 1:
        this.showHeader = "- Login";

        break;

      case 2:
        this.showHeader = "- Login Success";

        break;

      case 3:
        this.showHeader = "- Update User";

        break;
    }
  }


}
