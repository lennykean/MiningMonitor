import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { AlertDefinitionHashrateParametersComponent } from './alert-definition-hashrate-parameters.component';

describe('AlertDefinitionHashrateParametersComponent', () => {
    let component: AlertDefinitionHashrateParametersComponent;
    let fixture: ComponentFixture<AlertDefinitionHashrateParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AlertDefinitionHashrateParametersComponent],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionHashrateParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
