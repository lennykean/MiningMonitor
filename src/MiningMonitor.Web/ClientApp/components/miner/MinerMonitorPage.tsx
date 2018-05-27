import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { bindActionCreators } from 'redux';

import * as _ from 'lodash';

import { GpuChart } from '.';
import { FetchAllMinersAction, FetchSnapshotsAction, minerActions } from '../../actions';
import { GpuStats, Miner, Snapshot } from '../../models';
import { AppState, Busy, BusyAction } from '../../store';
import { pivotSnapshots } from '../../utils';

interface Props extends RouteComponentProps<{ id: string }> {
    busy: Busy[];
    miner: Miner;
    snapshots: Snapshot[];
    fetchAllMiners: FetchAllMinersAction;
    fetchSnapshots: FetchSnapshotsAction;
}

export class MinerMonitorPage extends React.Component<Props> {
    private intervalId: number;
    public get isLoading(): boolean {
        return _.some(this.props.busy, (busy) => busy.action === BusyAction.FetchingMiners);
    }
    public componentWillMount() {
        this.props.fetchSnapshots(this.props.match.params.id);
        this.intervalId = setInterval(() => {
            this.props.fetchSnapshots(this.props.match.params.id);
        }, 5000) as any as number;

        if (this.isLoading || this.props.miner) {
            return;
        }
        this.props.fetchAllMiners();
    }
    public componentWillUnmount() {
        clearInterval(this.intervalId);
    }
    public render() {
        if (!this.props.miner || !this.props.snapshots) {
            return null;
        }
        const allGpuData = pivotSnapshots(this.props.snapshots || []);
        const currentData = this.props.snapshots[this.props.snapshots.length - 1];
        return (
            <>
                <h1>{this.props.miner.name}</h1>
                <h4>{currentData && currentData.minerStatistics ?
                    currentData.minerStatistics.ethereum.hashrate / 1000 :
                    '--'} MH/s
                </h4>
                {allGpuData.map((gpuData, index) => (
                    <GpuChart key={index} data={gpuData} title={`GPU ${index + 1}`} syncId={this.props.miner.id} />
                ))}
            </>
        );
    }
}

const mapStateToProps = (state: AppState, ownProps: Props) => ({
    ...ownProps,
    busy: state.busy.busy,
    miner: _.find(state.miner.miners, { id: ownProps.match.params.id }),
    snapshots: state.snapshot.snapshots,
} as Props);

const mapDispatchToProps = (dispatch: Dispatch<AppState>) => ({
    fetchAllMiners: bindActionCreators(minerActions.fetchAllMiners, dispatch),
    fetchSnapshots: bindActionCreators(minerActions.fetchSnapshots, dispatch),
} as Props);

export default connect(mapStateToProps, mapDispatchToProps)(MinerMonitorPage);
