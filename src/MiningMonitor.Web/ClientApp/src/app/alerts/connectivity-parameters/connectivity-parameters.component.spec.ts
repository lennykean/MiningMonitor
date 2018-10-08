import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { ConnectivityParametersComponent } from './connectivity-parameters.component';

describe('ConnectivityParametersComponent', () => {
    let component: ConnectivityParametersComponent;
    let fixture: ComponentFixture<ConnectivityParametersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ConnectivityParametersComponent],
            imports: [
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ConnectivityParametersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
