import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageUserEditPasswordComponent } from './manage-user-edit-password.component';

describe('ManageUserEditPasswordComponent', () => {
  let component: ManageUserEditPasswordComponent;
  let fixture: ComponentFixture<ManageUserEditPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageUserEditPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageUserEditPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
