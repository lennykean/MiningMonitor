import * as React from 'react';

import autobind from 'autobind-decorator';
import * as d3 from 'd3';

import { Legend, Line, LineChart, Tooltip, XAxis, YAxis } from 'recharts';

import { GpuDataPoint } from '../../models';

interface Props {
    title: string;
    syncId: string;
    data: GpuDataPoint[];
}

export class GpuChart extends React.Component<Props, any> {
    private tickFormatter: (tick: Date) => string;
    private toolTipLabelFormatter: (tick: Date) => string;

    constructor(props) {
        super(props);
        this.toolTipLabelFormatter = d3.timeFormat('%m/%e/%y %H:%M');
        this.tickFormatter = d3.timeFormat('%H:%M');
    }
    public render() {
        const currentData = this.props.data[this.props.data.length - 1];
        return (
            <div className="gpu-chart">
                <h4>{this.props.title}</h4>
                <h6>
                    <span title="Hashrate" style={{ color: '#fff' }}>{currentData.hashRate} MH/s</span>
                    &nbsp;&bull;&nbsp;
                    <span title="Temperature" style={{ color: '#3498DB' }}>{currentData.temperature}° C</span>
                    &nbsp;&bull;&nbsp;
                    <span title="Fan speed" style={{ color: '#F39C12' }}>{currentData.fanSpeed}%</span>
                </h6>
                <LineChart height={200} width={300} data={this.props.data} syncId={this.props.syncId}>
                    <XAxis dataKey="snapshotTime" tickFormatter={this.formatTick} />
                    <Tooltip
                        wrapperStyle={{
                            backgroundColor: 'rgba(0,0,0,0.5)',
                            border: 'none',
                            fontWeight: 'bold',
                        }}
                        formatter={this.formatTooltipData}
                        labelFormatter={this.formatTooltipLabel}
                    />
                    <Legend />
                    <Line dot={false} name="Hashrate" dataKey="hashRate" stroke="#fff" />
                    <Line dot={false} name="Temp" dataKey="temperature" stroke="#3498DB" />
                    <Line dot={false} name="Fan" dataKey="fanSpeed" stroke="#F39C12" />
                </LineChart>
            </div>
        );
    }
    @autobind
    private formatTooltipData(value: any, name: string) {
        switch (name) {
            case 'Hashrate':
                return `${value} MH/s`;
            case 'Temp':
                return `${value}° C`;
            case 'Fan':
                return `${value}%`;
            default:
                return value;
        }
    }
    @autobind
    private formatTick(timestamp: number) {
        const date = new Date(timestamp);
        return this.tickFormatter(date);
    }
    @autobind
    private formatTooltipLabel(timestamp: number) {
        const date = new Date(timestamp);
        return this.toolTipLabelFormatter(date);
    }
}
