import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetloginComponent } from './assetlogin.component';

describe('AssetloginComponent', () => {
  let component: AssetloginComponent;
  let fixture: ComponentFixture<AssetloginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AssetloginComponent]
    });
    fixture = TestBed.createComponent(AssetloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
