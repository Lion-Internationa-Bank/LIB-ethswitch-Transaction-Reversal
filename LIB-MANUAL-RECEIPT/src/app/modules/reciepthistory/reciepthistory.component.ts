import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Receipt } from 'app/models/data.model';
import { ReceiptService } from 'app/service/receipt.service';
import { ToastrService } from 'ngx-toastr';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as ExcelJS from 'exceljs';
import { AuthService } from 'app/service/auth.service';

function formatNumber(num) {
  return Number(num).toLocaleString('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
  });
}
@Component({
  selector: 'app-reciepthistory',
  templateUrl: './reciepthistory.component.html',
  styleUrls: ['./reciepthistory.component.css']
})


export class ReciepthistoryComponent {
 receiptForm: FormGroup;
  receipts: Receipt[] = [];
  receipt:Receipt
  selectedReceipt: Receipt | null = null;
  modalVisible: boolean = false;
  searchFilters = {
    recieptNo: '',
    customerName: '',
    startDate: '',
    endDate: '',
    account: '',
    branch: '',
    serviceType:''
  };
  serviceTypes=['cash','account','other']


  constructor(
    private fb: FormBuilder,
    private receiptService: ReceiptService,
    private ngxService: NgxUiLoaderService,
    private toastr: ToastrService,
    public authservice:AuthService
  ) {
    this.receiptForm = this.fb.group({
      id: [0],
      updatedDate: [''],
      updatedBy: [''],
      status: [''],
      serviceFee: [0,Validators.required],
      serviceFee2: [0],
      vat: [0,Validators.required],
      vat2: [0],
      inputing_Branch: [''],
      transaction_Date: ['',],
      account_No: ['',Validators.required],
      amount1: [0,Validators.required],
      phone_No: [''],
      address: [''],
      tinNo: [''],
      debitor_Name: ['',Validators.required],
      paymentMethod: ['',Validators.required],
      refno: [''],
      branch: [''],
      cAccountNo: [''],
      createdBy: [''],
      approvedBy: [''],
      messsageNo: [''],
      paymentNo: [''],
      paymentType: ['']
    });
  }

  ngOnInit(): void {
    
   
    const today = new Date().toISOString().split('T')[0]; // Format: 'YYYY-MM-DD'
    const startDate = this.searchFilters.startDate?.split('T')[0] || today; // Default to today if not provided
    const endDate = this.searchFilters.endDate?.split('T')[0] || today; // Default to today if not provided
    

 
      console.log(startDate,endDate,"endDate")
    this.receiptService.getManual(startDate, endDate).subscribe((receipts) => {
      if (receipts && receipts.length > 0) {
        // If receipts are found, assign them to the local variable
        this.receipts = receipts.filter(t=>t.inputing_Branch==this.authservice.getbranch());
           console.log("Receipts:", this.receipts);
        this.ngxService.stop()
        // You can also apply additional filters here if needed
        // For example, filtering based on inputting branch
        this.filteredReceipts();
      } else {
        this.ngxService.stop()
        // If no receipts are found, handle the case (e.g., show a message)
        this.showSuccessMessage("No receipts found",true)
        console.log("No receipts found for the selected date range.");
        this.receipts = [];  // Clear the receipts list or set it to empty
      }
    });
    
  
  }

  private isSameDate(receiptDate: string, currentDate: Date): boolean {
    const receiptDateOnly = new Date(receiptDate).toISOString().split('T')[0]; // Extract 'YYYY-MM-DD'
    const currentDateOnly = currentDate.toISOString().split('T')[0];           // Extract 'YYYY-MM-DD'
    return receiptDateOnly === currentDateOnly;
  }
  
  loadReceipts(): void {
    this.ngxService.start()
    // Ensure StartDate and EndDate are provided (use today's date if not selected)
    const today = new Date().toISOString().split('T')[0]; // Format: 'YYYY-MM-DD'
    const startDate = this.searchFilters.startDate?.split('T')[0] || today; // Default to today if not provided
    const endDate = this.searchFilters.endDate?.split('T')[0] || today; // Default to today if not provided
    

    if (!this.searchFilters.startDate || !this.searchFilters.endDate) {

      alert('Please provide both start date and end date for the search.');
      this.ngxService.stop()
      return; // Stop further execution if dates are not provided
    }
   const start = new Date(this.searchFilters.startDate);
    const end = new Date(this.searchFilters.endDate);

     start.setHours(0, 0, 0, 0); // Normalize start date to 00:00:00

  end.setHours(23, 59, 59, 999); // Normalize end date to 23:59:59.999


    // Check if the startDate is before the endDate
    if (start > end) {
      alert('Start date cannot be later than end date.');
      this.ngxService.stop()
      return;
      
    }
    // Fetch receipts by the startDate and endDate
    console.log(startDate,endDate,"endDate")
    this.receiptService.getManual(startDate, endDate).subscribe((receipts) => {
      if (receipts && receipts.length > 0) {
        // If receipts are found, assign them to the local variable
        this.receipts = receipts.filter(t=>t.inputing_Branch==this.authservice.getbranch());
        console.log("Receipts:", this.receipts);
        this.ngxService.stop()
        // You can also apply additional filters here if needed
        // For example, filtering based on inputting branch
        this.receipts = this.filteredReceipts();  // Update this.receipts with the filtered data
      } else {
        this.ngxService.stop()
        // If no receipts are found, handle the case (e.g., show a message)
        this.showSuccessMessage("No receipts found",true)
        console.log("No receipts found for the selected date range.");
        this.receipts = [];  // Clear the receipts list or set it to empty
      }
    });
    
  }


  filteredReceipts() {
    return this.receipts.filter(receipt => {
      return (
        (this.searchFilters.serviceType ? receipt.paymentMethod.includes(this.searchFilters.serviceType) : true) &&
      
        (this.searchFilters.recieptNo ? this.pad(receipt.id).includes(this.searchFilters.recieptNo) : true) &&
        (this.searchFilters.customerName ? receipt.debitor_Name.toLowerCase().includes(this.searchFilters.customerName.toLowerCase()) : true) &&
        (this.searchFilters.account ? receipt.account_No.includes(this.searchFilters.account) : true) 
            );
    });
  }
  



  handleDateFormatting(formValue: Receipt): void {
    // Handle the formatting of `date`
    formValue.transaction_Date = this.formatDate(formValue.transaction_Date);
  }

  formatDate(date: string): string {
    if (date) {
      const parsedDate = new Date(date);
      if (!isNaN(parsedDate.getTime())) {
        return parsedDate.toISOString();
      } else {
        return '1970-01-01T00:00:00.000Z';  // Default if invalid
      }
    }
    return '1970-01-01T00:00:00.000Z';  // Default if missing
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

  editReceipt(receipt: any): void { 
    this.receiptForm.patchValue(receipt);  // Populate form with selected receipt data
}

openReceiptModal(receipt: Receipt) {
  this.selectedReceipt = { ...receipt };  // Use a shallow copy to prevent unwanted changes
console .log("receipt",receipt);
  this.modalVisible = true;  
 
    if (this.selectedReceipt.id !== undefined) {
        this.modalVisible = true;
      this.loadReceipts(); 
    
    }
}

closeReceiptModal(): void {
    this.modalVisible = false;  // Hide the modal
    this.selectedReceipt = null;  // Clear the selected receipt
    this.loadReceipts();
   
}

deleteReceipt(id: number,reciept:Receipt): void {
    this.ngxService.start();

   reciept.status="1";
  
   reciept.updatedDate = new Date().toISOString().split('T')[0];

   reciept.updatedBy=this.authservice.getuser();
    // Step 1: Make the delete API call to the backend
    this.receiptService.updateReceipt(reciept,id).subscribe({
        next: (response) => {
            // Step 2: On success, remove the receipt from the local array
            this.receipts = this.receipts.filter(receipt => receipt.id !== id);

            // Step 3: Refresh the data lists if needed
            this.loadReceipts();  // Reload the receipts list after deletion
            this.ngxService.stop();

            // Step 4: Show a success message to the user
            this.showSuccessMessage('Receipt deleted successfully.', false);
        },
        error: (error) => {
            // Step 5: Handle errors if any
            console.error('Error deleting receipt:', error);

            this.ngxService.stop();
            // Step 6: Show an error message
            this.showSuccessMessage('Error deleting receipt.', true);
        }
    });
}

exportToExcel() {
  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet('Receipts');

  worksheet.columns = [
    { header: 'No', key: 'no' },
    { header: 'Receipt No', key: 'receiptNo' },
    { header: 'Customer Name', key: 'customerName' },
    { header: 'Date', key: 'date' },
    { header: 'Amount', key: 'amount' },
    { header: 'Service Fee', key: 'serviceFee' },
    { header: 'VAT', key: 'vat' },
    { header: 'Branch', key: 'branch' },
    { header: 'Service Type', key: 'serviceType' },
    { header: 'Reason', key: 'reason' },
    { header: 'TIN No', key: 'tinNo' },
    { header: 'Phone No', key: 'phoneNo' }
  ];

  this.filteredReceipts().forEach((receipt, index) => {
    worksheet.addRow({
      no: index + 1,
      receiptNo: this.pad(receipt.id),
      customerName: receipt.debitor_Name,
      date: receipt.transaction_Date,
      amount: receipt.amount1,
      serviceFee: receipt.serviceFee,
      vat: receipt.vat,
      branch: receipt.branch,
      serviceType: receipt.paymentMethod,
      reason: receipt.messsageNo,
      tinNo: receipt.tinNo,
      phoneNo: receipt.phone_No
    });
  });

  workbook.xlsx.writeBuffer().then(buffer => {
    const blob = new Blob([buffer], { type: 'application/octet-stream' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'Receipts.xlsx';
    link.click();
  });
}

exportToPDF() {
  const receipts = this.filteredReceipts();
  if (receipts.length === 0) {
    return;
  }

  let content = `
    <html>
    <head>
      <style>
        body {
          font-family: Arial, sans-serif;
        }
        .title {
          font-size: 20px;
          font-weight: bold;
          margin-bottom: 10px;
        }
        table {
          width: 100%;
          border-collapse: collapse;
          margin-top: 20px;
        }
        th, td {
          border: 1px solid #dddddd;
          text-align: left;
          padding: 8px;
        }
        th {
          background-color: #f2f2f2;
        }
      </style>
    </head>
    <body>
      <div class="title">Receipt Report</div>
      <table>
        <thead>
          <tr>
            <th>No</th>
            <th>Receipt No</th>
            <th>Customer Name</th>
            <th>Date</th>
            <th>Amount</th>
            <th>Service Fee</th>
            <th>VAT</th>
            <th>Branch</th>
            <th>Service Type</th>
            <th>Reason</th>
            <th>TIN No</th>
            <th>Phone No</th>
          </tr>
        </thead>
        <tbody>
  `;

  receipts.forEach((receipt, index) => {
    content += `
      <tr>
        <td>${index + 1}</td>
        <td>${this.pad(receipt.id)}</td>
        <td>${receipt.debitor_Name}</td>
        <td>${receipt.transaction_Date}</td>
        <td>${receipt.amount1}</td>
        <td>${receipt.serviceFee}</td>
        <td>${receipt.vat}</td>
        <td>${receipt.branch}</td>
        <td>${receipt.paymentMethod}</td>
        <td>${receipt.messsageNo}</td>
        <td>${receipt.tinNo}</td>
        <td>${receipt.phone_No}</td>
      </tr>
    `;
  });

  content += `
        </tbody>
      </table>
    </body>
    </html>
  `;

  const printWindow = window.open('', '', 'height=500,width=800');
  printWindow.document.write(content);
  printWindow.document.close();
  printWindow.print();
}


printReceipt(receipt) {
  this.receipt = receipt; // Assuming 'transaction' contains all necessary transaction data.
  const receiptContent = document.getElementById('receiptContent');

  if (receiptContent) {
      // Preload the logo image
      const logoImage = new Image();
      logoImage.src = 'assets/img/LIBLogo2.jpg'; // URL of the image to preload

      logoImage.onload = () => {
          // Create an iframe
          const printWindow = document.createElement('iframe');
          printWindow.style.display = 'none';
          document.body.appendChild(printWindow);
          
          printWindow.contentWindow!.document.open();
          printWindow.contentWindow!.document.write(`
              <html>
                  <head>
                      <title>Receipt</title>
                      <style>                  .top {
  text-align: right; /* Aligns content to the left */
  margin-bottom: 10px; /* Space between date/receipt no and the receipt content */
  font-family: 'Times New Roman', Times, serif;
  font-size: 12px;
  color: #52829e;
}
.top p {
  margin: 0;
  padding: 0;
}
                          body {
                              font-family: 'Times New Roman', Times, serif;
                              margin: 10px 5px; /* Reduced left and right margins */
                              padding: 5px; /* Reduced padding */
                              color: #333333; /* Set text color to #333333 */
                   
                              font-size: 12px; /* Smaller font size */
                                 background: url('assets/img/LIBLogo2.jpg') no-repeat center center; /* Directly use the image URL */
                    background-size: cover;
                     
                          }

.topsection{
    display: flex;
            justify-content: space-between;
            border-bottom: 2px solid #52829e;
            padding: 1px 0;
            margin-bottom: 15px;
}

.reciepts{
                 border: 1px solid #52829e; /* Border around the whole page */
}

                          .receipt-header {
                          
                              display: flex;
                                  align-items: center; /* Vertically centers the title with the logo */
                              justify-content: space-between; /* Align image and title on left and address on right */
                              border-bottom: 2px solid #52829e; /* Header line in specified color */
                              padding: 1px 0; /* Reduced padding */
                              margin-bottom: 15px; /* Reduced margin bottom */
                          }
                          .logo-img {
                              width: 30%;
                              height: auto;
                          }
                              .logo-title-container {
    display: flex;
    align-items: center; /* Vertically center the logo and title */
}
                          .slip-title {
                            font-weight:bold;
                              font-size: 20px; /* Smaller title size */
                              color: #52829e;; /* Title color changed to #333333 */
                              text-align: left; /* Align title to the left */
                              margin-bottom: 5px;
                      padding:0px;
                               font-family: "Copperplate", "Georgia", serif; /* Bold, distinct serif fonts */

                          }
                                    .address p{
                                     padding-bottom: 5px; /* Spacing below the title */
  
                               margin:0px;margin-top:5px;
                                 }
                          .address {
                              text-align: right; /* Align address to the right */
                              width: 100%; /* Set width for address */
                      padding: 0px; /* Reduced padding */
                            margin:0px;
                                 }
                            .address div {
                              padding: 5px 0; /* Reduced padding between entries */
                              display: flex; /* Use flex to align items */
                                        margin-right:60px; /* Reduced margin */
                     
                          }
                              .add{
                                 text-align: center; /* Align address to the right */
                                   padding: 0px; /* Reduced padding */
                            margin:0px;
                               color:#52829e;
                       font-weight: bold; /* Smaller header size */
                       
                              }
                          h3 {
                              color: #333333; /* Title color changed to #333333 */
                              margin: 10px 0; /* Reduced margin */
                              text-align: center; /* Center align the title */
                              border-bottom: 1px solid #52829e; /* Horizontal line below the title in specified color */
                              padding-bottom: 5px; /* Space below the title */
                              font-size: 16px; /* Smaller header size */                               
                          }
                          .transaction-info {
                              margin: 0; /* Remove margin above and below */
                              border-bottom: 1px solid #52829e; /* Underline for Transaction Information in specified color */
                              padding-bottom: 5px; /* Spacing below the title */

                               text-align: left;
            width: 180%; gap: 4px;
                          }
                          .transaction-info div {
                              padding: 5px 0; /* Reduced padding between entries */
                              display: flex; /* Use flex to align items */
                                justify-content: flex-start; /* Space between title and value */
                         gap:14px;
                          }
                          .transaction-details {
                              margin-top: 0px; /* Reduced margin */
                              border-collapse: collapse; /* Collapse table borders */
                              width: 100%; /* Full width for table */
                              font-size: 12px; /* Smaller font size for table */
                             
                          }
                          .transaction-details th, .transaction-details td {
                              border: 1px solid #52829e; /* Border for table cells in specified color */
                              padding: 4px; /* Smaller padding for cells */
                           }
                          .transaction-details th {
                              background-color: #f2f2f2; /* Light gray background for headers */
                              color: #333333; /* Header text color */
                               text-align: center;
                          }
                          .transaction-details td.no-border {
                              border: none; /* Remove border for specific cells */
                       text-align: right; /* 
                          }
                          .amount-row {
                              display: flex;
                              justify-content: space-between;
                              padding: 5px 0; /* Padding for spacing */
                              margin-top: 5px; /* Margin to separate rows */
                          }
                          .footer {
                              margin-top: 50px; /* Reduced footer margin */
                              text-align: center;
                              color: #80353c;; /* Footer text color (darker gray) */
                              font-size: 10px; /* Smaller footer size */
                          }
                                 .footer2 {
                              margin-top: 150px; /* Reduced footer margin */
                              text-align: right;
                              color: #52829e;; /* Footer text color (darker gray) */
                              font-size: 10px; /* Smaller footer size */
                          }
                          .qr-code {
                              width: 10%;
                              height: auto;
                          }.amount-in-words { margin-top:10px;}
                              .amount-in-words span{
                             display: inline-block; /* Make span behave like a block for width control */
    border-bottom: 1px solid #52829e; /* Horizontal line below the text */
    padding-right: 40px; /* Extend the line further to the right */
    white-space: nowrap; /* Prevent wrapping if text is too long */    

       margin-top:10px;
                              }
    .background-container {
                          position: relative;
                          width: 100%;
                          height: auto;
                          overflow: hidden;
                      }
                          .style{
                   font-style: italic;
                          font-family:Snell Roundhand, cursive;
                           color: #80353c;}
.background-image {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 0;
    opacity: 0.06; /* Adjust opacity for a watermark effect */
    object-fit: cover; /* This makes the image cover the entire container */
    background-size: cover; /* Ensures the background image covers fully */
    background-position: center; /* Center the image within the container */
}

.red{ color: #80353c;; /* Footer text color (darker gray) */
          padding: 0px; /* Reduced padding */
                            margin:0px;
                                                 
}
                            .blue{ color: #52829e;; /* Footer text color (darker gray) */
          padding: 0px; /* Reduced padding */
                            margin:0px;
                                                 
}
                              .space{
                              margin-top:20px}
                          .field-row {
                              display: flex;
                              justify-content: space-between; /* Align fields in row */
                              margin-top: 5px; /* Reduced margin between fields */
                          }
                          .field-label {
                              color: #333333; /* Label color changed to #333333 */
                              margin-right: 10px; /* Space between label and line */
                          }
                          .field-line {
                              border-bottom: 1px solid #52829e; /* Line to write data in specified color */
                              width: 80%; /* Width of the line */
                              flex-grow: 1; /* Allow line to grow */
                          }
                              .center{
                               text-align: center;
                              }

                              .footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-family: 'Times New Roman', Times, serif;
    font-size: 14px;
    margin: 10px 0;
   
}

.footer-left, .footer-right {
    flex: 1;
}

.footer-center {
    flex: 1;
    text-align: center;
}
    .align{
       text-align: right;}

.stamp-image {
    max-width: 100px; /* Adjust size as needed */
    height: auto;
}

.contact-info {
    font-family: 'Times New Roman', Times, serif;
    font-size: 10px;
margin-top:10%;
       color: #52829e;
}

.contact-info a {
  
    color: #52829e;
    text-decoration: none;
}

                      </style>
                  </head>  

                  <body>    <div class="top">
                        <p>Date:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;${receipt.transaction_Date}</p>
                                    <p>Receipt  No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;${this.pad(receipt.id)}</p>
                                       </div>

                    <div class="reciepts"> 
                  <div class="background-container">
                      <img src="assets/img/LIBLogo2.jpg" class="background-image" alt="Background Logo" />
                 
                      <div class="receipt">
                          <div class="receipt-header">
                            <div class="logo-title-container">
                                  <img src="${logoImage.src}" alt="Lion International Bank Logo" class="logo-img" />
                                  <span class="slip-title"> 
                                   <p class="red">አንበሳ ኢንተርናሽናል ባንክ አ.ማ</p>Lion International Bank S.C</span>
                                        
                              </div>
                             
                          </div>
          
                          <h3>Value Add Tax Cash Sales  Reciept  / የተ.እ.ታ የእጅ በእጅ ሽያጭ  ደረሰኝ</h3>
                          <div class="topsection">
                      
                            <div class="address">
                                 <div><span>Lion International Bank S.C</span></div>
                                           <div><span>TIN No:</span>&nbsp;&nbsp;&nbsp;<span><u> 0003229535</u> </span></div>
                                              <div><span> Address:</span><span><u> Addis Ababa, Ath. Haile G/Selassie Avenue.Lex Plaza Bldg.</u></span></div>
                                                 <div><span>VAT Reg. Date:</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span><u>10 December 2010</u></span></div> 
                                 <div><span>VAT Reg. No:</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><u> 3118750003</u></span></div>
                                    <div><span>Fax:</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><u> (+251) 11 662 59 99</span></div></u>
                                 <div><span>Tel:</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><u> (+251) 11 662 60 00/60</span></div></u>
                                <div><span>P.O.Box:</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><u> 27026/1000 Addis Ababa</span></div></u>
                              
                            
                         
 </div>   
 
     <div class="transaction-info">
                              <div>
                                  <span>Payer's Name / የከፋይ ሥም :</span>
                                  <span><u>${ receipt.debitor_Name||' '}</u></span>
                              </div>
                                  <div>
                                  <span>Payer's TIN No/ የግብር ከፋይ መለያ ቁጥር :</span>
                                  <span><u>${ receipt.tin_No||''}</u></span>
                              </div>
                                          <div>
                                  <span>Payer's Adress/ የግብር ከፋይ አድራሻ:<u> ${ receipt.address||''} &nbsp;&nbsp;</u> </span>
                                  
                                         <span>Region / ክልል  :<u>${''} &nbsp;&nbsp;</u></span>
                                         <span>Subcity/ክ/ከተማ  :<u>${ ''} &nbsp;&nbsp;</u></span>
                                         <span>Woreda/ ወረዳ  :<u>${''} &nbsp;&nbsp;</u></span>
                              </div>
                                 <div>
                                  <span>Payer's Vat Registration NO / የከፋይ የተ.እ..ታ ምዝገባ ቁጥር :</span>
                                  <span><u>${ ''}</u></span>
                              </div>
                               
                         <div>
    <span>Payer Bank Acc. No / የባንክ ሂሳብ ቁጥር:</span>
    <span><u> ${this.maskAccountNumber(receipt.account_No)} </u></span>
</div>

                               <div>
                                  <span> National Id No/ብሄራዊ መታወቂያ ቁጥር :</span>
                                  <span><u></u></span>
                              </div>
          <div>
                                  <span>Payer's Phone No/የከፋይ ስልክ ቁ.:</span>
                                  <span><u>${receipt.phone_No||''}</u></span>
                              </div>
                    
                              <div>
                                  <span>Inputing Branch   / ቅርንጫፍ  :</span>
                                  <span><u>${receipt.inputing_Branch || 'Unknown'} </u></span>
                              </div>
 
                            
                          </div>
 </div>
                          <table class="transaction-details">
                              <thead>
                                  <tr>
                                      <th colspan="3" class="center"><h3>Transaction Details / የአገልግሎት ዝርዝር</h3></th>
                                  </tr>
                                  <tr>
                                      <th>Refrence Number/የአገልግሎት ቁጥር</th>
                                      <th>Transaction Type/የአገልግሎት አይነት</th>
                                      <th>Transaction Amount (Birr) /የገንዘብ መጠን(ብር)</th>
                                  </tr>
                              </thead>
                              <tbody>
                                  <tr>
                                      <td>${receipt.receiptNo || 'Unknown'}</td>
                                      <td>${receipt.paymentType || 'Unknown'}</td>
                                      <td class="align">${formatNumber(receipt.amount1 )|| 'Unknown'} </td>
                                  </tr>
                                  <tr>
                                      <td colspan="2" class="no-border">Service Fee / የአገልግሎት ክፍያ</td>
                                      <td class="align">${formatNumber(receipt.serviceFee) || '0.00'} </td>
                                  </tr>
                                  <tr>
                                      <td colspan="2" class="no-border">VAT (15%) / ተ.እ.ታ (15%)</td>
                                      <td class="align">${formatNumber(receipt.vat) || '0.00'}  </td>
                                  </tr>
                           
                                
                                  <tr>
                                      <td colspan="2" class="no-border"> Grand Total Including VAT/ጠ.ድምር ተ.እ.ታ ጨምሮ</td>
                                      <td class="align">${formatNumber(receipt.amount1 +receipt.vat +receipt.serviceFee) || '0.00'}</td>
                                  </tr>
                              </tbody>
                          </table>
          <div class="space">   <div class="amount-in-words">
                         Amount in Words / የገንዘቡ ልክ በፊደል :
                            <span>${this.numberToWords(receipt.amount1+receipt.vat +receipt.serviceFee)|| 'N/A'} </span>
                        </div>
                    
                        <div class="amount-in-words">
                         Payment Mode / የክፍያ ሁኔታ :
                            <span>${receipt.paymentMethod || 'Account'}</span>
                        </div>
          </div>
                       
                        
                         <div class="footer">
    <div class="footer-right">
       <p>
            <p class="blue">የስኬትዎ አጋር !</p>
            <p class="red">KEY TO SUCCESS!</p>
        </p>
          
    </div>
    
    <div class="footer-center">
        <img src="assets/img/pngs.png" alt="Bank Stamp" class="stamp-image">

    </div>

    <div class="footer-left">
     <p>Thank you for banking with us!</p>
                               <p>Please retain this receipt for your records.</p>  
                                                      <p>Lion International Bank S.C</p>
                            
        
       
    </div>
</div>



                      </div><div class="contact-info">
    <p>
        ● SWIFT CODE: LIBSETAA&nbsp;&nbsp;
        ● Website: <a href="http://www.anbesabank.com">www.anbesabank.com</a>&nbsp;&nbsp;
        ● Telegram: <a href="https://t.me/LionBankSC">https://t.me/LionBankSC</a>&nbsp;&nbsp;
        ● E-mail: <a href="mailto:info@anbesabank.com">info@anbesabank.com</a>
    </p>
</div></div>
              </div>    </body>
              </html>
          `);
          printWindow.contentWindow!.document.close();
          printWindow.contentWindow!.focus();
          printWindow.contentWindow!.print();
          printWindow.contentWindow!.close();
      };
  }
  // In your component.ts file


}
// printReceipt(receipt) {
//   this.receipt = receipt; // Assuming 'transaction' contains all necessary transaction data.
//   const receiptContent = document.getElementById('receiptContent');

//   if (receiptContent) {
//       // Preload the logo image
//       const logoImage = new Image();
//       logoImage.src = 'assets/img/LIBLogo2.jpg'; // URL of the image to preload

//       logoImage.onload = () => {
//           // Create an iframe
//           const printWindow = document.createElement('iframe');
//           printWindow.style.display = 'none';
//           document.body.appendChild(printWindow);
          
//           printWindow.contentWindow!.document.open();
//           printWindow.contentWindow!.document.write(`
//               <html>
//                   <head>
//                       <title>Receipt</title>
//                       <style>
//                           body {
//                               font-family: 'Times New Roman', Times, serif;
//                               margin: 10px 5px; /* Reduced left and right margins */
//                               padding: 5px; /* Reduced padding */
//                               color: #333333; /* Set text color to #333333 */
//                               border: 1px solid #52829e; /* Border around the whole page */
//                               font-size: 12px; /* Smaller font size */
//                                  background: url('assets/img/LIBLogo2.jpg') no-repeat center center; /* Directly use the image URL */
//                           background-size: cover;
                     
//                           }
                    
//                           .receipt-header {
                          
//                               display: flex;
//                                   align-items: center; /* Vertically centers the title with the logo */
//                               justify-content: space-between; /* Align image and title on left and address on right */
//                               border-bottom: 2px solid #52829e; /* Header line in specified color */
//                               padding: 1px 0; /* Reduced padding */
//                               margin-bottom: 15px; /* Reduced margin bottom */
//                           }
//                           .logo-img {
//                               width: 30%;
//                               height: auto;
//                           }
//                               .logo-title-container {
//     display: flex;
//     align-items: center; /* Vertically center the logo and title */
// }
//                           .slip-title {
//                             font-weight:bold;
//                               font-size: 20px; /* Smaller title size */
//                               color: #52829e;; /* Title color changed to #333333 */
//                               text-align: left; /* Align title to the left */
//                               margin-bottom: 5px;
//                       padding:0px;
//                                font-family: "Copperplate", "Georgia", serif; /* Bold, distinct serif fonts */

//                           }
//                                     .address p{
                                    
//                           padding: 0px; /* Reduced padding */
//                             margin:0px;margin-top:5px;
//                                  }
//                           .address {
//                               text-align: right; /* Align address to the right */
//                               width: 30%; /* Set width for address */
//                       padding: 0px; /* Reduced padding */
//                             margin:0px;
//                                  }
//                               .add{
//                                  text-align: center; /* Align address to the right */
//                                    padding: 0px; /* Reduced padding */
//                             margin:0px;
//                                color:#52829e;
//                        font-weight: bold; /* Smaller header size */
                       
//                               }
//                           h3 {
//                               color: #333333; /* Title color changed to #333333 */
//                               margin: 10px 0; /* Reduced margin */
//                               text-align: center; /* Center align the title */
//                               border-bottom: 1px solid #52829e; /* Horizontal line below the title in specified color */
//                               padding-bottom: 5px; /* Space below the title */
//                               font-size: 16px; /* Smaller header size */                               
//                           }
//                           .transaction-info {
//                               margin: 0; /* Remove margin above and below */
//                               border-bottom: 1px solid #52829e; /* Underline for Transaction Information in specified color */
//                               padding-bottom: 5px; /* Spacing below the title */
//                           }
//                           .transaction-info div {
//                               padding: 5px 0; /* Reduced padding between entries */
//                               display: flex; /* Use flex to align items */
//                               justify-content: space-between; /* Space between title and value */
//                              margin-right:200px; /* Reduced margin */
                             
//                           }
//                           .transaction-details {
//                               margin-top: 0px; /* Reduced margin */
//                               border-collapse: collapse; /* Collapse table borders */
//                               width: 100%; /* Full width for table */
//                               font-size: 12px; /* Smaller font size for table */
                             
//                           }
//                           .transaction-details th, .transaction-details td {
//                               border: 1px solid #52829e; /* Border for table cells in specified color */
//                               padding: 4px; /* Smaller padding for cells */
//                            }
//                           .transaction-details th {
//                               background-color: #f2f2f2; /* Light gray background for headers */
//                               color: #333333; /* Header text color */
//                                text-align: center;
//                           }
//                           .transaction-details td.no-border {
//                               border: none; /* Remove border for specific cells */
//                        text-align: right; /* 
//                           }
//                           .amount-row {
//                               display: flex;
//                               justify-content: space-between;
//                               padding: 5px 0; /* Padding for spacing */
//                               margin-top: 5px; /* Margin to separate rows */
//                           }
//                           .footer {
//                               margin-top: 70px; /* Reduced footer margin */
//                               text-align: center;
//                               color: #80353c;; /* Footer text color (darker gray) */
//                               font-size: 10px; /* Smaller footer size */
//                           }
//                                  .footer2 {
//                               margin-top: 170px; /* Reduced footer margin */
//                               text-align: right;
//                               color: #52829e;; /* Footer text color (darker gray) */
//                               font-size: 10px; /* Smaller footer size */
//                           }
//                           .qr-code {
//                               width: 10%;
//                               height: auto;
//                           }.amount-in-words { margin-top:10px;}
//                               .amount-in-words span{
//                              display: inline-block; /* Make span behave like a block for width control */
//     border-bottom: 1px solid #52829e; /* Horizontal line below the text */
//     padding-right: 40px; /* Extend the line further to the right */
//     white-space: nowrap; /* Prevent wrapping if text is too long */    

//        margin-top:10px;
//                               }
//     .background-container {
//                           position: relative;
//                           width: 100%;
//                           height: 100%;
//                           overflow: hidden;
//                       }
//                           .style{
//                    font-style: italic;
//                           font-family:Snell Roundhand, cursive;
//                            color: #80353c;}
// .background-image {
//     position: absolute;
//     top: 0;
//     left: 0;
//     width: 100%;
//     height: 100%;
//     z-index: 0;
//     opacity: 0.06; /* Adjust opacity for a watermark effect */
//     object-fit: cover; /* This makes the image cover the entire container */
//     background-size: cover; /* Ensures the background image covers fully */
//     background-position: center; /* Center the image within the container */
// }

// .red{ color: #80353c;; /* Footer text color (darker gray) */
//           padding: 0px; /* Reduced padding */
//                             margin:0px;
                                                 
// }
//                             .blue{ color: #52829e;; /* Footer text color (darker gray) */
//           padding: 0px; /* Reduced padding */
//                             margin:0px;
                                                 
// }
//                               .space{
//                               margin-top:20px}
//                           .field-row {
//                               display: flex;
//                               justify-content: space-between; /* Align fields in row */
//                               margin-top: 5px; /* Reduced margin between fields */
//                           }
//                           .field-label {
//                               color: #333333; /* Label color changed to #333333 */
//                               margin-right: 10px; /* Space between label and line */
//                           }
//                           .field-line {
//                               border-bottom: 1px solid #52829e; /* Line to write data in specified color */
//                               width: 80%; /* Width of the line */
//                               flex-grow: 1; /* Allow line to grow */
//                           }
//                               .center{
//                                text-align: center;
//                               }

//                               .footer {
//     display: flex;
//     justify-content: space-between;
//     align-items: center;
//     font-family: 'Times New Roman', Times, serif;
//     font-size: 14px;
//     margin: 10px 0;
   
// }

// .footer-left, .footer-right {
//     flex: 1;
// }

// .footer-center {
//     flex: 1;
//     text-align: center;
// }
//     .align{
//        text-align: right;}

// .stamp-image {
//     max-width: 100px; /* Adjust size as needed */
//     height: auto;
// }

// .contact-info {
//     font-family: 'Times New Roman', Times, serif;
//     font-size: 10px;
// margin-top:22%;
//        color: #52829e;
// }

// .contact-info a {
  
//     color: #52829e;
//     text-decoration: none;
// }

//                       </style>
//                   </head>
//                   <body>
//                   <div class="background-container">
//                       <img src="assets/img/LIBLogo2.jpg" class="background-image" alt="Background Logo" />
                 
//                       <div class="receipt">
//                           <div class="receipt-header">
//                             <div class="logo-title-container">
//                                   <img src="${logoImage.src}" alt="Lion International Bank Logo" class="logo-img" />
//                                   <span class="slip-title"> 
//                                    <p class="red">አንበሳ ኢንተርናሽናል ባንክ አ.ማ</p>Lion International Bank S.C</span>
                                        
//                               </div>
//                               <div class="address">
//                                   <p class="add">Lion International Bank S.C</p>
//                                             <p >TIN No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 0003229535 </p>
//                                     <p>VAT Reg. No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3118750003</p>
//                                   <p>VAT Reg. Date:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 10 December 2010</p>
//                                 <p>Date:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;${receipt.transaction_Date}</p>
//                                     <p>Receipt  No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;${this.pad(receipt.id)}</p>
//  </div>
//                           </div>
          
//                           <h3>Value Add Tax Cash Sales Electronic Reciept  / የተ.እ.ታ የእጅ በእጅ ሽያጭ ኤሌክትሮኒክ ደረሰኝ</h3>
//                           <div class="transaction-info">
//                               <div>
//                                   <span>Payer's Name / የከፋይ ሥም :</span>
//                                   <span>${ receipt.debitor_Name||' '}</span>
//                               </div>
//                                   <div>
//                                   <span>Payer's TIN No/ የግብር ከፋይ መለያ ቁጥር :</span>
//                                   <span>${ receipt.tin_No||''}</span>
//                               </div>
//                                  <div>
//                                   <span>Payer's Vat Registration NO / የከፋይ የተ.እ..ታ ምዝገባ ቁጥር :</span>
//                                   <span>${ ''}</span>
//                               </div>
                               
//                          <div>
//     <span>Payer Bank Acc. No / የባንክ ሂሳብ ቁጥር:</span>
//     <span> ${this.maskAccountNumber(receipt.account_No)} </span>
// </div>

//                                <div>
//                                   <span> National Id No/ብሄራዊ መታወቂያ ቁጥር :</span>
//                                   <span>${receipt.address||''}</span>
//                               </div>
//           <div>
//                                   <span>Payer's Phone No/የከፋይ ስልክ ቁ.:</span>
//                                   <span>${receipt.phone_No||''}</span>
//                               </div>
                    
//                               <div>
//                                   <span>Inputing Branch   / ቅርንጫፍ  :</span>
//                                   <span>${receipt.inputing_Branch || 'Unknown'} </span>
//                               </div>
                  
                            
//                           </div>
          
//                           <table class="transaction-details">
//                               <thead>
//                                   <tr>
//                                       <th colspan="3" class="center"><h3>Transaction Details / የአገልግሎት ዝርዝር</h3></th>
//                                   </tr>
//                                   <tr>
//                                       <th>Refrence Number/የአገልግሎት ቁጥር</th>
//                                       <th>Transaction Type/የአገልግሎት አይነት</th>
//                                       <th>Transaction Amount (Birr) /የገንዘብ መጠን(ብር)</th>
//                                   </tr>
//                               </thead>
//                               <tbody>
//                                   <tr>
//                                       <td>${receipt.receiptNo || 'Unknown'}</td>
//                                       <td>${receipt.paymentType || 'Unknown'}</td>
//                                       <td class="align">${formatNumber(receipt.amount1 )|| 'Unknown'} </td>
//                                   </tr>
//                                   <tr>
//                                       <td colspan="2" class="no-border">Service Fee / የአገልግሎት ክፍያ</td>
//                                       <td class="align">${formatNumber(receipt.serviceFee) || '0.00'} </td>
//                                   </tr>
//                                   <tr>
//                                       <td colspan="2" class="no-border">VAT (15%) / ተ.እ.ታ (15%)</td>
//                                       <td class="align">${formatNumber(receipt.vat) || '0.00'}  </td>
//                                   </tr>
                                  
//                                   ${receipt.paymentType == 'Derash' ? `
//                                     <tr>
//                                         <td colspan="2" class="no-border">Service Fee 2 / የአገልግሎት ክፍያ</td>
//                                         <td class="align">${formatNumber(receipt.serviceFee2) || '0.00'}</td>
//                                     </tr>
//                                     <tr>
//                                         <td colspan="2" class="no-border">VAT 2(15%) / ተ.እ.ታ (15%)</td>
//                                         <td class="align">${formatNumber(receipt.vat2) || '0.00'}</td>
//                                     </tr>
//                                 ` : ``}
                                
//                                   <tr>
//                                       <td colspan="2" class="no-border"> Grand Total Including VAT/ጠ.ድምር ተ.እ.ታ ጨምሮ</td>
//                                       <td class="align">${formatNumber(receipt.amount1 +receipt.vat +receipt.serviceFee +receipt.vat2 +receipt.serviceFee2) || '0.00'}</td>
//                                   </tr>
//                               </tbody>
//                           </table>
//           <div class="space">   <div class="amount-in-words">
//                          Amount in Words / የገንዘቡ ልክ በፊደል :
//                             <span>${this.numberToWords(receipt.amount1 +receipt.vat +receipt.serviceFee+receipt.vat2 +receipt.serviceFee2)|| 'N/A'} </span>
//                         </div>
                    
//                         <div class="amount-in-words">
//                          Payment Mode / የክፍያ ሁኔታ :
//                             <span>${receipt.paymentMethod || 'Acoount'}</span>
//                         </div>
//           </div>
                       
                        
//                          <div class="footer">
//     <div class="footer-right">
//        <p>
//             <span>Tel: (+251) 11 662 60 00/60</span><br>
//             <span>Fax: (+251) 11 662 59 99</span><br>
//             <span>P.O.Box: 27026/1000 Addis Ababa</span><br>
//             <span> Ath. Haile G/Selassie Avenue.Lex Plaza Bldg.</span>
//         </p>
          
//     </div>
    
//     <div class="footer-center">
//         <img src="assets/img/pngs.png" alt="Bank Stamp" class="stamp-image">

//     </div>

//     <div class="footer-left">
//      <p>Thank you for banking with us!</p>
//                                <p>Please retain this receipt for your records.</p>  
//                                                       <p>Lion International Bank S.C</p>
                            
//             <p class="blue">የስኬትዎ አጋር !</p>
//             <p class="red">KEY TO SUCCESS!</p>
       
//     </div>
// </div>



//                       </div><div class="contact-info">
//     <p>
//         ● SWIFT CODE: LIBSETAA&nbsp;&nbsp;
//         ● Website: <a href="http://www.anbesabank.com">www.anbesabank.com</a>&nbsp;&nbsp;
//         ● Telegram: <a href="https://t.me/LionBankSC">https://t.me/LionBankSC</a>&nbsp;&nbsp;
//         ● E-mail: <a href="mailto:info@anbesabank.com">info@anbesabank.com</a>
//     </p>
// </div></div>
//                   </body>
//               </html>
//           `);
//           printWindow.contentWindow!.document.close();
//           printWindow.contentWindow!.focus();
//           printWindow.contentWindow!.print();
//           printWindow.contentWindow!.close();
//       };
//   }
//   // In your component.ts file


// }

maskAccountNumber(accountNo: string): string {
  if (!accountNo || accountNo.length < 4) {
      return '';
  }
  const lastFourDigits = accountNo.slice(-4);
  const maskedPart = '*'.repeat(accountNo.length - 4);
  return maskedPart + lastFourDigits;
}
pad(num: number | undefined): string {
  if (num === undefined) {
    console.warn('pad function received undefined, returning default value:', num);
    return 'A0000000000'; // Default value with "A" prefix
  }
  return 'A' + num.toString().padStart(10, '0');
}

numberToWords(num: number): string {
  const a = ['', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine', 'ten', 'eleven', 'twelve', 'thirteen', 'fourteen', 'fifteen', 'sixteen', 'seventeen', 'eighteen', 'nineteen'];
  const b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];
  const g = ['', 'thousand', 'million', 'billion', 'trillion'];

  if (num === 0) return 'Zero Birr';

  const [integerPart, decimalPart] = num.toFixed(2).split('.').map(Number);  // Ensure 2 decimal places

  let result = '';
  let group = 0;
  let numInt = integerPart;

  // Convert the integer part
  while (numInt > 0) {
      const chunk = numInt % 1000;
      if (chunk) {
          const hundreds = Math.floor(chunk / 100);
          const tens = chunk % 100;
          const units = chunk % 10;

          let chunkStr = '';
          if (hundreds) {
              chunkStr += `${a[hundreds]} hundred `;
          }
          if (tens < 20) {
              chunkStr += `${a[tens]}`;
          } else {
              chunkStr += `${b[Math.floor(tens / 10)]}`;
              if (units) chunkStr += `-${a[units]}`;
          }

          result = `${chunkStr.trim()} ${g[group]} ${result}`.trim();
      }
      group += 1;
      numInt = Math.floor(numInt / 1000);
  }

  // Capitalize the first letter of each word for the integer part
  let birrPart = result
    .split(' ')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ')
    .trim() + ' Birr';

  // Convert the decimal part for cents
  let centPart = '';
  if (decimalPart) {
      if (decimalPart < 20) {
          centPart = `${a[decimalPart]} Cent`;
      } else {
          const tens = Math.floor(decimalPart / 10);
          const units = decimalPart % 10;
          centPart = `${b[tens]}`;
          if (units) centPart += `-${a[units]}`;
          centPart += ' Cent';
      }
  }

  // Construct the final result
  return centPart ? `${birrPart} And ${centPart}` : birrPart;
}
}
