import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { HeaderComponent } from './header/header.component';
import { MiningMonitorComponent } from './miningmonitor.component';
import { SidebarComponent } from './sidebar/sidebar.component';

describe('MiningMonitorComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                MiningMonitorComponent,
                HeaderComponent,
                SidebarComponent
            ],
            imports: [
                RouterTestingModule
            ]
        }).compileComponents();
    }));
    it('should create the app', async(() => {
        const fixture = TestBed.createComponent(MiningMonitorComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
});
