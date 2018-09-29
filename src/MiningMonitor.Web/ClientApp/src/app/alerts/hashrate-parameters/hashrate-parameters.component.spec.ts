import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { HashrateParametersComponent } from './hashrate-parameters.component';

describe('HashrateParametersComponent', () => {
    let component: HashrateParametersComponent;
    let fixture: ComponentFixture<HashrateParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [HashrateParametersComponent],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(HashrateParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
