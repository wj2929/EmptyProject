import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDatainfoEditComponent } from './manage-datainfo-edit.component';

describe('ManageDatainfoEditComponent', () => {
  let component: ManageDatainfoEditComponent;
  let fixture: ComponentFixture<ManageDatainfoEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDatainfoEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDatainfoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
