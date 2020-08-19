import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataComponent } from './manage-basicdata.component';

describe('ManageBasicdataComponent', () => {
  let component: ManageBasicdataComponent;
  let fixture: ComponentFixture<ManageBasicdataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
