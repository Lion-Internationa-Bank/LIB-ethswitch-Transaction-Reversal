import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateAcoountNumberComponent } from './update-acoount-number.component';

describe('UpdateAcoountNumberComponent', () => {
  let component: UpdateAcoountNumberComponent;
  let fixture: ComponentFixture<UpdateAcoountNumberComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateAcoountNumberComponent]
    });
    fixture = TestBed.createComponent(UpdateAcoountNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
