import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBasicdataCategorytypeCreateComponent } from './manage-basicdata-categorytype-create.component';

describe('ManageBasicdataCategorytypeCreateComponent', () => {
  let component: ManageBasicdataCategorytypeCreateComponent;
  let fixture: ComponentFixture<ManageBasicdataCategorytypeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageBasicdataCategorytypeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageBasicdataCategorytypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
