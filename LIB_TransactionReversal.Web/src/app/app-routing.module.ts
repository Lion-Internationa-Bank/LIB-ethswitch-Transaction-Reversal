import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultComponent } from './layouts/default/default.component';
import { LoginComponent } from './modules/login/login.component';
import { AuthGuardService } from './service/auth-guard.service';
import { AdminAuthGuardService } from './service/admin-auth-guard.service';
import { ChangePasswordComponent } from './modules/change-password/change-password.component';




const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: DefaultComponent,
    canActivate: [AuthGuardService],
    children: [
      { path: 'Change', component: ChangePasswordComponent },
   
     
      // {
      //   path: 'Admin',
      //   component: AdminComponent,
      //   canActivate: [AdminAuthAdminGuardService],
      // },
      // {
      //   path: 'Generate',
      //   component: GeneraterecieptComponent,
      //   canActivate: [AdminAuthGuardService],
      // },
      // {
      //   path: 'History',
      //   component: ReciepthistoryComponent,
      //   canActivate: [AdminAuthGuardService]
      // }, 
      {
        path: 'reversal',
        loadChildren:()=>import('./modules/reversal/reversal.module').then(m=>m.ReversalModule),
        canActivate: [AdminAuthGuardService]
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
