import { Injectable } from "@angular/core";
import { AuthService } from "./auth.service";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuardService implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // if (this.authService.getres().role.includes("0041")||
    // this.authService.getres().role == "0048" ||this.authService.getres().role == "0041"||
    // this.authService.getres().role == "0049"||this.authService.getres().role == "0052" ||this.authService.getres().role == "0078"||
    // this.authService.getres().role == "0017"||this.authService.getres().role == "0051" ||this.authService.getres().role == "0025"
    // || this.authService.getres().role == "DBD3") {
    //   return true;
    // }
  
    if (this.authService.getres().role == "0078" || this.authService.getres().role == "0025" || this.authService.getres().role == "0024"
    || this.authService.getres().role == "DBD3") {
      return true;
    }

    else {
      // Redirect to ItComponent for non-admin users
      this.router.navigate(['login']);
      return false;
    }
    
  }
  

}
