import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertActionDetailComponent } from './alert-action-detail.component';

describe('AlertActionDetailComponent', () => {
  let component: AlertActionDetailComponent;
  let fixture: ComponentFixture<AlertActionDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlertActionDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlertActionDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
