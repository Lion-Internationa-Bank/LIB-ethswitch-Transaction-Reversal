import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ReversalService } from '../services/reversal.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';
import * as XLSX from 'xlsx';
import { ApiUrlService } from 'app/service/apiurl.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-import-transaction',
  templateUrl: './import-transaction.component.html',
  styleUrls: ['./import-transaction.component.css']
})
export class ImportTransactionComponent implements AfterViewInit{
  displayedColumns: string[] = ['Issuer','Acquirer','Transaction_Date','refnum_F37', 'Amount', 'IsAlreadyExist'];
  dataSource = new MatTableDataSource([]);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  file: any;
  arrayBuffer: any;
  filelist: any;
  totalCount: number;
  ImportErrorExist=false;
  fileslists
  baseUrl='';
  //temlateUrl='';
  TransactionImportForm:FormGroup;
  TransactionReconsileForm:FormGroup;
  TransactionSearchForm:FormGroup;
  constructor(private reversalService :ReversalService,
              private ngxService: NgxUiLoaderService,
              private toaster:ToastrService,
              private apiUrl: ApiUrlService,
              private fb: FormBuilder){
  //  this.getReversalListForAlterative();
  //this.baseUrl = apiUrl.apiReversalTransUrl;
 // this.temlateUrl = this.baseUrl + 'ethiSwich%20reconcilation.xlsx'
    this.TransactionImportForm = fb.group({
      TransactionDate:[,Validators.required],
      });
    this.TransactionReconsileForm = fb.group({
      ReconsileDate:[,Validators.required]
      });
    this.TransactionSearchForm = fb.group({
      SearchDate:[,Validators.required],
      TransactionType:[,Validators.required]
      });
      //console.log(this.dataSource)
  }

  // getReversalListForAlterative(){
  //   this.ngxService.start('getReversalListForAlterative');
  //   let selecterrn =[];
  //   this.reversalService.getListForReversal().subscribe(
  //     res => {
  //       this.reversalService.getSelectedForReversal({}).subscribe(res1 =>{
  //         selecterrn = res1.map(p=>p.rrn);
  //         console.log(selecterrn);
  //         let filtered = res.transactions.filter(x => !selecterrn.includes(x.rrn))
  //         this.dataSource = new MatTableDataSource(filtered);
  //         this.dataSource.paginator = this.paginator;
  //         this.ngxService.stop('getReversalListForAlterative');
  //       }, error => {
  //         this.toaster.error("Please try again");
  //         this.ngxService.stop('getReversalListForAlterative');
  //       })
        
        
  //     }, error => {
  //       this.toaster.error("Please try again");
  //       this.ngxService.stop('getReversalListForAlterative');
  //     }
  //   )
  // }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }
  ImportEthswichTrans(transactionList=[]){
    this.ngxService.start('ImportEthswichTrans')
    this.reversalService.ImportTransaction(transactionList).subscribe(
      res => {
       //console.log(res);
       
        if(res.length > 0){
          if(res.filter(item => item.isAlreadyExist == true).length >0){
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.toaster.error('There is transaction number already imported');
            this.ngxService.stop('ImportEthswichTrans')
          } else {

            // transactionList.forEach(element => {
            //   if(res.filter(item => item.reference == element.Reference).length >0){
            //     element.IsAlreadyExist = true;
            //   }
            // });
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            //this.toaster.error('There are Already Transactions Already Imported');
            this.ngxService.stop('ImportEthswichTrans')
            this.toaster.success('Excel Imported Successfully');
            return;
          }
         
        }
        else if(res.status == 1)
        {
          this.toaster.error(res.message);
          this.ngxService.stop('ImportEthswichTrans')
        }
        this.ngxService.stop('ImportEthswichTrans')
      }, error=>{
        this.ngxService.stop('ImportEthswichTrans')
        this.toaster.error('Could not import the excel');
      }
    )
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  addfile(event) {
    this.file = event.target.files[0];
    if(this.file.type !='application/vnd.ms-excel' && this.file.type !='application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'){
      this.toaster.error('Please select Excel File');
      return;
    }

    if(!this.file.name.includes('A2A')){
       this.toaster.error('Please select A2A Excel File');
      return;
    }
    let fileReader = new FileReader();
    fileReader.readAsArrayBuffer(this.file);
    fileReader.onload = (e) => {
      this.arrayBuffer = fileReader.result;
      var data = new Uint8Array(this.arrayBuffer);
      var arr = new Array();
      for (var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
      var bstr = arr.join("");
      var workbook = XLSX.read(bstr, { type: "binary" });
      var first_sheet_name = workbook.SheetNames[0];

      var worksheet = workbook.Sheets[first_sheet_name];
      var arraylist = XLSX.utils.sheet_to_json(worksheet, { raw: true });
      this.filelist = arraylist;
      this.totalCount = this.filelist.length;
    }
  }

  ImportItem() {
    if (this.filelist != null && this.filelist.length <= 0) {
    }
    else {
      
      // let list =this.filelist.map(p=>{
      //   return{ Reference:String(p.Reference),
      //     Amount:Number(p.Amount),
      //     IsAlreadyExist:false}
      // })
    let validExcel =this.filelist.filter(p=> p.__EMPTY_2);
     if(validExcel && validExcel.length >1){
      this.toaster.error('Please select Valid Excel File');
      return;
     }


      let list =this.filelist.map(p=>{
        if(p.__EMPTY == 'Acquirer' || p.__EMPTY == undefined){
          return;
        }
        if(p.__EMPTY_2){
          return;
        }
        return{ 
          Issuer: p['BANK Reconciliation Report'],
          Acquirer:p.__EMPTY,
          MTI:p.__EMPTY_1,
          Card_Number:p.__EMPTY_2,
          Amount:p.__EMPTY_3,
          Currency:p.__EMPTY_4,
          Transaction_Date1: p.__EMPTY_5,
          Transaction_Date: this.convertDateTimeFormat(p.__EMPTY_5),
          Transaction_Description:p.__EMPTY_7,
          Terminal_ID:p.__EMPTY_8,
          Transaction_Place:p.__EMPTY_9,
          STAN_F11:p.__EMPTY_10,
          Refnum_F37:p.__EMPTY_11,
          Authidresp_F38:p.__EMPTY_12,
          Fe_utrnno:p.__EMPTY_13,
          Bo_utrnno:p.__EMPTY_14,
          IsAlreadyExist:false,
          Status:'pending',
          TransactionDateFrom: this.TransactionImportForm.controls['TransactionDate'].getRawValue()?.toDateString(),
          //TransactionDateTo: new Date(this.TransactionImportForm.controls['DateTo'].getRawValue()),
        }
      });
      console.log(list);
      this.ImportEthswichTrans(list);
      //this.dataSource = new MatTableDataSource(list);
    }
  }

  ShowImportError(){

  }

  ReconsileTransaction(){
    this.ngxService.start('ReconsileTransaction')
    let params ={
      Date: this.TransactionReconsileForm.controls['ReconsileDate'].getRawValue()?.toDateString(),
      //DateTo: this.TransactionImportForm.controls['DateTo'].getRawValue()?.toDateString()
    }
    this.reversalService.ReconsilePendingTransaction(params).subscribe(
      res => {
        this.toaster.success('reconsilation made successfully');
        this.ngxService.stop('ReconsileTransaction')
      }, error => {
        console.log(error);
        this.toaster.error(error?.error?.detail);
        this.ngxService.stop('ReconsileTransaction')
      })
  }

  convertDateTimeFormat(dateTimeString) {
    // Split the date and time
    if(!dateTimeString) return;
    const [datePart, timePart] = dateTimeString.split(' ');
    
    // Split the date part by '.'
    const parts = datePart.split('.');
    
    // Check if the parts array has three elements
    if (parts.length === 3) {
        // Rearrange the parts to 'mm/dd/yyyy HH:MM:SS'
        return `${parts[1]}/${parts[0]}/${parts[2]} ${timePart}`;
    } else {
        this.toaster.error('Invalid date format. Please use dd.mm.yyyy HH:MM:SS.');
    }
}

getImportedTransaction(){
  this.ngxService.start('getImportedTransaction');
  let params ={
    Date: this.TransactionSearchForm.controls['SearchDate'].getRawValue()?.toDateString(),
    //DateTo: this.TransactionImportForm.controls['DateTo'].getRawValue()?.toDateString(),
    TransactionType: this.TransactionSearchForm.controls['TransactionType'].getRawValue(),
    //Status: this.TransactionSearchForm.controls.status.value
    Status: 'Pending'
  }
  this.reversalService.GetImportedTransaction(params).subscribe(
    res => {
       this.dataSource = new MatTableDataSource(res);
       this.dataSource.paginator = this.paginator;
       this.ngxService.stop('getImportedTransaction');
    },
    error =>{
      this.toaster.error("Please try again");
      this.ngxService.stop('getImportedTransaction');
    }
  )
}

}

