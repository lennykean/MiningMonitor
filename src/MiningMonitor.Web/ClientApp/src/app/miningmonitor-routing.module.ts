import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { MinersComponent } from './miners/miners.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            { path: '', component: HomeComponent },
            { path: 'miners', component: MinersComponent }
        ])
    ],
    exports: [
        RouterModule
    ],
})
export class MiningMonitorRoutingModule { }
