import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { bindActionCreators, Dispatch } from 'redux';

import autobind from 'autobind-decorator';

import { MinerForm } from '.';
import {
    ClearValidationErrorsAction,
    CreateMinerAction,
    minerActions,
    validationActions,
} from '../../actions';
import { Miner } from '../../models';
import { AppState, Busy, BusyAction } from '../../store';

interface Props extends RouteComponentProps<{ id: string }> {
    busy: Busy[];
    validation: { [key: string]: string[] };
    clearValidationErrors: ClearValidationErrorsAction;
    createMiner: CreateMinerAction;
}

export class MinerCreatePage extends React.Component<Props> {
    public componentWillUnmount() {
        this.props.clearValidationErrors();
    }
    @autobind
    public async submit(miner: Miner) {
        await this.props.createMiner(miner, '/miners');
    }
    public render() {
        return (
            <>
            <h1>Add New Miner</h1>
            <MinerForm onSubmit={this.submit} validation={this.props.validation} />
            </>
        );
    }
}

const mapStateToProps = (state: AppState, ownProps: Props) => ({
    ...ownProps,
    busy: state.busy.busy,
    validation: state.validation.validation,
} as Props);

const mapDispatchToProps = (dispatch: Dispatch<AppState>) => ({
    clearValidationErrors: bindActionCreators(validationActions.clearValidationErrors, dispatch),
    createMiner: bindActionCreators(minerActions.createMiner, dispatch),
} as Props);

export default connect(mapStateToProps, mapDispatchToProps)(MinerCreatePage);
