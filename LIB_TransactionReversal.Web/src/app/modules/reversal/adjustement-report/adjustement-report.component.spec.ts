import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustementReportComponent } from './adjustement-report.component';

describe('AdjustementReportComponent', () => {
  let component: AdjustementReportComponent;
  let fixture: ComponentFixture<AdjustementReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdjustementReportComponent]
    });
    fixture = TestBed.createComponent(AdjustementReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
