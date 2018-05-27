import * as React from 'react';
import { Route, Switch } from 'react-router';

import { AdminPage, HomePage, LoginPage, MinerCreatePage, MinerEditPage, MinerListPage, MinerMonitorPage } from '.';

export const Routes: React.SFC = () => (
    <Switch>
        <Route exact path="/" component={HomePage} />
        <Route exact path="/miners" component={MinerListPage} />
        <Route exact path="/miner/new" component={MinerCreatePage} />
        <Route exact path="/miner/:id" component={MinerEditPage} />
        <Route exact path="/monitor/:id" component={MinerMonitorPage} />
        <Route exact path="/login" component={LoginPage} />
        <Route path="/admin" component={AdminPage} />
    </Switch>
);
