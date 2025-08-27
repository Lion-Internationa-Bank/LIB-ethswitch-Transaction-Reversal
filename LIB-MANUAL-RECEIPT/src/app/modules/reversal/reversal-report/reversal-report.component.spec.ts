import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReversalReportComponent } from './reversal-report.component';

describe('ReversalReportComponent', () => {
  let component: ReversalReportComponent;
  let fixture: ComponentFixture<ReversalReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReversalReportComponent]
    });
    fixture = TestBed.createComponent(ReversalReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
