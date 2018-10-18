import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertActionsComponent } from './alert-actions.component';

describe('AlertActionsComponent', () => {
    let component: AlertActionsComponent;
    let fixture: ComponentFixture<AlertActionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AlertActionsComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertActionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
