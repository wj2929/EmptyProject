import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCategorytypeEditComponent } from './manage-basicdata-categorytype-edit.component';

describe('ManageBasicdataCategorytypeEditComponent', () => {
  let component: ManageBasicdataCategorytypeEditComponent;
  let fixture: ComponentFixture<ManageBasicdataCategorytypeEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCategorytypeEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCategorytypeEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
