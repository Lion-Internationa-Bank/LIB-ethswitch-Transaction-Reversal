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
  selector: 'app-reconsilation-summary-report',
  templateUrl: './reconsilation-summary-report.component.html',
  styleUrls: ['./reconsilation-summary-report.component.css']
})
export class ReconsilationSummaryReportComponent implements AfterViewInit{
  displayedColumns: string[] = ['transactionDate', 'transactionType',  'pendingOnLIBCount', 'pendingOnEthswichCount','successfullyReconsiledCount'];
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
    DateFrom:[],
    DateTo:[],
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  searchReconsilationSummaryReport(){
    this.ngxService.start('searchReconsilationSummaryReport');
    console.log(this.TransactionSearchForm.controls);
    let params ={
      DateFrom: this.TransactionSearchForm.controls['DateFrom'].getRawValue()?.toDateString(),
      DateTo: this.TransactionSearchForm.controls['DateTo'].getRawValue()?.toDateString(),
    }
    console.log(params);
    this.reversalService.ReconsilationSummary(params).subscribe(
      res => {
         this.dataSource = new MatTableDataSource(res);
         this.dataSource.paginator = this.paginator;
         this.ngxService.stop('searchReconsilationSummaryReport');
      },
      error =>{
        this.toaster.error("Please try again");
        this.ngxService.stop('searchReconsilationSummaryReport');
      }
    )
  }

  exportExcel(){
    const customerData = this.dataSource.data.map(item =>{
      return {
        transactionDate: item.transactionDate,
        transactionType: item.transactionType,
        pendingOnLIB:item.pendingOnLIBCount,
        pendingOnEthswich:item.pendingOnEthswichCount,
        successfullyReconsiled:item.successfullyReconsiledCount
       
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
       headers: ['transactionDate', 'transactionType', 'pendingOnLIB', 'pendingOnEthswich',
        'successfullyReconsiled'] // Optional headers
    };
    const customerData = this.dataSource.data.map(item =>{
      return {
        transactionDate: item.transactionDate,
        transactionType: item.transactionType,
        pendingOnLIB:item.pendingOnLIBCount,
        pendingOnEthswich:item.pendingOnEthswichCount,
        successfullyReconsiled:item.successfullyReconsiledCount
      }});
    new AngularCsv(customerData, 'Customer List', options);

  }

}
