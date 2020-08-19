import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDatainfoCreateComponent } from './manage-datainfo-create.component';

describe('ManageDatainfoCreateComponent', () => {
  let component: ManageDatainfoCreateComponent;
  let fixture: ComponentFixture<ManageDatainfoCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDatainfoCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDatainfoCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
