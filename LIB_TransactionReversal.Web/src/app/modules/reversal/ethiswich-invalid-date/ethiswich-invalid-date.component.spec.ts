import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EthiswichInvalidDateComponent } from './ethiswich-invalid-date.component';

describe('EthiswichInvalidDateComponent', () => {
  let component: EthiswichInvalidDateComponent;
  let fixture: ComponentFixture<EthiswichInvalidDateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EthiswichInvalidDateComponent]
    });
    fixture = TestBed.createComponent(EthiswichInvalidDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
