
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'app/service/auth.service';
import { SidebarService } from 'app/service/sidebar.service';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTES: RouteInfo[] = [
  {
    path: '/Change', title: 'Change', icon: 'sync', class: ''
  },
  // {
  //   path: '/Generate', title: 'Generate Receipt', icon: 'receipt', class: ''
  // },
  // {
  //   path: '/History', title: 'Receipt History', icon: 'history', class: ''
  // },
  {
    path: '/reversal/import', title: 'Import Etswich Trans', icon: 'cloud_upload', class: ''
  },
  {
    path: '/reversal/alternative-list', title: 'Approval for Reversal', icon: 'check', class: ''
  },
  {
    path: '/reversal/transaction-list', title: 'Reverse Transaction', icon: 'list', class: ''
  },


  {
    path: '/reversal/adjustment-approval-list', title: 'Approve Adjustement', icon: 'check', class: ''
  },
  {
    path: '/reversal/adjustment-payment-list', title: 'pay Adjustement', icon: 'list', class: ''
  },
  {
    path: '/reversal/report', title: 'Reversal Report', icon: 'print', class: 'report'
  },
  {
    path: '/reversal/adjustement_report', title: 'Adjustement Report', icon: 'print', class: 'report'
  },

  
  {
    path: '/reversal/successfull-transaction', title: 'Reconciled transaction', icon: 'print', class: 'report'
  },
  {
    path: '/reversal/transaction-notfound-etswich', title: 'Not Reconciled at Etswich', icon: 'print', class: 'report'
  },
  {
    path: '/reversal/transaction-notfound-lib', title: 'Not Reconciled at LIB', icon: 'print', class: 'report'
  },
  {
    path: '/reversal/reconsilation-summary', title: 'Reconsilation Summary', icon: 'print', class: 'report'
  },
  {
    path: '/reversal/ethiswich_invalid_date', title: 'Ethiswich Incorect Date', icon: 'print', class: 'report'
  }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})



export class SidebarComponent implements OnInit {
  menuItems: any[];
  reportmenuItems;
  display:boolean=false;
  role:String=this.authService.getrole();
  constructor(public authService: AuthService,
    public sidebarService: SidebarService
  ) {
    this.menuItems = [];
  }


  
user:String;
ngOnInit() {



 

  this.role = this.authService.getrole();
  this.user = this.authService.getres().userName;

  const services = this.authService.Services|| []; // Services array
  this.menuItems = []; // Start with an empty menuItems array
  this.reportmenuItems =[]

  if (this.authService.isitAuthenticated()) {
    this.display = true; // Allow navigation if user is authenticated
  }

  // Role-specific menu item generation
  // if (this.role.includes('0041')||this.role.includes('0048')
  //   ||this.role.includes('0049')||
  //   this.role.includes('0017')||this.role.includes('0052')||this.role.includes('0078')||this.role.includes('0052')||this.role.includes('0051')||this.role.includes('0025')
  //   ||this.role.includes('DBD3')) {
   if (this.role.includes('0078') || this.role.includes('0025') || this.role.includes('0024') ) {
    // Approval roles
    if (services.includes('EthReconciliation')) {
      this.menuItems.push(...ROUTES.filter(menuItem => 
        // menuItem.path === '/Generate'||
        //  menuItem.path === '/History' || 
         menuItem.path === '/reversal/transaction-list'|| menuItem.path === '/reversal/import' || menuItem.path === '/reversal/report' || menuItem.path === '/reversal/adjustement_report' || 
          menuItem.path === '/reversal/successfull-transaction' ||
          menuItem.path === '/reversal/transaction-notfound-etswich' || menuItem.path === '/reversal/transaction-notfound-lib' ||
          menuItem.path === '/reversal/adjustment-payment-list'
         || menuItem.path === '/reversal/reconsilation-summary'
        || menuItem.path === '/reversal/ethiswich_invalid_date'
        )
        );

    }
    
  
  } 
  else if (this.role.includes('DBD3')) {
      if (services.includes('EthReconciliation')) {
         this.menuItems.push(...ROUTES.filter(menuItem => 
        // menuItem.path === '/Generate'||
        //  menuItem.path === '/History' || 
          menuItem.path === '/reversal/alternative-list' || menuItem.path === '/reversal/report' || menuItem.path === '/reversal/adjustement_report' || 
          menuItem.path === '/reversal/import' || menuItem.path === '/reversal/successfull-transaction' ||
          menuItem.path === '/reversal/transaction-notfound-etswich' || menuItem.path === '/reversal/transaction-notfound-lib' ||
          menuItem.path === '/reversal/adjustment-approval-list' 
         || menuItem.path === '/reversal/reconsilation-summary'
        || menuItem.path === '/reversal/ethiswich_invalid_date'
        )
        );
      }
  } 
}
//  else if (this.role === 'Admin') {
//     this.menuItems = []; // No routes for Admin
//   } 
// }



  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };
}
