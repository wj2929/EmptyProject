import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCategoryEditComponent } from './manage-basicdata-category-edit.component';

describe('ManageBasicdataCategoryEditComponent', () => {
  let component: ManageBasicdataCategoryEditComponent;
  let fixture: ComponentFixture<ManageBasicdataCategoryEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCategoryEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCategoryEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
