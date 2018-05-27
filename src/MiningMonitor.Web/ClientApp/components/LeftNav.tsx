import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { bindActionCreators } from 'redux';

import { Nav, Navbar, NavItem } from 'reactstrap';

import * as _ from 'lodash';

import { FetchAllMinersAction, minerActions } from '../actions';
import { Miner } from '../models';
import { AppState, Busy, BusyAction } from '../store';

interface Props {
    busy?: Busy[];
    miners?: Miner[];
    fetchAllMiners?: FetchAllMinersAction;
}

export class LeftNav extends React.Component<Props> {
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
                <Navbar light color="light" className="left-nav">
                    <Nav navbar>
                        <h3>
                            <NavLink exact to="/miners" className="nav-link">
                                Miners
                        </NavLink>
                        </h3>
                        {this.props.miners.map((miner) => (
                            <NavItem key={miner.id}>
                                <NavLink exact to={`/miner/${miner.id}`} className="nav-link">
                                    <span title={miner.name}>{miner.name}</span>
                                </NavLink>
                            </NavItem>
                        ))}
                    </Nav>
                </Navbar>
            </>
        );
    }
}

function mapStateToProps(state: AppState, ownProps: Props) {
    return {
        ...ownProps,
        miners: state.miner.miners,
        busy: state.busy.busy,
    } as Props;
}

function mapDispatchToProps(dispatch: Dispatch<AppState>) {
    return {
        fetchAllMiners: bindActionCreators(minerActions.fetchAllMiners, dispatch),
    } as Props;
}

export default connect(mapStateToProps, mapDispatchToProps, null, { pure: false })(LeftNav);
