import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCustomformitemCreateComponent } from './manage-basicdata-customformitem-create.component';

describe('ManageBasicdataCustomformitemCreateComponent', () => {
  let component: ManageBasicdataCustomformitemCreateComponent;
  let fixture: ComponentFixture<ManageBasicdataCustomformitemCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCustomformitemCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCustomformitemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
