import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiUrlService } from 'app/service/apiurl.service';
import { AuthService } from 'app/service/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReversalService {
  
    _getListForReversalUrl = 'api/lib/v1/checkDailyLimit'
    _SaveSelectedForReversalUrl = 'api/TransactionReversal'
    _getSelectedForReversalUrl = 'api/TransactionReversal'
    _createTransactionUrl = 'api/TransactionReversal/CreateTransaction'
    _getReversalReportUrl = 'api/TransactionReversal/GetReversalReport'
    _getImportTransactiontUrl ='api/EthswitchTransactionImport'
    _getSuccessfullTransactionUrl='api/EthswitchTransactionImport/GetSuccessfullTransaction'
    _getTransactionNotFoundLibUrl = 'api/EthswitchTransactionImport/GetTransactionNotFoundLib'
    _getTransactionNotFoundEthswitchUrl = 'api/EthswitchTransactionImport/GetTransactionNotFoundEthswitch'
    _ReconsilePendingTransactionUrl = 'api/EthswitchTransactionImport/ReconsilePendingTransaction'
    _CheckedPendingTransactionForReversalUrl = 'api/TransactionReversal/CheckedPendingTransactionForReversal'
    _GetImportedTransactionUrl = 'api/EthswitchTransactionImport/GetImportedTransaction'

    _getSelectedForAdjustementUrl = 'api/TransactionAdjustement'
    _createAdustmentTransactionUrl = 'api/TransactionAdjustement/CreateTransaction'
    _CheckedPendingTransactionForAdjustementUrl = 'api/TransactionAdjustement/CheckedPendingTransactionForReversal'
    _UpdateTransactionAccontUrl = 'api/TransactionAdjustement/updateTransactionAccountNumber'
    _ReconsilationSummaryUrl = 'api/EthswitchTransactionImport/ReconsillationSummaryReport'
    _InvalidEthiswichDateTransactionUrl = 'api/EthswitchTransactionImport/GetInvalidEthiswichDateTransaction'
    _getAdjustementReportUrl = 'api/TransactionAdjustement/GetAdjustementReport'
    _getAccountHolderNameUrl = 'api/EthswitchTransactionImport/GetAccountHolderName'


  get getListForReversalUrl(){
    return this._getListForReversalUrl;
  }
  get SaveSelectedForReversalUrl(){
    return this._SaveSelectedForReversalUrl;
  }
  get getSelectedForReversalUrl(){
    return this._getSelectedForReversalUrl;
  }
  get createTransactionUrl(){
    return this._createTransactionUrl;
  }
  get getReversalReportUrl(){
    return this._getReversalReportUrl;
  }

  get getImportTransactiontUrl(){
    return this._getImportTransactiontUrl;
  }

  get getSuccessfullTransactionUrl(){
    return this._getSuccessfullTransactionUrl;
  }

  get getTransactionNotFoundLibUrl(){
    return this._getTransactionNotFoundLibUrl;
  }

  get getTransactionNotFoundEthswitchUrl(){
    return this._getTransactionNotFoundEthswitchUrl;
  }

  get ReconsilePendingTransactionUrl(){
    return this._ReconsilePendingTransactionUrl;
  }
  get CheckedPendingTransactionForReversalUrl(){
    return this._CheckedPendingTransactionForReversalUrl;
  }
  get GetImportedTransactionUrl(){
    return this._GetImportedTransactionUrl;
  }
  get getSelectedForAdjustementUrl(){
    return this._getSelectedForAdjustementUrl;
  }

  get createAdustmentTransactionUrl(){
    return this._createAdustmentTransactionUrl;
  }

  get CheckedPendingTransactionForAdjustementUrl(){
    return this._CheckedPendingTransactionForAdjustementUrl;
  }

  get UpdateTransactionAccontUrl(){
    return this._UpdateTransactionAccontUrl;
  }
  
  get ReconsilationSummaryUrl(){
    return this._ReconsilationSummaryUrl;
  }

  get InvalidEthiswichDateTransactionUrl(){
    return this._InvalidEthiswichDateTransactionUrl;
  }
  
  get getAccountHolderNameUrl(){
    return this._getAccountHolderNameUrl;
  }

    get getAdjustementReportUrl(){
    return this._getAdjustementReportUrl;
  }
  
  constructor(private httpClient: HttpClient,
              private apiUrl: ApiUrlService,
              private authservice: AuthService) { }

  // getListForReversal(): Observable<any>{
  //   return this.httpClient.get('http://10.1.10.90:7000/api/lib/v1/checkDailyLimit')
  // }

  SaveSelectedForReversal(transactionforReversal): Observable<any>{
    
    return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.SaveSelectedForReversalUrl}`,transactionforReversal)
  }

  getSelectedForReversal(params): Observable<any>{
   
    const param = new HttpParams()
    .append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')  
    .append('ReferenceNo' , params.ReferenceNo != undefined ? params.ReferenceNo : '')  
    .append('Date', params?.Date)
   // .append('DateTo', params?.DateTo)
    .append('Status', params.Status != undefined ? params.Status : '0')
    return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getSelectedForReversalUrl}`,{params: param})
  }
  createTransaction(id): Observable<any>{
    return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.createTransactionUrl}/${id}`)
  }

  getReversalReport(params): Observable<any>{
    const param = new HttpParams()
    .append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')   
    .append('DateFrom', params?.DateFrom)
    .append('DateTo', params?.DateTo)
    return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getReversalReportUrl}`,{params: param})
  }

  ImportTransaction(transList): Observable<any>{
    return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.getImportTransactiontUrl}`,transList)
  }

  getSuccessfullTransaction(params): Observable<any>{

  const param = new HttpParams()
  .append('TransactionType', params.TransactionType)
  .append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')   
  .append('GlAccountNo', params.GlAccountNo != undefined ? params.GlAccountNo : '')
  .append('Date', params?.Date)
  // .append('DateTo', params?.DateTo)
  return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getSuccessfullTransactionUrl}`,{params: param})
 }

 getTransactionNotFoundLib(params): Observable<any>{

const param = new HttpParams()
.append('TransactionType', params.TransactionType)
.append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')   
.append('GlAccountNo', params.GlAccountNo != undefined ? params.GlAccountNo : '')
.append('Date', params?.Date)
// .append('DateTo', params?.DateTo)
 return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getTransactionNotFoundLibUrl}`,{params: param })
 }

 getTransactionNotFoundEthswitch(params): Observable<any>{

const param = new HttpParams()
.append('TransactionType', params.TransactionType)
.append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')   
.append('Date', params?.Date)
.append('GlAccountNo', params.GlAccountNo != undefined ? params.GlAccountNo : '')
// .append('DateTo', params?.DateTo)
 return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getTransactionNotFoundEthswitchUrl}`,{params: param })
 }

 ReconsilePendingTransaction(params): Observable<any>{
  const param = new HttpParams()
    .append('Date', params?.Date)
    //.append('DateTo', params?.DateTo)
   return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.ReconsilePendingTransactionUrl}`,{params: param})
 }

 CheckedPendingTransactionForReversal(ids): Observable<any>{
 
 return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.CheckedPendingTransactionForReversalUrl}`,ids)
 }

 GetImportedTransaction(params): Observable<any>{
  
  const param = new HttpParams()
    .append('Date', params?.Date)
    //.append('DateTo', params?.DateTo)
    .append('TransactionType', params.TransactionType)
   return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.GetImportedTransactionUrl}`,{params: param})
 }

 createBatchTransaction(ids): Observable<any>{
  
 return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.createTransactionUrl}`,ids)
 }

 getSelectedForAdjustement(params): Observable<any>{
  
  const param = new HttpParams()
  .append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')  
  .append('ReferenceNo' , params.ReferenceNo != undefined ? params.ReferenceNo : '')  
  .append('Date', params?.Date)
  // .append('DateTo', params?.DateTo)
  .append('Status', params.Status != undefined ? params.Status : '0')
  return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getSelectedForAdjustementUrl}`,{params: param})
}

createAdjustmentTransaction(id): Observable<any>{
  
  return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.createAdustmentTransactionUrl}/${id}`)
}

createBatchAdjustmentTransaction(ids): Observable<any>{
  
  return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.createAdustmentTransactionUrl}`,ids)
}

CheckedPendingTransactionForAdjustment(ids): Observable<any>{
 return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.CheckedPendingTransactionForAdjustementUrl}`,ids)
 }

  updateTransactionAccount(transaction): Observable<any>{
  
 return this.httpClient.post(`${this.apiUrl.apiReversalTransUrl}${this.UpdateTransactionAccontUrl}`,transaction)
 }

   ReconsilationSummary(params): Observable<any>{
    const param = new HttpParams()
    .append('DateFrom', params?.DateFrom)
    .append('DateTo', params?.DateTo)
    return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.ReconsilationSummaryUrl}`,{params: param })
  }

  InvalidEthiswichDateTransaction(params): Observable<any>{
  const param = new HttpParams()
  .append('Date', params?.Date)
  // .append('DateTo', params?.DateTo)
  return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.InvalidEthiswichDateTransactionUrl}`,{params: param})
 }

   getAdjustementReport(params): Observable<any>{
      
    const param = new HttpParams()
    .append('AccountNo', params.AccountNo != undefined ? params.AccountNo : '')   
    .append('DateFrom', params?.DateFrom)
    .append('DateTo', params?.DateTo)
    return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getAdjustementReportUrl}`,{params: param })
  }

  getAccountHolderName(account): Observable<any>{
    return this.httpClient.get(`${this.apiUrl.apiReversalTransUrl}${this.getAccountHolderNameUrl}/${account}`)
  }

}
