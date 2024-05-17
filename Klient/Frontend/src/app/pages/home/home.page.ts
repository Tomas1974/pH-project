import {AfterViewInit, Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit{

messageToToggle: string="";
  constructor(private router: Router,
              public homeService:DataService)
{

  }

  ngOnInit(): void {
    this.homeService.chooseComponent=0;
    this.messageToToggle="Already have an account?";
    }



  toggleLogin(): void {


    if (this.homeService.chooseComponent==0) {
      this.homeService.chooseComponent = 1;
      this.messageToToggle="Create An Account";
    }
    else if (this.homeService.chooseComponent==1)
    { this.homeService.chooseComponent=0;

      this.messageToToggle="Already have an account?";
    }




  }


}
