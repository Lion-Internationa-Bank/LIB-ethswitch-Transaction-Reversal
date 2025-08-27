import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReversalService } from '../services/reversal.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-update-acoount-number',
  templateUrl: './update-acoount-number.component.html',
  styleUrls: ['./update-acoount-number.component.css']
})
export class UpdateAcoountNumberComponent {
 UpdateAccountForm:FormGroup;

 constructor(private reversalService :ReversalService,
               private dialogRef: MatDialogRef<UpdateAcoountNumberComponent>,
               private ngxService: NgxUiLoaderService,
               private fb: FormBuilder,
                @Inject(MAT_DIALOG_DATA) public data: any,
               private toaster:ToastrService){
    this.UpdateAccountForm = fb.group({
     AccountNo:['',Validators.required],
     ReferenceNo:[data.refNo],
     FullName:['',Validators.required]
     });
   }
  UpdateAccount(){
    this.ngxService.start();
    const trans = {
      id:this.data.id,
      creditedAccount: this.UpdateAccountForm.controls.AccountNo.value.toString(),
      refNo: this.UpdateAccountForm.controls.ReferenceNo.value,
      customerName: this.UpdateAccountForm.controls.FullName.value
    }
    this.reversalService.updateTransactionAccount(trans).subscribe(
      res => {
        this.ngxService.stop();
        this.toaster.success('Account Number Update Successfully');
        this.dialogRef.close();
      },
      error =>{
       this.ngxService.stop();
       this.toaster.error('Can not update Account Number. Please Try again');
      }
    )
  }

    getAccountHolderName(){
    this.ngxService.start('getAccountHolderName');
    const accountNo = this.UpdateAccountForm.controls.AccountNo.value.toString()
    if(accountNo == ''){
       this.UpdateAccountForm.controls.FullName.setValue('');
        this.ngxService.stop('getAccountHolderName');
       return;
    }
    this.reversalService.getAccountHolderName(accountNo).subscribe( res => {
      console.log(res);
      if(res)
       this.UpdateAccountForm.controls.FullName.setValue(res.fullname);
      else  this.UpdateAccountForm.controls.FullName.setValue(''); 
      this.ngxService.stop('getAccountHolderName');
   },
   error =>{
     this.toaster.error("Please try again");
     this.ngxService.stop('getAccountHolderName');
   });
  }

}
