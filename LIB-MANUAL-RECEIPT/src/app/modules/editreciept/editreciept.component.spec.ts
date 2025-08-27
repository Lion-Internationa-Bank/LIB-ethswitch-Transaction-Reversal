import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditrecieptComponent } from './editreciept.component';

describe('EditrecieptComponent', () => {
  let component: EditrecieptComponent;
  let fixture: ComponentFixture<EditrecieptComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditrecieptComponent]
    });
    fixture = TestBed.createComponent(EditrecieptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
