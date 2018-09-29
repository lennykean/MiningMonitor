import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertDefinitionEditComponent } from './alert-definition-edit.component';

describe('AlertDefinitionEditComponent', () => {
  let component: AlertDefinitionEditComponent;
  let fixture: ComponentFixture<AlertDefinitionEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlertDefinitionEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlertDefinitionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
