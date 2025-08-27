import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReconsilationSummaryReportComponent } from './reconsilation-summary-report.component';

describe('ReconsilationSummaryReportComponent', () => {
  let component: ReconsilationSummaryReportComponent;
  let fixture: ComponentFixture<ReconsilationSummaryReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReconsilationSummaryReportComponent]
    });
    fixture = TestBed.createComponent(ReconsilationSummaryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
