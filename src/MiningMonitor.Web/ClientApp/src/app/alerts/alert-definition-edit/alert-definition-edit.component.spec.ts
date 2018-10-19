import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { EnumPipe } from '../../enum.pipe';
import { HumanizePipe } from '../../humanize.pipe';
import { AlertActionDetailComponent } from '../alert-action-detail/alert-action-detail.component';
import { AlertActionsComponent } from '../alert-actions/alert-actions.component';
import { AlertDefinitionFormComponent } from '../alert-definition-form/alert-definition-form.component';
import { AlertDefinitionParametersComponent } from '../alert-definition-parameters/alert-definition-parameters.component';
import { ConnectivityParametersComponent } from '../connectivity-parameters/connectivity-parameters.component';
import { GpuThresholdParametersComponent } from '../gpu-threshold-parameters/gpu-threshold-parameters.component';
import { HashrateParametersComponent } from '../hashrate-parameters/hashrate-parameters.component';
import { AlertDefinitionEditComponent } from './alert-definition-edit.component';

describe('AlertDefinitionEditComponent', () => {
    let component: AlertDefinitionEditComponent;
    let fixture: ComponentFixture<AlertDefinitionEditComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertActionDetailComponent,
                AlertActionsComponent,
                AlertDefinitionEditComponent,
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
                HttpClientTestingModule,
                RouterTestingModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionEditComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
