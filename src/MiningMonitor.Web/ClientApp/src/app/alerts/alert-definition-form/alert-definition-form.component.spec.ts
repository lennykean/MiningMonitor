import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { EnumPipe } from '../../enum.pipe';
import { HumanizePipe } from '../../humanize.pipe';
import { AlertActionDetailComponent } from '../alert-action-detail/alert-action-detail.component';
import { AlertActionsComponent } from '../alert-actions/alert-actions.component';
import { AlertDefinitionParametersComponent } from '../alert-definition-parameters/alert-definition-parameters.component';
import { ConnectivityParametersComponent } from '../connectivity-parameters/connectivity-parameters.component';
import { GpuThresholdParametersComponent } from '../gpu-threshold-parameters/gpu-threshold-parameters.component';
import { HashrateParametersComponent } from '../hashrate-parameters/hashrate-parameters.component';
import { AlertDefinitionFormComponent } from './alert-definition-form.component';

describe('AlertDefinitionFormComponent', () => {
    let component: AlertDefinitionFormComponent;
    let fixture: ComponentFixture<AlertDefinitionFormComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertActionDetailComponent,
                AlertActionsComponent,
                AlertDefinitionFormComponent,
                AlertDefinitionParametersComponent,
                ConnectivityParametersComponent,
                EnumPipe,
                GpuThresholdParametersComponent,
                HashrateParametersComponent,
                HumanizePipe
            ],
            imports: [
                FontAwesomeModule,
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
