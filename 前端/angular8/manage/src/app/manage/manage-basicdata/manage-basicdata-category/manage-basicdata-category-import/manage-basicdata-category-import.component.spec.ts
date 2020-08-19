import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCategoryImportComponent } from './manage-basicdata-category-import.component';

describe('ManageBasicdataCategoryImportComponent', () => {
  let component: ManageBasicdataCategoryImportComponent;
  let fixture: ComponentFixture<ManageBasicdataCategoryImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCategoryImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCategoryImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
