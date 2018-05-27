import * as React from 'react';
import { NavLink } from 'react-router-dom';

import { Nav, NavItem } from 'reactstrap';

export class AdminHeader extends React.Component {
    public render() {
        return (
            <>
                <h1>Administration</h1>
                <Nav tabs className="admin-tabs">
                    <NavItem>
                        <NavLink to="/admin/settings" className="nav-link">Settings</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink to="/admin/users" className="nav-link">Users</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink to="/admin/collectors" className="nav-link">Collectors</NavLink>
                    </NavItem>
                </Nav>
            </>
        );
    }
}
