import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReversalRoutingModule } from './reversal-routing.module';
import { ReversalListForAlternativeComponent } from './reversal-list-for-alternative/reversal-list-for-alternative.component';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ReversalListForFinanceComponent } from './reversal-list-for-finance/reversal-list-for-finance.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule,  } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { ReversalReportComponent } from './reversal-report/reversal-report.component';
import { ImportTransactionComponent } from './import-transaction/import-transaction.component';
import { SuccessfullTransactionComponent } from './successfull-transaction/successfull-transaction.component';
import { TransactionNotFoundLibComponent } from './transaction-not-found-lib/transaction-not-found-lib.component';
import { TransactionNotFoundEthswitchComponent } from './transaction-not-found-ethswitch/transaction-not-found-ethswitch.component';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatExpansionModule} from '@angular/material/expansion';
import { AdjustementListForAlternativeComponent } from './adjustement-list-for-alternative/adjustement-list-for-alternative.component';
import { AdjustementListForFinanceComponent } from './adjustement-list-for-finance/adjustement-list-for-finance.component';
import { UpdateAcoountNumberComponent } from './update-acoount-number/update-acoount-number.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ReconsilationSummaryReportComponent } from './reconsilation-summary-report/reconsilation-summary-report.component';
import { EthiswichInvalidDateComponent } from './ethiswich-invalid-date/ethiswich-invalid-date.component';
import { AdjustementReportComponent } from './adjustement-report/adjustement-report.component';


@NgModule({
  declarations: [
    ReversalListForAlternativeComponent,
    ReversalListForFinanceComponent,
    ReversalReportComponent,
    ImportTransactionComponent,
    SuccessfullTransactionComponent,
    TransactionNotFoundLibComponent,
    TransactionNotFoundEthswitchComponent,
    AdjustementListForAlternativeComponent,
    AdjustementListForFinanceComponent,
    UpdateAcoountNumberComponent,
    ReconsilationSummaryReportComponent,
    EthiswichInvalidDateComponent,
    AdjustementReportComponent
  ],
  imports: [
    CommonModule,
    ReversalRoutingModule,
    MatTableModule,
    HttpClientModule,
    MatButtonModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatCheckboxModule,
    MatExpansionModule,
    MatDialogModule
  ]
})
export class ReversalModule { }
