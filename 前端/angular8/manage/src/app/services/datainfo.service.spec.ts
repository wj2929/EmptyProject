import { TestBed } from '@angular/core/testing';

import { DataInfoService } from './DataInfo.service';

describe('DataInfoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataInfoService = TestBed.get(DataInfoService);
    expect(service).toBeTruthy();
  });
});
