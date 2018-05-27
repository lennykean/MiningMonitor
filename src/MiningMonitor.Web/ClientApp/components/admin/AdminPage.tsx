import * as React from 'react';
import { Route, Switch } from 'react-router';

import { AdminHeader, CollectorsListPage, SettingsPage, UserCreatePage, UserListPage } from '.';

export class AdminPage extends React.Component {
    public render() {
        return (
            <>
                <AdminHeader />
                <Switch>
                    <Route exact path="/admin/collectors" component={CollectorsListPage} />
                    <Route exact path="/admin/settings" component={SettingsPage} />
                    <Route exact path="/admin/users" component={UserListPage} />
                    <Route exact path="/admin/users/new" component={UserCreatePage} />
                </Switch>
            </>
        );
    }
}
