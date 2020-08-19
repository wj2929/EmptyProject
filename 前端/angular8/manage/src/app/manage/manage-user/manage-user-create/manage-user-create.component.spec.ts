import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageUserCreateComponent } from './manage-user-create.component';

describe('ManageUserCreateComponent', () => {
  let component: ManageUserCreateComponent;
  let fixture: ComponentFixture<ManageUserCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageUserCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageUserCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
