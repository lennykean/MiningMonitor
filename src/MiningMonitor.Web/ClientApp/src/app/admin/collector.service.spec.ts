import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CollectorService } from './collector.service';

describe('CollectorService', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            HttpClientTestingModule
        ]
    }));

    it('should be created', () => {
        const service: CollectorService = TestBed.get(CollectorService);
        expect(service).toBeTruthy();
    });
});
