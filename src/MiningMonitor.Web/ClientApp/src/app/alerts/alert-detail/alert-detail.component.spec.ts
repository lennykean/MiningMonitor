import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { MinerNamePipe } from '../../miner-name.pipe';
import { AlertDetailComponent } from './alert-detail.component';

describe('AlertDetailComponent', () => {
    let component: AlertDetailComponent;
    let fixture: ComponentFixture<AlertDetailComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertDetailComponent,
                MinerNamePipe
            ],
            imports: [
                HttpClientTestingModule,
                RouterTestingModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertDetailComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
