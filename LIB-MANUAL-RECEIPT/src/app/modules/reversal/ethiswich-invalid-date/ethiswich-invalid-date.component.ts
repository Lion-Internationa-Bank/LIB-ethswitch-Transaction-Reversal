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
  selector: 'app-ethiswich-invalid-date',
  templateUrl: './ethiswich-invalid-date.component.html',
  styleUrls: ['./ethiswich-invalid-date.component.css']
})
export class EthiswichInvalidDateComponent implements AfterViewInit{
  displayedColumns: string[] = ['acquirer','issuer', 'amount', 'refnum_F37', 'transaction_Date'];
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
    TransactionDate:[],
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  searchTransaction(){
    this.ngxService.start('searchTransaction');
    console.log(this.TransactionSearchForm.controls);
    let params ={
      Date: this.TransactionSearchForm.controls['TransactionDate'].getRawValue()?.toDateString(),
    }

    this.reversalService.InvalidEthiswichDateTransaction(params).subscribe(
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

}


