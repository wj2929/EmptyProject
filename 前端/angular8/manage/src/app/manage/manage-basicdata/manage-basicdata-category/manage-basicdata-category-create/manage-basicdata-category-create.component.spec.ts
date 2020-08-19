import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCategoryCreateComponent } from './manage-basicdata-category-create.component';

describe('ManageBasicdataCategoryCreateComponent', () => {
  let component: ManageBasicdataCategoryCreateComponent;
  let fixture: ComponentFixture<ManageBasicdataCategoryCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCategoryCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCategoryCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
