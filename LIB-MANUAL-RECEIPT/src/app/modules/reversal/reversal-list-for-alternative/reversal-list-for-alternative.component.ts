import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ReversalService } from '../services/reversal.service';
import { MatPaginator } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as XLSX from 'xlsx';
import { disableDebugTools } from '@angular/platform-browser';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-reversal-list-for-alternative',
  templateUrl: './reversal-list-for-alternative.component.html',
  styleUrls: ['./reversal-list-for-alternative.component.css']
})
export class ReversalListForAlternativeComponent implements AfterViewInit {
  displayedColumns: string[] = ['select','accountNumber', 'receiverAccount', 'amount', 'rrn', 'createdAt'];
  dataSource = new MatTableDataSource([]);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  TransactionSearchForm:FormGroup;
  selection = new SelectionModel(true, []);

  constructor(private reversalService :ReversalService,
              private ngxService: NgxUiLoaderService,
              private fb: FormBuilder,
              private toaster:ToastrService){
  //  this.getReversalListForFinance();
   this.TransactionSearchForm = fb.group({
    AccountNo:[],
    ReferenceNo:[],
    TransactionDate:[],
    // DateTo:[],
    status:['Pending']
    });
  }

  getReversalListForFinance(){
    this.ngxService.start('getReversalListForFinance');
    let params ={
      Status: 'Pending'
    }
    this.reversalService.getSelectedForReversal(params).subscribe(
      res => {
         this.dataSource = new MatTableDataSource(res);
         this.dataSource.paginator = this.paginator;
         this.ngxService.stop('getReversalListForFinance');
      }, error =>{
        this.toaster.error("Please try again");
        this.ngxService.stop('getReversalListForFinance');
      }
    )
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  createtransfere(id){
    this.ngxService.start('createtransfere');
    this.reversalService.createTransaction(id).subscribe(
      res => {
        if(res.status == "1"){
          this.toaster.success("Transaction Reversal Made Successfully!");
        }
        else {
          this.toaster.error("Unable to Made Reversal Successfully!");
        }
        this.getReversalListForFinance();
        this.ngxService.stop('createtransfere');
      }, error =>{
        this.toaster.error("Unable to Made Reversal Successfully!");
        this.ngxService.stop('createtransfere');
      }
    )
  }

  searchTransaction(){
    this.ngxService.start('searchTransaction');
    console.log(this.TransactionSearchForm.controls);
    let params ={
      AccountNo: this.TransactionSearchForm.controls.AccountNo.value,
      ReferenceNo: this.TransactionSearchForm.controls.ReferenceNo.value,
      Date: this.TransactionSearchForm.controls['TransactionDate'].getRawValue()?.toDateString(),
      // DateTo: this.TransactionSearchForm.controls['DateTo'].getRawValue()?.toDateString(),
      //Status: this.TransactionSearchForm.controls.status.value
      Status: 'Pending'
    }
    this.reversalService.getSelectedForReversal(params).subscribe(
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

 

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      console.log(this.selection);
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  checkedTransaction(){
    this.ngxService.start('checkedTransaction');
    let transids = this.selection.selected.map(p => p.id);
    this.reversalService.CheckedPendingTransactionForReversal(transids).subscribe( res => {
     this.searchTransaction();
     this.toaster.success("Transaction Checked Successfully");
      this.ngxService.stop('checkedTransaction');
   },
   error =>{
     this.toaster.error("Please try again");
     this.ngxService.stop('checkedTransaction');
   });
  }

}


