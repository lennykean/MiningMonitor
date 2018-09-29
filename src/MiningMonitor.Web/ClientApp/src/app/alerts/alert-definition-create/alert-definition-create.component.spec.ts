import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { EnumPipe } from '../../enum.pipe';
import {
    AlertDefinitionConnectivityParametersComponent
} from '../alert-definition-connectivity-parameters/alert-definition-connectivity-parameters.component';
import { AlertDefinitionFormComponent } from '../alert-definition-form/alert-definition-form.component';
import {
    AlertDefinitionGpuThresholdParametersComponent
} from '../alert-definition-gpu-threshold-parameters/alert-definition-gpu-threshold-parameters.component';
import {
    AlertDefinitionHashrateParametersComponent
} from '../alert-definition-hashrate-parameters/alert-definition-hashrate-parameters.component';
import { AlertDefinitionParametersComponent } from '../alert-definition-parameters/alert-definition-parameters.component';
import { AlertDefinitionCreateComponent } from './alert-definition-create.component';

describe('AlertDefinitionCreateComponent', () => {
    let component: AlertDefinitionCreateComponent;
    let fixture: ComponentFixture<AlertDefinitionCreateComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionConnectivityParametersComponent,
                AlertDefinitionCreateComponent,
                AlertDefinitionFormComponent,
                AlertDefinitionGpuThresholdParametersComponent,
                AlertDefinitionHashrateParametersComponent,
                AlertDefinitionParametersComponent,
                EnumPipe
            ],
            imports: [
                FormsModule,
                HttpClientTestingModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionCreateComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
