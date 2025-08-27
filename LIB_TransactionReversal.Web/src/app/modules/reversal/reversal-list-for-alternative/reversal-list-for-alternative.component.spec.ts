import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReversalListForAlternativeComponent } from './reversal-list-for-alternative.component';

describe('ReversalListForAlternativeComponent', () => {
  let component: ReversalListForAlternativeComponent;
  let fixture: ComponentFixture<ReversalListForAlternativeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReversalListForAlternativeComponent]
    });
    fixture = TestBed.createComponent(ReversalListForAlternativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
