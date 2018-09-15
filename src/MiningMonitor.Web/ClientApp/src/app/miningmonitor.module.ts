import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AuthInterceptor } from './auth.interceptor';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { MiningMonitorComponent } from './miningmonitor.component';
import { MiningMonitorRoutingModule } from './miningmonitor-routing.module';
import { MinersComponent } from './miners/miners.component';
import { SidebarComponent } from './sidebar/sidebar.component';

@NgModule({
    declarations: [
        HeaderComponent,
        HomeComponent,
        MiningMonitorComponent,
        SidebarComponent,
        MinersComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
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
