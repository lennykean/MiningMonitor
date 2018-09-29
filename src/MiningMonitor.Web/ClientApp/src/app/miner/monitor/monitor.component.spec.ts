import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ChartsModule } from 'ng2-charts';

import { GpuChartComponent } from '../gpu-chart/gpu-chart.component';
import { MonitorComponent } from './monitor.component';

describe('MonitorComponent', () => {
    let component: MonitorComponent;
    let fixture: ComponentFixture<MonitorComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                GpuChartComponent,
                MonitorComponent
            ],
            imports: [
                HttpClientTestingModule,
                RouterTestingModule,
                ChartsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MonitorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
