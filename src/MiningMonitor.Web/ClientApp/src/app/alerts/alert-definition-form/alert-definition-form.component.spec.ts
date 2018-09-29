import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

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
import { AlertDefinitionParametersComponent } from '../alert-definition-parameters/alert-definition-parameters.component';
import { AlertDefinitionFormComponent } from './alert-definition-form.component';

describe('AlertDefinitionFormComponent', () => {
    let component: AlertDefinitionFormComponent;
    let fixture: ComponentFixture<AlertDefinitionFormComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionConnectivityParametersComponent,
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
        fixture = TestBed.createComponent(AlertDefinitionFormComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
