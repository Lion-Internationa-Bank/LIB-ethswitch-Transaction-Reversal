import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessfullTransactionComponent } from './successfull-transaction.component';

describe('SuccessfullTransactionComponent', () => {
  let component: SuccessfullTransactionComponent;
  let fixture: ComponentFixture<SuccessfullTransactionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SuccessfullTransactionComponent]
    });
    fixture = TestBed.createComponent(SuccessfullTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
