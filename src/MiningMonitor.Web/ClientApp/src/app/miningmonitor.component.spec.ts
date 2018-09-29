import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { AlertBadgeComponent } from './alerts/alert-badge/alert-badge.component';
import { HeaderComponent } from './header/header.component';
import { MiningMonitorComponent } from './miningmonitor.component';
import { SidebarComponent } from './sidebar/sidebar.component';

describe('MiningMonitorComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertBadgeComponent,
                HeaderComponent,
                MiningMonitorComponent,
                SidebarComponent
            ],
            imports: [
                HttpClientTestingModule,
                RouterTestingModule,
                FontAwesomeModule
            ]
        }).compileComponents();
    }));
    it('should create the app', async(() => {
        const fixture = TestBed.createComponent(MiningMonitorComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
});
