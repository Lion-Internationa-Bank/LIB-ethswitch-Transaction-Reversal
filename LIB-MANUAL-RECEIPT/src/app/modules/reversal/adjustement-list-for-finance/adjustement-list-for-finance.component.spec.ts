import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustementListForFinanceComponent } from './adjustement-list-for-finance.component';

describe('AdjustementListForFinanceComponent', () => {
  let component: AdjustementListForFinanceComponent;
  let fixture: ComponentFixture<AdjustementListForFinanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdjustementListForFinanceComponent]
    });
    fixture = TestBed.createComponent(AdjustementListForFinanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
