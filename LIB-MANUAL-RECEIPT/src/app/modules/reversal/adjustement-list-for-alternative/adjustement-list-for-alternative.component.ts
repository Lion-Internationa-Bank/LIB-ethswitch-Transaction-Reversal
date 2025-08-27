import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ReversalService } from '../services/reversal.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { UpdateAcoountNumberComponent } from '../update-acoount-number/update-acoount-number.component';

@Component({
  selector: 'app-adjustement-list-for-alternative',
  templateUrl: './adjustement-list-for-alternative.component.html',
  styleUrls: ['./adjustement-list-for-alternative.component.css']
})
export class AdjustementListForAlternativeComponent implements AfterViewInit {
  displayedColumns: string[] = ['select','accountNumber', 'customerName',  'amount', 'rrn', 'createdAt','Action'];
  dataSource = new MatTableDataSource([]);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  TransactionSearchForm:FormGroup;
  selection = new SelectionModel(true, []);

  constructor(private reversalService :ReversalService,
              private ngxService: NgxUiLoaderService,
              private fb: FormBuilder,
              private toaster:ToastrService,
              private dialog:MatDialog,){
  //  this.getAdjustmentListForFinance();
   this.TransactionSearchForm = fb.group({
    AccountNo:[],
    ReferenceNo:[],
    TransactionDate:[],
    // DateTo:[],
    status:['Pending']
    });
  }

  getAdjustmentListForFinance(){
    this.ngxService.start('getAdjustementListForFinance');
    let params ={
      Status: 'Pending'
    }
    this.reversalService.getSelectedForAdjustement(params).subscribe(
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

 

  searchTransaction(){
    this.ngxService.start('searchTransaction');
    console.log(this.TransactionSearchForm.controls);
    let params ={
      AccountNo: this.TransactionSearchForm.controls.AccountNo.value,
      ReferenceNo: this.TransactionSearchForm.controls.ReferenceNo.value,
      Date: this.TransactionSearchForm.controls['TransactionDate'].getRawValue()?.toDateString(),
      // DateTo: this.TransactionSearchForm.controls['DateTo'].getRawValue()?.toDateString(),
      Status: 'Pending'
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

  checkedTransaction(){
    this.ngxService.start('checkedTransaction');
    let transids = this.selection.selected.map(p => p.id);
    this.reversalService.CheckedPendingTransactionForAdjustment(transids).subscribe( res => {
     this.searchTransaction();
     this.toaster.success("Transaction Checked Successfully");
      this.ngxService.stop('checkedTransaction');
   },
   error =>{
     this.toaster.error("Please try again");
     this.ngxService.stop('checkedTransaction');
   });
  }

  setaccountnumber(trans){
    const dialogRef = this.dialog.open(UpdateAcoountNumberComponent, {
      data: trans,
      width:'350px',
      height:'400px'
    })

    dialogRef.afterClosed().subscribe(result => {
     this.searchTransaction();
    });
  }



}


