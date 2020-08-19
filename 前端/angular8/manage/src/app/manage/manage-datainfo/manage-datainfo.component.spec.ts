import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDatainfoComponent } from './manage-datainfo.component';

describe('ManageDatainfoComponent', () => {
  let component: ManageDatainfoComponent;
  let fixture: ComponentFixture<ManageDatainfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDatainfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDatainfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
