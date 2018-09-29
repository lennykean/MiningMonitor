import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { AlertDefinitionsService } from './alert-definitions.service';

describe('AlertDefinitionsService', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            HttpClientTestingModule
        ]
    }));

    it('should be created', () => {
        const service: AlertDefinitionsService = TestBed.get(AlertDefinitionsService);
        expect(service).toBeTruthy();
    });
});
