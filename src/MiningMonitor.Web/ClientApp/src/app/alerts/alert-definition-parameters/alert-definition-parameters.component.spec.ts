import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { EnumPipe } from '../../enum.pipe';
import { AlertDefinitionParametersComponent } from './alert-definition-parameters.component';
import { ConnectivityParametersComponent } from '../connectivity-parameters/connectivity-parameters.component';
import { GpuThresholdParametersComponent } from '../gpu-threshold-parameters/gpu-threshold-parameters.component';
import { HashrateParametersComponent } from '../hashrate-parameters/hashrate-parameters.component';

describe('AlertDefinitionParametersComponent', () => {
    let component: AlertDefinitionParametersComponent;
    let fixture: ComponentFixture<AlertDefinitionParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                ConnectivityParametersComponent,
                GpuThresholdParametersComponent,
                HashrateParametersComponent,
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
