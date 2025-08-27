import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ReversalService } from '../services/reversal.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-adjustement-list-for-finance',
  templateUrl: './adjustement-list-for-finance.component.html',
  styleUrls: ['./adjustement-list-for-finance.component.css']
})
export class AdjustementListForFinanceComponent  implements AfterViewInit{
  displayedColumns: string[] = ['select','accountNumber','customerName', 'amount', 'rrn', 'createdAt', 'message','Action'];
  dataSource = new MatTableDataSource([]);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  TransactionSearchForm:FormGroup;
   selection = new SelectionModel(true, []);

  constructor(private reversalService :ReversalService,
              private ngxService: NgxUiLoaderService,
              private fb: FormBuilder,
              private toaster:ToastrService){
  //  this.getAdjustmentListForFinance();
   this.TransactionSearchForm = fb.group({
    AccountNo:[],
    ReferenceNo:[],
    TransactionDate:[],
    // DateTo:[],
    status:['0']
    });
  }

  getAdjustmentListForFinance(){
    this.ngxService.start('getAdjustementListForFinance');
   
    this.reversalService.getSelectedForAdjustement({}).subscribe(
      res => {
         this.dataSource = new MatTableDataSource(res);
         this.dataSource.paginator = this.paginator;
         this.ngxService.stop('getAdjustementListForFinance');
      }, error =>{
        this.toaster.error("Please try again");
        this.ngxService.stop('getAdjustementListForFinance');
      }
    )
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  createtransfere(id){
    this.ngxService.start('createtransfere');
    this.reversalService.createAdjustmentTransaction(id).subscribe(
      res => {
        if(res.status == "1"){
          this.toaster.success("Transaction Reversal Made Successfully!");
        }
        else {
          this.toaster.error("Unable to Made Reversal Successfully!");
        }
        this.searchTransaction();
        this.ngxService.stop('createtransfere');
      }, error =>{
        this.toaster.error("Unable to Made Reversal Successfully!");
        this.ngxService.stop('createtransfere');
         this.searchTransaction();
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
      Status: this.TransactionSearchForm.controls.status.value
    }
    this.reversalService.getSelectedForAdjustement(params).subscribe(
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

  AdjustTransactions(){
    this.ngxService.start('AdjustTransactions');
    let transids = this.selection.selected.map(p => p.id);
    this.reversalService.createBatchAdjustmentTransaction(transids).subscribe( res => {
     this.searchTransaction();
     this.toaster.success("your request send successfully");
      this.ngxService.stop('AdjustTransactions');
   },
   error =>{
     this.toaster.error("Please try again");
     this.ngxService.stop('AdjustTransactions');
     this.searchTransaction();
   });
  }

}

