import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

import { GpuDataIndex } from '../../../models/GpuDataIndex';

@Component({
    selector: 'mm-gpu-chart',
    templateUrl: './gpu-chart.component.html',
    styleUrls: ['./gpu-chart.component.scss']
})
export class GpuChartComponent implements OnChanges {
    @Input()
    public data: { label: string; data: { x: Date; y: number; }[]; }[];
    @Input()
    public name: string;

    public hashrate: number;
    public temp: number;
    public fanSpeed: number;
    public options = {
        animation: false,
        responsive: true,
        maintainAspectRatio: false,
        legend: false,
        tooltips: {
            enabled: true,
            mode: 'index',
            intersect: false,
            callbacks: {
                label: this.FormatLabel
            }
        },
        hover: {
            mode: 'index',
            intersect: false
        },
        scales: {
            xAxes: [{
                type: 'time'
            }],
            yAxes: [{
                ticks: {
                    suggestedMin: 0,
                    suggestedMax: 80
                },
            }]
        },
        elements: {
            line: {
                fill: false,
                tension: 0
            },
            point: {
                radius: 0,
                hoverRadius: 4
            }
        }
    };
    public colors = [
        {
            backgroundColor: '#fff',
            borderColor: '#fff'
        },
        {
            backgroundColor: '#3498DB',
            borderColor: '#3498DB'
        },
        {
            backgroundColor: '#F39C12',
            borderColor: '#F39C12'
        }
    ];

    public ngOnChanges(changes: SimpleChanges) {
        const hashrateData = changes.data.currentValue[GpuDataIndex.HashRate].data;
        const tempDate = changes.data.currentValue[GpuDataIndex.Temp].data;
        const fanSpeedData = changes.data.currentValue[GpuDataIndex.FanSpeed].data;

        this.hashrate = hashrateData[hashrateData.length - 1].y;
        this.temp = tempDate[tempDate.length - 1].y;
        this.fanSpeed = fanSpeedData[fanSpeedData.length - 1].y;
    }

    private FormatLabel(tooltipItems, data) {
        switch (tooltipItems.datasetIndex) {
            case GpuDataIndex.HashRate:
                return `${data.datasets[tooltipItems.datasetIndex].label}: ${tooltipItems.yLabel} MH/s`;
            case GpuDataIndex.Temp:
                return `${data.datasets[tooltipItems.datasetIndex].label}: ${tooltipItems.yLabel}°`;
            case GpuDataIndex.FanSpeed:
                return `${data.datasets[tooltipItems.datasetIndex].label}: ${tooltipItems.yLabel}%`;
            default:
                return `${data.datasets[tooltipItems.datasetIndex].label}: ${tooltipItems.yLabel}`;
        }
    }
}
