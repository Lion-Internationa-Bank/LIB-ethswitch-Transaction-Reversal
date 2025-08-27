import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'app/service/auth.service';
import { ReceiptService } from 'app/service/receipt.service';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'app-editreciept',
  templateUrl: './editreciept.component.html',
  styleUrls: ['./editreciept.component.css']
})
export class EditrecieptComponent {
  @Input() showModal: boolean = false;
  @Input() ReceiptData: any;  // Receipt data passed in
  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();
  private _receiptForm: FormGroup; // Reactive form for receipt
  public get receiptForm(): FormGroup {
    return this._receiptForm;
  }
  constructor(private fb: FormBuilder,
      private authService: AuthService,
        private ngxService: NgxUiLoaderService,
        private toastr: ToastrService,private recieptservice :ReceiptService) {}
        public set receiptForm(value: FormGroup) {
          this._receiptForm = value;
        }
      
ngOnChanges(): void {
if (this.ReceiptData) {
  this.receiptForm = this.fb.group({
    status: [this.ReceiptData?.status || ''],
    updatedBy: [this.authService.getuser()], // Optional, assuming AuthService provides username
    updatedDate: [new Date().toISOString()],
    id: [this.ReceiptData?.id],
    recieptNo: [this.ReceiptData?.recieptNo,],
    customerName: [this.ReceiptData?.customerName, Validators.required],
    account: [this.ReceiptData?.account, Validators.required],
    date: [this.ReceiptData?.date, Validators.required],
    amount: [this.ReceiptData?.amount, Validators.required],
    serviceFee: [this.ReceiptData?.serviceFee, Validators.required],
    vat: [this.ReceiptData?.vat, Validators.required],
    branch: [this.ReceiptData?.branch, Validators.required],
    serviceType: [this.ReceiptData?.serviceType, Validators.required],
    reason: [this.ReceiptData?.reason,],
    createdBy: [this.ReceiptData?.createdBy,],
    tinNo: [this.ReceiptData?.tinNo,],
    address: [this.ReceiptData?.address,],
    phoneNo: [this.ReceiptData?.phoneNo,],
    
    // Borrower, Mortgager, Property Type, and LHC Plate No will still be required

  });
  
}
}

  // Method to handle saving of the edited receipt
  saveReceipt() {
    this.ngxService.start()
    if (this.receiptForm.valid) {
   
      this.receiptForm.value.updatedDate = new Date().toISOString().split('T')[0];
      this.receiptForm.value.updatedBy=this.authService.getuser();

console.log("test",this.receiptForm)
      const updatedAsset = this.receiptForm.value;
      
      // Call the receiptService to update the receipt asset
      this.recieptservice.updateReceipt(updatedAsset,updatedAsset.id).subscribe(
        (response) => {
    
          this.showSuccessMessage('receipt asset updated successfully!');
          this.ngxService.stop()
          // Close the modal after updating
          this.closeModal.emit();
          
          
        },
        (error) => {
          this.ngxService.stop()
          // Handle error response
          console.error('Error updating asset:', error);
          this.showSuccessMessage('Failed to update receipt asset. Please try again.',true);
        }
      );
    }
   
  }
  formatDateForInput(date: string): string {
    const formattedDate = new Date(date);
    return formattedDate.toISOString().split('T')[0];  // Convert to YYYY-MM-DD
  }

  // When the user changes the date, update ReceiptData.date in ISO format
  onDateChange(newDate: string): void {
    // Convert the date back to ISO format
    this.ReceiptData.date = new Date(newDate).toISOString();  // Formats as "2025-01-14T00:00:00Z"
  }
  // Method to cancel the editing and close the modal
  cancel() {
    this.closeModal.emit();
  }


  closeModalHandler(): void {
    this.closeModal.emit();
  }

  showSuccessMessage(message: string, isError: boolean = false): void {
    if (isError) {

      this.toastr.error(message, 'Error', {
        timeOut: 1000,
        positionClass: 'toast-top-right',
        closeButton: true
      });
    } else {
   
      this.toastr.success(message, 'Success', {
        timeOut: 1000,
        positionClass: 'toast-top-right',
        closeButton: true
      });
    }
  }
}

