import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCustomformitemEditComponent } from './manage-basicdata-customformitem-edit.component';

describe('ManageBasicdataCustomformitemEditComponent', () => {
  let component: ManageBasicdataCustomformitemEditComponent;
  let fixture: ComponentFixture<ManageBasicdataCustomformitemEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCustomformitemEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCustomformitemEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
