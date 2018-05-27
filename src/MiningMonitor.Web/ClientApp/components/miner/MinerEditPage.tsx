import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { bindActionCreators, Dispatch } from 'redux';

import { Button } from 'reactstrap';

import autobind from 'autobind-decorator';

import * as _ from 'lodash';
import * as Icon from 'react-feather';

import { MinerForm } from '.';
import {
    ClearValidationErrorsAction,
    DeleteMinerAction,
    FetchAllMinersAction,
    minerActions,
    UpdateMinerAction,
    validationActions,
} from '../../actions';
import { Miner } from '../../models';
import { AppState, Busy, BusyAction } from '../../store';

interface Props extends RouteComponentProps<{ id: string }> {
    busy: Busy[];
    miner: Miner;
    validation: { [key: string]: string[] };
    clearValidationErrors: ClearValidationErrorsAction;
    fetchAllMiners: FetchAllMinersAction;
    updateMiner: UpdateMinerAction;
    deleteMiner: DeleteMinerAction;
}

export class MinerEditPage extends React.Component<Props> {
    public get isLoading(): boolean {
        return _.some(this.props.busy, (busy) => busy.action === BusyAction.FetchingMiners);
    }
    public componentWillMount() {
        if (this.isLoading || this.props.miner) {
            return;
        }
        this.props.fetchAllMiners();
    }
    public componentWillUnmount() {
        this.props.clearValidationErrors();
    }
    public componentWillReceiveProps(props: Props) {
        if (props.miner === this.props.miner) {
            return;
        }
        this.props.clearValidationErrors();
    }
    @autobind
    public submit(miner: Miner) {
        this.props.updateMiner(miner);
    }
    @autobind
    public delete() {
        this.props.deleteMiner(this.props.miner, '/miners');
    }
    public render() {
        if (!this.props.miner) {
            return null;
        }
        return (
            <>
                {!this.props.miner.collectorId ?
                    <Button className="float-right" outline color="danger" onClick={this.delete}>Delete Miner</Button> :
                    null}
                <h1>{this.props.miner.name}</h1>
                <Link to={`/monitor/${this.props.miner.id}`} className="btn btn-outline-success">
                    <Icon.Activity size={14} /> Monitor {this.props.miner.name}
                </Link>
                <MinerForm miner={this.props.miner} onSubmit={this.submit} validation={this.props.validation} />
            </>
        );
    }
}

const mapStateToProps = (state: AppState, ownProps: Props) => ({
    ...ownProps,
    busy: state.busy.busy,
    miner: _.find(state.miner.miners, { id: ownProps.match.params.id }),
    validation: state.validation.validation,
} as Props);

const mapDispatchToProps = (dispatch: Dispatch<AppState>) => ({
    clearValidationErrors: bindActionCreators(validationActions.clearValidationErrors, dispatch),
    fetchAllMiners: bindActionCreators(minerActions.fetchAllMiners, dispatch),
    updateMiner: bindActionCreators(minerActions.updateMiner, dispatch),
    deleteMiner: bindActionCreators(minerActions.deleteMiner, dispatch),
} as Props);

export default connect(mapStateToProps, mapDispatchToProps)(MinerEditPage);
