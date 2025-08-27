import { Injectable } from '@angular/core';
import {environment} from 'environments/environment'

@Injectable({
  providedIn: 'root', // Make the service available throughout the app
})
export class ApiUrlService {
 
  readonly apiUrl = 'https://10.1.10.106:70/api/';
 // readonly apiUrl = 'https://10.1.22.206:7008/api/';
 //readonly apiUrl = 'http://10.1.22.206:4060/api/';
 //readonly apiUrl = 'https://10.1.10.106:7070/api/';

readonly apiUrlUser = 'http://10.1.10.106:4040/api';
//readonly apiUrlUser = 'http://10.1.22.25:8080/api';
readonly apiAwachUrl = environment.awachUrl;
readonly apiReversalTransUrl = environment.apiReversalTransUrl;
}