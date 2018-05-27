import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { bindActionCreators, Dispatch } from 'redux';

import * as Icon from 'react-feather';
import { Button, Table } from 'reactstrap';

import * as _ from 'lodash';

import { FetchAllMinersAction, minerActions } from '../../actions';
import { Miner } from '../../models';
import { AppState, Busy } from '../../store';
import { BusyAction } from '../../store/state/BusyAction';

interface Props {
    busy: Busy[];
    miners: Miner[];
    fetchAllMiners: FetchAllMinersAction;
}

export class MinerListPage extends React.Component<Props> {
    public get isLoading(): boolean {
        return _.some(this.props.busy, (busy) => busy.action === BusyAction.FetchingMiners);
    }
    public componentWillMount() {
        if (this.isLoading || (this.props.miners && this.props.miners.length)) {
            return;
        }
        this.props.fetchAllMiners();
    }
    public render() {
        return (
            <>
            <Link className="btn btn-outline-info float-right" to="/miner/new">Add Miner</Link>
            <h1>Miners</h1>
            <Table striped className="miners">
                <thead>
                    <tr>
                        <th></th>
                        <th>Name</th>
                        <th>Data Collection</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.miners.map((miner) => (
                        <tr key={miner.id}>
                            <td title={`Monitor ${miner.name}`}>
                                <Link id="miner-link" to={`/monitor/${miner.id}`}>
                                    <Icon.Activity />
                                </Link>
                            </td>
                            <td title={`Edit ${miner.name}`}>
                                <Link id="miner-link" to={`/miner/${miner.id}`}>
                                    {miner.name}
                                </Link>
                            </td>
                            <td>{miner.collectData ? 'Enabled' : 'Disabled'}</td>
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
        miners: state.miner.miners,
    };
}

function mapDispatchToProps(dispatch: Dispatch<AppState>) {
    return {
        fetchAllMiners: bindActionCreators(minerActions.fetchAllMiners, dispatch),
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(MinerListPage);
