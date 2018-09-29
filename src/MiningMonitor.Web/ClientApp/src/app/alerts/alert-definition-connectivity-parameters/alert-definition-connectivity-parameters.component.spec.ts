import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertDefinitionConnectivityParametersComponent } from './alert-definition-connectivity-parameters.component';

describe('AlertDefinitionConnectivityParametersComponent', () => {
    let component: AlertDefinitionConnectivityParametersComponent;
    let fixture: ComponentFixture<AlertDefinitionConnectivityParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AlertDefinitionConnectivityParametersComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionConnectivityParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
