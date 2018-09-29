import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { RouterTestingModule } from '@angular/router/testing';
import { MinersComponent } from './miners.component';

describe('MinersComponent', () => {
    let component: MinersComponent;
    let fixture: ComponentFixture<MinersComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                MinersComponent
            ],
            imports: [
                HttpClientTestingModule,
                RouterTestingModule,
                FontAwesomeModule
            ]
        }).compileComponents();
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
