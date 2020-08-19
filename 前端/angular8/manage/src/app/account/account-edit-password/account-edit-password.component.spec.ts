import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountEditPasswordComponent } from './account-edit-password.component';

describe('AccountEditPasswordComponent', () => {
  let component: AccountEditPasswordComponent;
  let fixture: ComponentFixture<AccountEditPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountEditPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountEditPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
