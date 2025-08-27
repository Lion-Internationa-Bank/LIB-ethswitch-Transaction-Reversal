import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { ReversalService } from '../services/reversal.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MatPaginator } from '@angular/material/paginator';
import { AngularCsv } from 'angular-csv-ext';
import { ExcelService } from '../services/excel.service';

@Component({
  selector: 'app-transaction-not-found-ethswitch',
  templateUrl: './transaction-not-found-ethswitch.component.html',
  styleUrls: ['./transaction-not-found-ethswitch.component.css']
})
export class TransactionNotFoundEthswitchComponent implements AfterViewInit{
  displayedColumns: string[] = ['accountNumber', 'receiverAccount', 'amount', 'rrn', 'createdAt','bankName'];
  dataSource = new MatTableDataSource([]);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  TransactionSearchForm:FormGroup;

  constructor(private reversalService :ReversalService,
              private ngxService: NgxUiLoaderService,
              private fb: FormBuilder,
              private toaster:ToastrService,
              private excelService: ExcelService){
  //  this.getReversalReport();
   this.TransactionSearchForm = fb.group({
    TransactionType:[,Validators.required],
    AccountNo:[],
    TransactionDate:[],
    // DateTo:[],
    GlAccountNo:[],
    status:['0']
    });
   
  }

  getReversalReport(){
    this.ngxService.start('getReversalReport');
    this.reversalService.getTransactionNotFoundEthswitch({}).subscribe(
      res => {
         this.dataSource = new MatTableDataSource(res);
         this.dataSource.paginator = this.paginator;
         this.ngxService.stop('getReversalReport');
      }, error => {
        this.toaster.error("Please try again");
        this.ngxService.stop('getReversalReport');
      }
    )
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  searchTransaction(){
    this.ngxService.start('searchTransaction');
    console.log(this.TransactionSearchForm.controls);
    let params ={
      TransactionType: this.TransactionSearchForm.controls.TransactionType.value,
      AccountNo: this.TransactionSearchForm.controls.AccountNo.value,
      GlAccountNo: this.TransactionSearchForm.controls.GlAccountNo.value,
      Date: this.TransactionSearchForm.controls['TransactionDate'].getRawValue()?.toDateString(),
      // DateTo: this.TransactionSearchForm.controls['DateTo'].getRawValue()?.toDateString(),
    }
    console.log(params);
    this.reversalService.getTransactionNotFoundEthswitch(params).subscribe(
      res => {
         this.dataSource = new MatTableDataSource(res);
         this.dataSource.paginator = this.paginator;
         this.ngxService.stop('searchTransaction');
      },
      error =>{
        this.toaster.error("Please try again");
        this.ngxService.stop('searchTransaction');
      }
    )
  }

  exportExcel(){
    const customerData = this.dataSource.data.map(item =>{
      return {
        LIBAccountNumber: item.debitedAccountNumber,
        ReceiverAccount: item.creditedAccount,
        Amount:item.amount,
        refNo:item.refNo,
        TransactionDate:item.transactionDate
      }
      
    })
    this.excelService.exportAsExcelFile(customerData, 'Customer List');
  }

  exportCSV(){
    const options = {
      fieldSeparator: ',',
      quoteStrings: '"',
      decimalSeparator: '.',
      showLabels: true,
      title: 'User Data',
      useBom: true,
       headers: ['LIBAccountNumber', 'ReceiverAccount', 'Amount', 'refNo', 'TransactionDate'] // Optional headers
    };
    const customerData = this.dataSource.data.map(item =>{
      return {
        LIBAccountNumber: item.debitedAccountNumber,
        ReceiverAccount: item.creditedAccount,
        Amount:item.amount,
        refNo:item.refNo,
        TransactionDate:item.transactionDate
      }});
    new AngularCsv(customerData, 'Customer List', options);

  }

  isWithinFifteenMinutes(transactionDate: Date): boolean {
    let selectedDate = new Date(this.TransactionSearchForm.controls['TransactionDate'].getRawValue()?.toDateString());
    let nextdDate = selectedDate;
    nextdDate =new Date(nextdDate.setDate(nextdDate.getDate() + 1));
    let fifteenMinutesBefore = new Date(nextdDate);
    fifteenMinutesBefore = new Date(fifteenMinutesBefore.setMinutes(fifteenMinutesBefore.getMinutes() - 15));
    if(new Date(transactionDate) >= new Date(fifteenMinutesBefore) && new Date(transactionDate) < new Date(nextdDate)){
       return true;
    }
    else {
       return false;
    }
  }

}
