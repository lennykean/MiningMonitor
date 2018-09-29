import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ChartsModule } from 'ng2-charts';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';

import { AdminComponent } from './admin/admin.component';
import { AlertBadgeComponent } from './alerts/alert-badge/alert-badge.component';
import { AlertDetailComponent } from './alerts/alert-detail/alert-detail.component';
import { AlertDefinitionsComponent } from './alerts/alert-definitions/alert-definitions.component';
import { AlertsComponent } from './alerts/alerts/alerts.component';
import { AuthInterceptor } from './auth.interceptor';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { MainComponent } from './main/main.component';
import { MinerCreateComponent } from './miner/miner-create/miner-create.component';
import { MinerEditComponent } from './miner/miner-edit/miner-edit.component';
import { MinerFormComponent } from './miner/miner-form/miner-form.component';
import { MinersComponent } from './miner/miners/miners.component';
import { MiningMonitorComponent } from './miningmonitor.component';
import { MiningMonitorRoutingModule } from './miningmonitor-routing.module';
import { MonitorComponent } from './miner/monitor/monitor.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SettingsComponent } from './admin/settings/settings.component';
import { UsersComponent } from './admin/users/users.component';
import { CollectorsComponent } from './admin/collectors/collectors.component';
import { UserCreateComponent } from './admin/user-create/user-create.component';
import { GpuChartComponent } from './miner/gpu-chart/gpu-chart.component';
import { MinerNamePipe } from './miner-name.pipe';

library.add(fas);

@NgModule({
    declarations: [
        AdminComponent,
        AlertBadgeComponent,
        AlertDefinitionsComponent,
        AlertDetailComponent,
        AlertsComponent,
        CollectorsComponent,
        HeaderComponent,
        HomeComponent,
        MainComponent,
        MinerCreateComponent,
        MinerEditComponent,
        MinerFormComponent,
        MinerNamePipe,
        MinersComponent,
        MiningMonitorComponent,
        MonitorComponent,
        LoginComponent,
        LogoutComponent,
        SidebarComponent,
        SettingsComponent,
        UsersComponent,
        UserCreateComponent,
        GpuChartComponent,
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        ChartsModule,
        FontAwesomeModule,
        MiningMonitorRoutingModule
    ],
    providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true
    }],
    bootstrap: [MiningMonitorComponent]
})
export class MiningMonitorModule { }
