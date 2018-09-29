import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { MinerService } from './miner.service';

describe('MinerService', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            HttpClientTestingModule
        ]
    }));

    it('should be created', () => {
        const service: MinerService = TestBed.get(MinerService);
        expect(service).toBeTruthy();
    });
});
