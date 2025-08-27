import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReciepthistoryComponent } from './reciepthistory.component';

describe('ReciepthistoryComponent', () => {
  let component: ReciepthistoryComponent;
  let fixture: ComponentFixture<ReciepthistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReciepthistoryComponent]
    });
    fixture = TestBed.createComponent(ReciepthistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
