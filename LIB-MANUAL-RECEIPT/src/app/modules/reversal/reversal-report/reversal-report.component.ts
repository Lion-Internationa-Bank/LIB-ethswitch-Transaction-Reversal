import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ReversalService } from '../services/reversal.service';
import { AngularCsv } from 'angular-csv-ext';
import { ExcelService } from '../services/excel.service';

@Component({
  selector: 'app-reversal-report',
  templateUrl: './reversal-report.component.html',
  styleUrls: ['./reversal-report.component.css']
})
export class ReversalReportComponent implements AfterViewInit{
  displayedColumns: string[] = ['accountNumber', 'receiverAccount', 'amount', 'rrn','newTransaction', 'createdAt','approvedBy','approvedDate','reversedBy','reversalDate'];
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
    AccountNo:[],
    DateFrom:[],
    DateTo:[],
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
      AccountNo: this.TransactionSearchForm.controls.AccountNo.value,
      DateFrom: this.TransactionSearchForm.controls['DateFrom'].getRawValue()?.toDateString(),
      DateTo: this.TransactionSearchForm.controls['DateTo'].getRawValue()?.toDateString(),
    }
    console.log(params);
    this.reversalService.getReversalReport(params).subscribe(
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
        LIBAccountNumber: item.accountNumber,
        ReceiverAccount: item.receiverAccount,
        Amount:item.amount,
        RefNo:item.rrn,
        TransactionId:item.newTransaction,
        Date: item.createdAt,
        CheckedBy:item.approvedBy,
        ApprovedDate:item.approvedDate,
        ReversedBy: item.reversedBy,
        ReversalDate: item.reversalDate
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
       headers: ['LIBAccountNumber', 'ReceiverAccount', 'Amount', 'RefNo',
        'TransactionId','Date','CheckedBy','ApprovedDate', 'ReversedBy', 'ReversalDate'
       ] // Optional headers
    };
    const customerData = this.dataSource.data.map(item =>{
      return {
        LIBAccountNumber: item.accountNumber,
        ReceiverAccount: item.receiverAccount,
        Amount:item.amount,
        RefNo:item.rrn,
        TransactionId:item.newTransaction,
        Date: item.createdAt,
        CheckedBy:item.approvedBy,
        ApprovedDate:item.approvedDate,
        ReversedBy: item.reversedBy,
        ReversalDate: item.reversalDate
      }});
    new AngularCsv(customerData, 'Customer List', options);

  }

}
