import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReversalListForAlternativeComponent } from './reversal-list-for-alternative/reversal-list-for-alternative.component';
import { ReversalListForFinanceComponent } from './reversal-list-for-finance/reversal-list-for-finance.component';
import { ReversalReportComponent } from './reversal-report/reversal-report.component';
import { ImportTransactionComponent } from './import-transaction/import-transaction.component';
import { SuccessfullTransactionComponent } from './successfull-transaction/successfull-transaction.component';
import { TransactionNotFoundEthswitchComponent } from './transaction-not-found-ethswitch/transaction-not-found-ethswitch.component';
import { TransactionNotFoundLibComponent } from './transaction-not-found-lib/transaction-not-found-lib.component';
import { AdjustementListForAlternativeComponent } from './adjustement-list-for-alternative/adjustement-list-for-alternative.component';
import { AdjustementListForFinanceComponent } from './adjustement-list-for-finance/adjustement-list-for-finance.component';
import { ReconsilationSummaryReportComponent } from './reconsilation-summary-report/reconsilation-summary-report.component';
import { EthiswichInvalidDateComponent } from './ethiswich-invalid-date/ethiswich-invalid-date.component';
import { AdjustementReportComponent } from './adjustement-report/adjustement-report.component';

const routes: Routes = [
  {path:'alternative-list', component:ReversalListForAlternativeComponent},
  {path:'transaction-list', component:ReversalListForFinanceComponent},
  {path:'report', component:ReversalReportComponent},
  {path:'adjustement_report', component: AdjustementReportComponent},
  {path:'import', component:ImportTransactionComponent},
  {path:'successfull-transaction', component:SuccessfullTransactionComponent},
  {path:'transaction-notfound-etswich', component:TransactionNotFoundEthswitchComponent},
  {path:'transaction-notfound-lib', component:TransactionNotFoundLibComponent},
  {path:'adjustment-approval-list', component:AdjustementListForAlternativeComponent},
  {path:'adjustment-payment-list', component:AdjustementListForFinanceComponent},
  {path:'reconsilation-summary', component:ReconsilationSummaryReportComponent},
  {path:'ethiswich_invalid_date',component:EthiswichInvalidDateComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReversalRoutingModule { }
