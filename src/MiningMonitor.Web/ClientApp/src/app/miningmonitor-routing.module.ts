import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MinerCreateComponent } from './miner-create/miner-create.component';
import { MinerEditComponent } from './miner-edit/miner-edit.component';
import { MinersComponent } from './miners/miners.component';
import { MainComponent } from './main/main.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {
                path: '',
                component: MainComponent,
                canActivate: [AuthGuard],
                children: [
                    { path: '', component: HomeComponent },
                    { path: 'miner/new', component: MinerCreateComponent },
                    { path: 'miner/:id', component: MinerEditComponent },
                    { path: 'miners', component: MinersComponent }
                ]
            },
            {
                path: 'login',
                component: LoginComponent
            }
        ])
    ],
    exports: [
        RouterModule
    ],
})
export class MiningMonitorRoutingModule { }
