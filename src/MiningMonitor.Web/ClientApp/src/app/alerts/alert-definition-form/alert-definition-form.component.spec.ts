import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { EnumPipe } from '../../enum.pipe';
import { AlertDefinitionParametersComponent } from '../alert-definition-parameters/alert-definition-parameters.component';
import { AlertDefinitionFormComponent } from './alert-definition-form.component';
import { ConnectivityParametersComponent } from '../connectivity-parameters/connectivity-parameters.component';
import { GpuThresholdParametersComponent } from '../gpu-threshold-parameters/gpu-threshold-parameters.component';
import { HashrateParametersComponent } from '../hashrate-parameters/hashrate-parameters.component';

describe('AlertDefinitionFormComponent', () => {
    let component: AlertDefinitionFormComponent;
    let fixture: ComponentFixture<AlertDefinitionFormComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionFormComponent,
                AlertDefinitionParametersComponent,
                ConnectivityParametersComponent,
                GpuThresholdParametersComponent,
                HashrateParametersComponent,
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
