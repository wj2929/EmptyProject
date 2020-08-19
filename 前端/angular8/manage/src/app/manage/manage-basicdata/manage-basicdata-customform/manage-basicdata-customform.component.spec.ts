import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCustomformComponent } from './manage-basicdata-customform.component';

describe('ManageBasicdataCustomformComponent', () => {
  let component: ManageBasicdataCustomformComponent;
  let fixture: ComponentFixture<ManageBasicdataCustomformComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCustomformComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCustomformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
