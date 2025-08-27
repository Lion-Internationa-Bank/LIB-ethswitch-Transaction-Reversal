import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionNotFoundLibComponent } from './transaction-not-found-lib.component';

describe('TransactionNotFoundLibComponent', () => {
  let component: TransactionNotFoundLibComponent;
  let fixture: ComponentFixture<TransactionNotFoundLibComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TransactionNotFoundLibComponent]
    });
    fixture = TestBed.createComponent(TransactionNotFoundLibComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
