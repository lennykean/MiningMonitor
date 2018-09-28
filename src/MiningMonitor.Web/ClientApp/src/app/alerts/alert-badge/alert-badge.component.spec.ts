import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertBadgeComponent } from './alert-badge.component';

describe('AlertBadgeComponent', () => {
    let component: AlertBadgeComponent;
    let fixture: ComponentFixture<AlertBadgeComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AlertBadgeComponent],
            imports: [
                HttpClientTestingModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertBadgeComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
