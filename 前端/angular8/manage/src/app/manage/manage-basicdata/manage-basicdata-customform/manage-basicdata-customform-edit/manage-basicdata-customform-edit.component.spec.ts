import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCustomformEditComponent } from './manage-basicdata-customform-edit.component';

describe('ManageBasicdataCustomformEditComponent', () => {
  let component: ManageBasicdataCustomformEditComponent;
  let fixture: ComponentFixture<ManageBasicdataCustomformEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCustomformEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCustomformEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
