import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataanalysisSignupComponent } from './dataanalysis-signup.component';

describe('DataanalysisSignupComponent', () => {
  let component: DataanalysisSignupComponent;
  let fixture: ComponentFixture<DataanalysisSignupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataanalysisSignupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataanalysisSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
