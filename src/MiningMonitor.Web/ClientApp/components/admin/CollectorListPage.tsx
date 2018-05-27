import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators, Dispatch } from 'redux';

import { Button, Table } from 'reactstrap';

import autobind from 'autobind-decorator';

import * as _ from 'lodash';
import * as Icon from 'react-feather';

import { collectorActions, FetchAllCollectorsAction, UpdateCollectorAction } from '../../actions';
import { Collector } from '../../models';
import { AppState, Busy } from '../../store';
import { BusyAction } from '../../store/state/BusyAction';

interface Props {
    busy: Busy[];
    collectors: Collector[];
    fetchAllCollectors: FetchAllCollectorsAction;
    updateCollector: UpdateCollectorAction;
}

export class CollectorListPage extends React.Component<Props> {
    public get isLoading(): boolean {
        return _.some(this.props.busy, (busy) => busy.action === BusyAction.FetchingCollectors);
    }
    public componentWillMount() {
        if (this.isLoading || (this.props.collectors && this.props.collectors.length)) {
            return;
        }
        this.props.fetchAllCollectors();
    }
    @autobind
    public accept(collector: Collector) {
        this.props.updateCollector({
            ...collector,
            approved: true,
        });
    }
    @autobind
    public reject(collector: Collector) {
        this.props.updateCollector({
            ...collector,
            approved: false,
        });
    }
    public render() {
        return (
            <>
                <h2>Data Collectors</h2>
                <Table striped>
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Status</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.collectors.map((collector) => (
                            <tr key={collector.id}>
                                <td>{collector.name}</td>
                                <td>{
                                    collector.approved === true ?
                                        'Approved' :
                                        collector.approved === false ?
                                            'Rejected' :
                                            'Pending'}
                                </td>
                                <td>
                                    {collector.approved !== true ?
                                        <Button
                                            onClick={() => this.accept(collector)}
                                            className="btn-sm btn-outline-success"
                                        >
                                            <Icon.Check size={12} /> Apporove
                                        </Button> : null}
                                    {collector.approved !== false ?
                                        <Button
                                            onClick={() => this.reject(collector)}
                                            className="btn-sm btn-outline-danger"
                                        >
                                            <Icon.X size={12} /> Reject
                                        </Button> : null}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </>
        );
    }
}

function mapStateToProps(state: AppState, ownProps: Props) {
    return {
        ...ownProps,
        busy: state.busy.busy,
        collectors: state.collector.collectors,
    };
}

function mapDispatchToProps(dispatch: Dispatch<AppState>) {
    return {
        fetchAllCollectors: bindActionCreators(collectorActions.fetchAllCollectors, dispatch),
        updateCollector: bindActionCreators(collectorActions.updateCollector, dispatch),
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(CollectorListPage);
