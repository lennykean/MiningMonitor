import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertDetailComponent } from './alert-detail.component';

describe('AlertDetailComponent', () => {
    let component: AlertDetailComponent;
    let fixture: ComponentFixture<AlertDetailComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AlertDetailComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDetailComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
