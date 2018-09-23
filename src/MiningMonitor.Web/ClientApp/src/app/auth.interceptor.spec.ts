import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { AuthInterceptor } from './auth.interceptor';

describe('AuthInterceptor', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            HttpClientTestingModule,
            RouterTestingModule,
        ]
    }));

    it('should be created', () => {
        const service: AuthInterceptor = TestBed.get(AuthInterceptor);
        expect(service).toBeTruthy();
    });
});
