import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReversalListForFinanceComponent } from './reversal-list-for-finance.component';

describe('ReversalListForFinanceComponent', () => {
  let component: ReversalListForFinanceComponent;
  let fixture: ComponentFixture<ReversalListForFinanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReversalListForFinanceComponent]
    });
    fixture = TestBed.createComponent(ReversalListForFinanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
