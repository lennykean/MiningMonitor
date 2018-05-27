import * as React from 'react';
import { Route, Switch } from 'react-router';
import { NavLink } from 'react-router-dom';

import { Container, Row } from 'reactstrap';

import { App, Header, LeftNav, LoginPage, Routes } from '.';

export const Main: React.SFC = () => (
    <>
        <Header />
        <main>
            <Switch>
                <Route exact path="/login" component={LoginPage} />
                <Route path="/" component={App} />
            </Switch>
        </main>
    </>
);
