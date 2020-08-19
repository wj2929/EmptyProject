import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomFormSettingComponent } from './custom-form-setting.component';
describe('CustomFormSettingComponent', () => {
  let component: CustomFormSettingComponent;
  let fixture: ComponentFixture<CustomFormSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomFormSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomFormSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
