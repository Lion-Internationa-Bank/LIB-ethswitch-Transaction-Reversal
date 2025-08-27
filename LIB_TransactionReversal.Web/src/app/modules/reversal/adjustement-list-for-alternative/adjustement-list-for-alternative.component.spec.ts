import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustementListForAlternativeComponent } from './adjustement-list-for-alternative.component';

describe('AdjustementListForAlternativeComponent', () => {
  let component: AdjustementListForAlternativeComponent;
  let fixture: ComponentFixture<AdjustementListForAlternativeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdjustementListForAlternativeComponent]
    });
    fixture = TestBed.createComponent(AdjustementListForAlternativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
