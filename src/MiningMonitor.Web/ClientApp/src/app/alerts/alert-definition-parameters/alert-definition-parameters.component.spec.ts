import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { EnumPipe } from '../../enum.pipe';
import {
    AlertDefinitionConnectivityParametersComponent
} from '../alert-definition-connectivity-parameters/alert-definition-connectivity-parameters.component';
import {
    AlertDefinitionGpuThresholdParametersComponent
} from '../alert-definition-gpu-threshold-parameters/alert-definition-gpu-threshold-parameters.component';
import {
    AlertDefinitionHashrateParametersComponent
} from '../alert-definition-hashrate-parameters/alert-definition-hashrate-parameters.component';
import { AlertDefinitionParametersComponent } from './alert-definition-parameters.component';

describe('AlertDefinitionParametersComponent', () => {
    let component: AlertDefinitionParametersComponent;
    let fixture: ComponentFixture<AlertDefinitionParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionConnectivityParametersComponent,
                AlertDefinitionGpuThresholdParametersComponent,
                AlertDefinitionHashrateParametersComponent,
                AlertDefinitionParametersComponent,
                EnumPipe
            ],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
