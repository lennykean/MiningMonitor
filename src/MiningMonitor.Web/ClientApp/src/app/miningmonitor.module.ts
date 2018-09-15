import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';

import { AuthInterceptor } from './auth.interceptor';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { MiningMonitorComponent } from './miningmonitor.component';
import { MiningMonitorRoutingModule } from './miningmonitor-routing.module';
import { MinerEditComponent } from './miner-edit/miner-edit.component';
import { MinerFormComponent } from './miner-form/miner-form.component';
import { MinersComponent } from './miners/miners.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { MainComponent } from './main/main.component';
import { LoginComponent } from './login/login.component';
import { MinerCreateComponent } from './miner-create/miner-create.component';

library.add(fas);

@NgModule({
    declarations: [
        HeaderComponent,
        HomeComponent,
        LoginComponent,
        MainComponent,
        MiningMonitorComponent,
        MinerEditComponent,
        MinerFormComponent,
        MinersComponent,
        SidebarComponent,
        MinerCreateComponent
    ],
    imports: [
        BrowserModule,
        FontAwesomeModule,
        FormsModule,
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
