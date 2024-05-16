import {Injectable} from "@angular/core";
import {Router} from "@angular/router";
import {DataService} from "./Services/Data.service";

@Injectable()
export class AuthGuard {

  constructor(private router: Router, readonly dataService: DataService) {}

  canActivate() {
    if (this.dataService.loginUser != "") {
      // logged in so return true'
      return true;
    }

    // not logged in so redirect to login page with the return url
    this.router.navigate(['home']);
    return false;
  }
}
