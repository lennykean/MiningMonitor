import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AdminComponent } from './admin/admin.component';
import { CollectorsComponent } from './admin/collectors/collectors.component';
import { SettingsComponent } from './admin/settings/settings.component';
import { UserCreateComponent } from './admin/user-create/user-create.component';
import { UsersComponent } from './admin/users/users.component';
import { AlertDefinitionCreateComponent } from './alerts/alert-definition-create/alert-definition-create.component';
import { AlertDefinitionEditComponent } from './alerts/alert-definition-edit/alert-definition-edit.component';
import { AlertDefinitionsComponent } from './alerts/alert-definitions/alert-definitions.component';
import { AlertDetailComponent } from './alerts/alert-detail/alert-detail.component';
import { AlertsComponent } from './alerts/alerts/alerts.component';
import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { MainComponent } from './main/main.component';
import { MinerCreateComponent } from './miner/miner-create/miner-create.component';
import { MinerEditComponent } from './miner/miner-edit/miner-edit.component';
import { MinersComponent } from './miner/miners/miners.component';
import { MonitorComponent } from './miner/monitor/monitor.component';

@NgModule({
  imports: [
    RouterModule.forRoot([
      {
        path: '',
        component: MainComponent,
        canActivate: [AuthGuard],
        runGuardsAndResolvers: 'always',
        children: [
          { path: '', component: HomeComponent },
          {
            path: 'admin',
            component: AdminComponent,
            children: [
              { path: 'collectors', component: CollectorsComponent },
              { path: 'settings', component: SettingsComponent },
              { path: 'users', component: UsersComponent },
              { path: 'users/new', component: UserCreateComponent },
            ],
          },
          { path: 'alert/:id', component: AlertDetailComponent },
          {
            path: 'alertdefinition/new',
            component: AlertDefinitionCreateComponent,
          },
          {
            path: 'alertdefinition/:id',
            component: AlertDefinitionEditComponent,
          },
          { path: 'alertdefinitions', component: AlertDefinitionsComponent },
          { path: 'alerts', component: AlertsComponent },
          { path: 'miner/new', component: MinerCreateComponent },
          { path: 'miner/:id', component: MinerEditComponent },
          { path: 'miners', component: MinersComponent },
          { path: 'monitor', component: MonitorComponent },
          { path: 'monitor/:id', component: MonitorComponent },
        ],
      },
      { path: 'login', component: LoginComponent },
      { path: 'logout', component: LogoutComponent },
    ]),
  ],
  exports: [RouterModule],
})
export class MiningMonitorRoutingModule {}
