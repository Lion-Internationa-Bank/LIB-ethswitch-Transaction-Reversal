import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ReversalService } from '../services/reversal.service';
import { MatPaginator } from '@angular/material/paginator';
import { AngularCsv } from 'angular-csv-ext';
import { ExcelService } from '../services/excel.service';

@Component({
  selector: 'app-transaction-not-found-lib',
  templateUrl: './transaction-not-found-lib.component.html',
  styleUrls: ['./transaction-not-found-lib.component.css']
})
export class TransactionNotFoundLibComponent implements AfterViewInit{
  displayedColumns: string[] = ['accountNumber', 'receiverAccount', 'amount', 'rrn', 'createdAt', 'ethTransactionDate','bankName'];
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
    this.reversalService.getReversalReport({}).subscribe(
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
    if(params.TransactionType == '1'){
      this.displayedColumns = ['accountNumber','rrn', 'amount',  'createdAt', 'ethTransactionDate','bankName', 'reason'];
    } else {
      this.displayedColumns = ['accountNumber', 'receiverAccount', 'amount', 'rrn', 'createdAt', 'ethTransactionDate','bankName'];
    }
    this.reversalService.getTransactionNotFoundLib(params).subscribe(
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
    let fifteenMinutesAfter = new Date(selectedDate);
    fifteenMinutesAfter = new Date(fifteenMinutesAfter.setMinutes(fifteenMinutesAfter.getMinutes() + 15));
    if(new Date(transactionDate) <= new Date(fifteenMinutesAfter) && new Date(transactionDate) >= new Date(selectedDate)){
       return true;
    }
    else {
       return false;
    }
  }

}

