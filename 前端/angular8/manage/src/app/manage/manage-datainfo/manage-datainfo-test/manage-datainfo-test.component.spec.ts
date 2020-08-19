import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDatainfoTestComponent } from './manage-datainfo-test.component';

describe('ManageDatainfoTestComponent', () => {
  let component: ManageDatainfoTestComponent;
  let fixture: ComponentFixture<ManageDatainfoTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDatainfoTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDatainfoTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
