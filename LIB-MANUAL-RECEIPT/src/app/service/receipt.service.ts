import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { ApiUrlService } from './apiurl.service';
import { Receipt, UserData } from 'app/models/data.model';


@Injectable({
  providedIn: 'root'
})
export class ReceiptService {
 

  constructor(private http: HttpClient,private apiUrlService: ApiUrlService) { }

  getAllReceipts(): Observable<Receipt[]> {
    return this.http.get<Receipt[]>(this.apiUrlService.apiUrl + 'Reciept');
  }

  getReceipt(id:number): Observable<Receipt> {
     return this.http.get<Receipt>(this.apiUrlService.apiUrl + 'Reciept/'+id);
  }

  getManual(startDate: string, endDate: string): Observable<any> {
    const params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate);

    return this.http.post<any>(this.apiUrlService.apiUrl+ 'Reciept/manual', null, { params });
  }
  addReceipt(addReceiptRequest:any): Observable<any> {
    // addReceiptRequest.id="0000000-0000-0000-0000-000000000000"
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrlService.apiUrl + 'Reciept', addReceiptRequest,httpOptions);
  }

  updateReceipt(ReceiptDetails: Receipt,Id:number): Observable<Receipt> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<Receipt>(this.apiUrlService.apiUrl + 'Reciept/'+Id, ReceiptDetails, httpOptions);
  }

  deleteReceipt(Id: number): Observable<string> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.delete<string>(this.apiUrlService.apiUrl + 'Reciept/' + Id+'/' , httpOptions);
  }

}
