import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneraterecieptComponent } from './generatereciept.component';

describe('GeneraterecieptComponent', () => {
  let component: GeneraterecieptComponent;
  let fixture: ComponentFixture<GeneraterecieptComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GeneraterecieptComponent]
    });
    fixture = TestBed.createComponent(GeneraterecieptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
