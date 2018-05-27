import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { bindActionCreators, Dispatch } from 'redux';

import autobind from 'autobind-decorator';

import { UserForm } from '.';
import {
    ClearValidationErrorsAction,
    CreateUserAction,
    userActions,
    validationActions,
} from '../../actions';
import { User } from '../../models';
import { AppState, Busy, BusyAction } from '../../store';

interface Props {
    busy: Busy[];
    validation: { [key: string]: string[] };
    clearValidationErrors: ClearValidationErrorsAction;
    createUser: CreateUserAction;
}

export class UserCreatePage extends React.Component<Props> {
    public componentWillUnmount() {
        this.props.clearValidationErrors();
    }
    @autobind
    public async submit(user: User) {
        await this.props.createUser(user, '/admin/users');
    }
    public render() {
        return (
            <>
                <h2>Add New User</h2>
                <UserForm onSubmit={this.submit} validation={this.props.validation} />
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
    createUser: bindActionCreators(userActions.createUser, dispatch),
} as Props);

export default connect(mapStateToProps, mapDispatchToProps)(UserCreatePage);
