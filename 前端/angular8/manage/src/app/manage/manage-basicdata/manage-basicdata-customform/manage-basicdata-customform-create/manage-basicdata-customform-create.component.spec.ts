import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCustomformCreateComponent } from './manage-basicdata-customform-create.component';

describe('ManageBasicdataCustomformCreateComponent', () => {
  let component: ManageBasicdataCustomformCreateComponent;
  let fixture: ComponentFixture<ManageBasicdataCustomformCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCustomformCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCustomformCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
