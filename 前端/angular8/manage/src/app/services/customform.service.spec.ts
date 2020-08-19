import { TestBed } from '@angular/core/testing';

import { CustomformService } from './customform.service';

describe('CustomformService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomformService = TestBed.get(CustomformService);
    expect(service).toBeTruthy();
  });
});
