import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionNotFoundEthswitchComponent } from './transaction-not-found-ethswitch.component';

describe('TransactionNotFoundEthswitchComponent', () => {
  let component: TransactionNotFoundEthswitchComponent;
  let fixture: ComponentFixture<TransactionNotFoundEthswitchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TransactionNotFoundEthswitchComponent]
    });
    fixture = TestBed.createComponent(TransactionNotFoundEthswitchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
