import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ChartsModule } from 'ng2-charts';

import { GpuChartComponent } from './gpu-chart.component';

describe('GpuChartComponent', () => {
    let component: GpuChartComponent;
    let fixture: ComponentFixture<GpuChartComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [GpuChartComponent],
            imports: [
                ChartsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(GpuChartComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
