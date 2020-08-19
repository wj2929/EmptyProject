import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCategoryComponent } from './manage-basicdata-category.component';

describe('ManageBasicdataCategoryComponent', () => {
  let component: ManageBasicdataCategoryComponent;
  let fixture: ComponentFixture<ManageBasicdataCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
