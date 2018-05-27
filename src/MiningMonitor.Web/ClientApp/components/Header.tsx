import * as React from 'react';
import { Link } from 'react-router-dom';

import * as Icon from 'react-feather';
import { Nav, Navbar, NavItem } from 'reactstrap';

export const Header: React.SFC = () => (
    <>
        <Navbar dark fixed="top" color="primary navbar-expand">
            <Link to="/" className="navbar-brand">
                Mining Monitor
            </Link>
            <Nav navbar className="mr-auto">
                <NavItem>
                    <Link to="/miners" className="nav-link">Miners</Link>
                </NavItem>
            </Nav>
            <Nav navbar >
                <NavItem>
                    <Link to="/admin" className="nav-link icon" title="Server Administration">
                        <Icon.Settings />
                    </Link>
                </NavItem>
            </Nav>
        </Navbar>
    </>
);
