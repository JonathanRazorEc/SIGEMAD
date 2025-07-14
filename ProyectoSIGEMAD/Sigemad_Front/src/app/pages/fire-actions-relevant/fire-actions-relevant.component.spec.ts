import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireActionsRelevantComponent } from './fire-actions-relevant.component';

describe('FireActionsRelevantComponent', () => {
  let component: FireActionsRelevantComponent;
  let fixture: ComponentFixture<FireActionsRelevantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireActionsRelevantComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FireActionsRelevantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
