import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { bindActionCreators, Dispatch } from 'redux';

import * as Icon from 'react-feather';
import { Button, Table } from 'reactstrap';

import * as _ from 'lodash';

import { FetchAllUsersAction, userActions } from '../../actions';
import { User } from '../../models';
import { AppState, Busy } from '../../store';
import { BusyAction } from '../../store/state/BusyAction';

interface Props {
    busy: Busy[];
    users: User[];
    fetchAllUsers: FetchAllUsersAction;
}

export class UserListPage extends React.Component<Props> {
    public get isLoading(): boolean {
        return _.some(this.props.busy, (busy) => busy.action === BusyAction.FetchingUsers);
    }
    public componentWillMount() {
        if (this.isLoading || (this.props.users && this.props.users.length)) {
            return;
        }
        this.props.fetchAllUsers();
    }
    public render() {
        return (
            <>
                <Link className="btn btn-outline-info float-right" to="/admin/users/new">Add User</Link>
                <h2>Users</h2>
                <Table striped>
                    <thead>
                        <tr>
                            <th>Username</th>
                            <th>E-Mail</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.users.map((user) => (
                            <tr key={user.username}>
                                <td>{user.username}</td>
                                <td>{user.email}</td>
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
        users: state.user.users,
    };
}

function mapDispatchToProps(dispatch: Dispatch<AppState>) {
    return {
        fetchAllUsers: bindActionCreators(userActions.fetchAllUsers, dispatch),
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(UserListPage);
