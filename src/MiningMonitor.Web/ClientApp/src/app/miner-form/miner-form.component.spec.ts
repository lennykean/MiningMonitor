import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { MinerFormComponent } from './miner-form.component';

describe('MinerFormComponent', () => {
    let component: MinerFormComponent;
    let fixture: ComponentFixture<MinerFormComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [MinerFormComponent],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MinerFormComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
