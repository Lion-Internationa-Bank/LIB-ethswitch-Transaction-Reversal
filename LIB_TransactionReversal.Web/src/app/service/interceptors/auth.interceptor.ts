import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, 
HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, from, Observable, switchMap, throwError } from 'rxjs';

import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'app/service/auth.service';
@Injectable({
 providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
 constructor(
 public authService: AuthService,
 private router: Router,
 private toastr: ToastrService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): 
    Observable<HttpEvent<any>> {
        // get the auth token
        var token =  localStorage.getItem('token');
        // if the token is present, clone the request
        // replacing the original headers with the authorization
        if (token) {
        req = req.clone({
        setHeaders: {
        Authorization: `Bearer ${token}`
        }
        });
        }
       
        // send the request to the next handler
        return next.handle(req).pipe(
        catchError((error:any) => {
        // Perform logout on 401 – Unauthorized HTTP response errors
        if (error instanceof HttpErrorResponse){
        if (error.status === 401) {
          this.authService.logout();
          this.router.navigate(['login']);
        //console.log('un Autotized') 
         return this.handelUnAuthError(req,next);
        } else if(error.status === 0){
          this.toastr.error("The Server not Responding Please try again");
          
        }}
        return throwError(error);
        })
        );
    }

     handelUnAuthError(req: HttpRequest<any>, next: HttpHandler) {
        return throwError(()=>{
          this.authService.logout();
          this.router.navigate(['login']);
        //)
     });
    }



}