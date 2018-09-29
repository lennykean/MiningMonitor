import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { EnumPipe } from '../../enum.pipe';
import { AlertDefinitionFormComponent } from '../alert-definition-form/alert-definition-form.component';
import { AlertDefinitionParametersComponent } from '../alert-definition-parameters/alert-definition-parameters.component';
import { ConnectivityParametersComponent } from '../connectivity-parameters/connectivity-parameters.component';
import { GpuThresholdParametersComponent } from '../gpu-threshold-parameters/gpu-threshold-parameters.component';
import { HashrateParametersComponent } from '../hashrate-parameters/hashrate-parameters.component';
import { AlertDefinitionCreateComponent } from './alert-definition-create.component';

describe('AlertDefinitionCreateComponent', () => {
    let component: AlertDefinitionCreateComponent;
    let fixture: ComponentFixture<AlertDefinitionCreateComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionCreateComponent,
                AlertDefinitionFormComponent,
                AlertDefinitionParametersComponent,
                ConnectivityParametersComponent,
                EnumPipe,
                GpuThresholdParametersComponent,
                HashrateParametersComponent,
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
