import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { VersionService } from './version.service';

describe('VersionService', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            HttpClientTestingModule
        ]
    }));

    it('should be created', () => {
        const service: VersionService = TestBed.get(VersionService);
        expect(service).toBeTruthy();
    });
});
