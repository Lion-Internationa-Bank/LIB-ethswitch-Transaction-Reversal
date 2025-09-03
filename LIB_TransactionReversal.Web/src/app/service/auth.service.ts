import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';
import { Observable, throwError, timer } from 'rxjs';
import { catchError, switchMap, takeWhile } from 'rxjs/operators';
import { Login } from 'app/models/data.model';
import { TransactionService } from './transaction.service';
import { SidebarService } from './sidebar.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticated: boolean = false;
  public Role: string;
  public Services: string;
  public name: string;
  public branch: string;
  public branchname: string;
  public id: number;
  public password: String;
  public res: Login;
  public incorrect: boolean = false;
  public suspended: boolean = false;
  public tra: boolean = false;
  public roleincorrect: boolean = false;
  public locked:boolean=false;
  public message:string="";
  public mustchangepassword:boolean=false
  private MAX_FAILED_ATTEMPTS = 10;
  private failedAttempts: { [username: string]: number } = {};
  public accountLocked: { [username: string]: boolean } = {};
  token:string;

  newUser: Login = {
    id: 0,
    branch: "",
    fullName: "",
    userName: "",
    password: "",
    role: "",
    branchCode: "",
    updatedBy: "",
    updatedDate: "",
    status: "",
  };

  constructor(
    private loginService: LoginService,
    private router: Router,
    private sidebarService:SidebarService,
    private transactionService: TransactionService
  ) {}

  login(username: string, password: string) {
    this.loginService.login(username, password).subscribe({
      next: (response) => {
        if (response && response.success) {

      
          // Store token and user data in local storage
          localStorage.setItem('token', response.token);
          this.token=response.token;
        


          localStorage.setItem('userData', JSON.stringify({
            branch: response.branch,
            branchCode: response.branchCode,
            role: response.role,
            services: response.services,
            userName:username
          }));

          // Set user details in the component
          this.name = username;
          this.branch = response.branchCode;
          this.branchname = response.branch;
          this.Role = response.role;
          this.Services=response.services;
 

       
          // Navigate based on role
          this.redirectBasedOnRole(response.role,response.services);
          if (response.mustChangePassword) {
            // Redirect to the Change Password page
            this.mustchangepassword=true;
            this.tra = true;
           this.name=username;
            this.router.navigate(['/Change']);
            return;
          }
        } else {
          this.message=response.message;
          setTimeout(() => this.message="", 2000);
          console.error('Login failed: Invalid response format',response);
        }
      },
      error: (err) => {

        console.error('Login error:', err);
        this.incorrect = true;
        this.message='Login Error';
        setTimeout(() => this.message="", 2000);
        setTimeout(() => this.resetStatusFlags(), 1000); // Reset after 1 second
      },
    });
  }
  
  // redirectBasedOnRole(role: string) {
  //   this.tra=true;
  //   switch (role) {
  //     case 'Admin':
  //       this.router.navigate(['/Admin']);
  //       break;
  //     case 'FanaAdmin':
  //       this.router.navigate(['/FanaCustom']);
  //       break;
  //     case 'Finance':
  //       this.router.navigate(['/RtgsAllReport']);
  //       break;
  //     case '0052':
  //     case '0073':
  //     case '0017':
  //       this.router.navigate(['/Request']);
  //       break;
  //     case '0048':
  //     case '0041':
  //     case '0049':
  //       this.router.navigate(['/Approval']);
  //       break;
  //     case '0078':
  //       this.router.navigate(['/FanaReport']);
  //       break;
  //     default:
  //       console.error('Role not recognized:', role);
  //       this.roleincorrect = true;
  //       break;
  //   }
  // }
  

  redirectBasedOnRole(role: string, services: string[]) {
    this.tra = true;
    // Define navigation routes for services
    // const serviceRoutes = {
    //   Reciept: {
    //     Generate: '/Generate',
    //     History: '/History',
       
    //   },
     
    // };
  
    // Check the services and route accordingly
    // const navigateToServiceRoute =
    //  (service: string, routeType: 'Generate' |'History') => {
    //   if (serviceRoutes[service]) {
    
    //     //this.router.navigate([serviceRoutes[service][routeType]]);
    //     this.router.navigateByUrl('/reversal/import')
    //   } else {
    //     console.error(`Service '${service}' not recognized`);
    //   }
    // };
  
      switch (role) {
        // case '0041':
        //   if (services.includes('Reciept')) {
        //     navigateToServiceRoute('Reciept', 'Generate');
        //   }
        //   break;
    
        //   case '0048':
        //   if (services.includes('Reciept')) {
        //     navigateToServiceRoute('Reciept', 'Generate');
        //   }
        //   break;
        //   case '0052':
        //     if (services.includes('Reciept')) {
        //       navigateToServiceRoute('Reciept', 'Generate');
        //     }
        //     break;
        //     case '0049':
        //       if (services.includes('Reciept')) {
        //         navigateToServiceRoute('Reciept', 'Generate');
        //       }
        //       break;
        //       case '0017':
        //         if (services.includes('Reciept')) {
        //           navigateToServiceRoute('Reciept', 'Generate');
        //         }
        //         break;
        //         case '0051':
        //           if (services.includes('Reciept')) {
        //             navigateToServiceRoute('Reciept', 'Generate');
        //           }
        //           break;
                case '0025':
                  if (services.includes('EthReconciliation')) {
                    this.router.navigateByUrl('/reversal/transaction-list')
                  }
                  break;
                 case '0024':
                  if (services.includes('EthReconciliation')) {
                    this.router.navigateByUrl('/reversal/transaction-list')
                  }
                  break;
                case 'DBD3':
                  if (services.includes('EthReconciliation')) {
                    this.router.navigateByUrl('/reversal/import')
                  }
                  break;
                case '0078':
                  if (services.includes('EthReconciliation')) {
                    this.router.navigateByUrl('/reversal/import')
                  }
                  break;
        default:
          console.error('Role not recognized:', role);
          this.roleincorrect = true;
          break;
      }
   
  }
  

  getUserData() {
    const role = this.getrole();
    const branch = this.getbranch();
    const user = this.getuser();

    return this.transactionService.getUserDetails(branch, user, role);
  }

  getincorrect(): boolean {
    return this.incorrect;
  }

  getrole(): string {
    return this.Role;
  }

  getbranch(): string {
    return this.branch;
  }
  getbranchName(): string {
    return this.branchname;
  }
  
  getuser(): string {
    return this.name;
  }

  getid(): number {
    return this.id;
  }

  getpassword(): String {
    return this.password;
  }

  getres(): Login {
    this.res = JSON.parse(localStorage.getItem('userData'));
    return this.res;
  }

  logout(): void {
    localStorage.removeItem('token');
    this.isAuthenticated = false;
    this.Role = '';
    this.router.navigate(['/login']);
  }

  isitAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  private startTokenExpirationTimer(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const tokenParts = token.split('.');
    if (tokenParts.length !== 3) {
      console.error('Invalid token format');
      return;
    }

    const payload = JSON.parse(atob(tokenParts[1]));
    if (!payload || !payload.exp) {
      console.error('Expiration time not found in token payload');
      return;
    }

    const expirationTime = payload.exp * 1000; // Convert expiration time to milliseconds
    const currentTime = Date.now();
    const adjustedExpirationTime = currentTime + (60 * 60 * 1000); // 120 minutes from now
    const timeUntilExpiration = Math.max(adjustedExpirationTime - currentTime, expirationTime - currentTime);
    
    timer(timeUntilExpiration)
      .pipe(takeWhile(() => this.isitAuthenticated()))
      .subscribe(() => {
        this.logout();
      });
  }

  private resetStatusFlags(): void {
    this.incorrect = false;
    this.locked = false;
    this.suspended = false;
    this.roleincorrect = false;
  }
}
