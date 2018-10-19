import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { EnumPipe } from '../../enum.pipe';
import { HumanizePipe } from '../../humanize.pipe';
import { AlertActionDetailComponent } from '../alert-action-detail/alert-action-detail.component';
import { AlertActionsComponent } from './alert-actions.component';

describe('AlertActionsComponent', () => {
    let component: AlertActionsComponent;
    let fixture: ComponentFixture<AlertActionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AlertActionDetailComponent,
                AlertActionsComponent,
                EnumPipe,
                HumanizePipe
            ],
            imports: [
                FontAwesomeModule,
                FormsModule
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AlertActionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
