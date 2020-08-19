import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDashboardLoginLogComponent } from './manage-dashboard-login-log.component';

describe('ManageDashboardLoginLogComponent', () => {
  let component: ManageDashboardLoginLogComponent;
  let fixture: ComponentFixture<ManageDashboardLoginLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDashboardLoginLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDashboardLoginLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
