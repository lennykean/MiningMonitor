import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { AlertsComponent } from './alerts.component';

describe('AlertsComponent', () => {
    let component: AlertsComponent;
    let fixture: ComponentFixture<AlertsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AlertsComponent],
            imports: [
                HttpClientTestingModule,
                RouterTestingModule,
                FontAwesomeModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
