import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MinersComponent } from './miners.component';

describe('MinersComponent', () => {
    let component: MinersComponent;
    let fixture: ComponentFixture<MinersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                MinersComponent
            ]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MinersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
