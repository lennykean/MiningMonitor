import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { EnumPipe } from '../../enum.pipe';
import { HumanizePipe } from '../../humanize.pipe';
import { GpuThresholdParametersComponent } from './gpu-threshold-parameters.component';

describe('GpuThresholdParametersComponent', () => {
    let component: GpuThresholdParametersComponent;
    let fixture: ComponentFixture<GpuThresholdParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                EnumPipe,
                GpuThresholdParametersComponent,
                HumanizePipe
            ],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(GpuThresholdParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
