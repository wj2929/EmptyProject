import { TestBed } from '@angular/core/testing';

import { SelhttpService } from './selhttp.service';

describe('SelhttpService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SelhttpService = TestBed.get(SelhttpService);
    expect(service).toBeTruthy();
  });
});
