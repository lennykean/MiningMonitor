import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { SnapshotService } from './snapshot.service';

describe('SnapshotService', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            HttpClientTestingModule
        ]
    }));

    it('should be created', () => {
        const service: SnapshotService = TestBed.get(SnapshotService);
        expect(service).toBeTruthy();
    });
});
