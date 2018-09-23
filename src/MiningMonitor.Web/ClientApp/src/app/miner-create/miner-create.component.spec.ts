import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { MinerCreateComponent } from './miner-create.component';
import { MinerFormComponent } from '../miner-form/miner-form.component';

describe('MinerCreateComponent', () => {
    let component: MinerCreateComponent;
    let fixture: ComponentFixture<MinerCreateComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                MinerCreateComponent,
                MinerFormComponent
            ],
            imports: [
                FormsModule,
                HttpClientTestingModule,
                RouterTestingModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MinerCreateComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
