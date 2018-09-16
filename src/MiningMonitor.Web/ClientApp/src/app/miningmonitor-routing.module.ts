import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MainComponent } from './main/main.component';
import { MinerCreateComponent } from './miner-create/miner-create.component';
import { MinerEditComponent } from './miner-edit/miner-edit.component';
import { MinersComponent } from './miners/miners.component';
import { MonitorComponent } from './monitor/monitor.component';
import { LogoutComponent } from './logout/logout.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {
                path: '',
                component: MainComponent,
                canActivate: [AuthGuard],
                children: [
                    { path: '', component: HomeComponent },
                    { path: 'admin', component: AdminComponent },
                    { path: 'miner/new', component: MinerCreateComponent },
                    { path: 'miner/:id', component: MinerEditComponent },
                    { path: 'miners', component: MinersComponent },
                    { path: 'monitor/:id', component: MonitorComponent },
                ]
            },
            { path: 'login', component: LoginComponent },
            { path: 'logout', component: LogoutComponent }
        ])
    ],
    exports: [
        RouterModule
    ],
})
export class MiningMonitorRoutingModule { }
