import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { EnumPipe } from '../../enum.pipe';
import { AlertDefinitionGpuThresholdParametersComponent } from './alert-definition-gpu-threshold-parameters.component';

describe('AlertDefinitionGpuThresholdParametersComponent', () => {
    let component: AlertDefinitionGpuThresholdParametersComponent;
    let fixture: ComponentFixture<AlertDefinitionGpuThresholdParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionGpuThresholdParametersComponent,
                EnumPipe
            ],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionGpuThresholdParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
