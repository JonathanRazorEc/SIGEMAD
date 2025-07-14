import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActionsRecordsComponent } from './actions-records.component';

describe('ActionsRecordsComponent', () => {
  let component: ActionsRecordsComponent;
  let fixture: ComponentFixture<ActionsRecordsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActionsRecordsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActionsRecordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
