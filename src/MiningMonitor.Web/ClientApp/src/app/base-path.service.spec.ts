import { TestBed } from '@angular/core/testing';

import { BasePathService } from './base-path.service';

describe('BasePathService', () => {
    beforeEach(() => TestBed.configureTestingModule({}));

    it('should be created', () => {
        const service: BasePathService = TestBed.get(BasePathService);
        expect(service).toBeTruthy();
    });
});
