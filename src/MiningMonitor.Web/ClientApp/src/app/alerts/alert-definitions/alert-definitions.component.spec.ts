import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { MinerNamePipe } from '../../miner-name.pipe';
import { AlertDefinitionsComponent } from './alert-definitions.component';

describe('AlertDefinitionsComponent', () => {
    let component: AlertDefinitionsComponent;
    let fixture: ComponentFixture<AlertDefinitionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDefinitionsComponent,
                MinerNamePipe
            ],
            imports: [
                HttpClientTestingModule,
                RouterTestingModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDefinitionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
