import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { MinerFormComponent } from '../miner-form/miner-form.component';
import { MinerEditComponent } from './miner-edit.component';

describe('MinerEditComponent', () => {
    let component: MinerEditComponent;
    let fixture: ComponentFixture<MinerEditComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                MinerEditComponent,
                MinerFormComponent
            ],
            imports: [
                FormsModule,
                HttpClientTestingModule,
                RouterTestingModule,
                FontAwesomeModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MinerEditComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
