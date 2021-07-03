import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ChartsModule } from 'ng2-charts';

import { AdminComponent } from './admin/admin.component';
import { CollectorsComponent } from './admin/collectors/collectors.component';
import { SettingsComponent } from './admin/settings/settings.component';
import { UserCreateComponent } from './admin/user-create/user-create.component';
import { UsersComponent } from './admin/users/users.component';
import { AlertActionDetailComponent } from './alerts/alert-action-detail/alert-action-detail.component';
import { AlertActionsComponent } from './alerts/alert-actions/alert-actions.component';
import { AlertBadgeComponent } from './alerts/alert-badge/alert-badge.component';
import { AlertDefinitionCreateComponent } from './alerts/alert-definition-create/alert-definition-create.component';
import { AlertDefinitionEditComponent } from './alerts/alert-definition-edit/alert-definition-edit.component';
import { AlertDefinitionFormComponent } from './alerts/alert-definition-form/alert-definition-form.component';
import { AlertDefinitionParametersComponent } from './alerts/alert-definition-parameters/alert-definition-parameters.component';
import { AlertDefinitionsComponent } from './alerts/alert-definitions/alert-definitions.component';
import { AlertDetailComponent } from './alerts/alert-detail/alert-detail.component';
import { AlertsComponent } from './alerts/alerts/alerts.component';
import { ConnectivityParametersComponent } from './alerts/connectivity-parameters/connectivity-parameters.component';
import { GpuThresholdParametersComponent } from './alerts/gpu-threshold-parameters/gpu-threshold-parameters.component';
import { HashrateParametersComponent } from './alerts/hashrate-parameters/hashrate-parameters.component';
import { AuthInterceptor } from './auth.interceptor';
import { EnumPipe } from './enum.pipe';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { HumanizePipe } from './humanize.pipe';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { MainComponent } from './main/main.component';
import { MinerNamePipe } from './miner-name.pipe';
import { GpuChartComponent } from './miner/gpu-chart/gpu-chart.component';
import { MinerChartComponent } from './miner/miner-chart/miner-chart.component';
import { MinerCreateComponent } from './miner/miner-create/miner-create.component';
import { MinerEditComponent } from './miner/miner-edit/miner-edit.component';
import { MinerFormComponent } from './miner/miner-form/miner-form.component';
import { MinersComponent } from './miner/miners/miners.component';
import { MonitorComponent } from './miner/monitor/monitor.component';
import { MiningMonitorRoutingModule } from './miningmonitor-routing.module';
import { MiningMonitorComponent } from './miningmonitor.component';
import { SidebarComponent } from './sidebar/sidebar.component';

@NgModule({
    declarations: [
        AdminComponent,
        AlertActionsComponent,
        AlertBadgeComponent,
        AlertDefinitionCreateComponent,
        AlertDefinitionEditComponent,
        AlertDefinitionFormComponent,
        AlertDefinitionParametersComponent,
        AlertDefinitionsComponent,
        AlertDetailComponent,
        AlertsComponent,
        CollectorsComponent,
        ConnectivityParametersComponent,
        EnumPipe,
        GpuChartComponent,
        MinerChartComponent,
        GpuThresholdParametersComponent,
        HashrateParametersComponent,
        HeaderComponent,
        HomeComponent,
        HumanizePipe,
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
        AlertActionDetailComponent,
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
