import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataanalysisComponent } from './dataanalysis.component';

describe('DataanalysisComponent', () => {
  let component: DataanalysisComponent;
  let fixture: ComponentFixture<DataanalysisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataanalysisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataanalysisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
