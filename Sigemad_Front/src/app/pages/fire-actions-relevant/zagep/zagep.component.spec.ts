import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ZagepComponent } from './zagep.component';

describe('ZagepComponent', () => {
  let component: ZagepComponent;
  let fixture: ComponentFixture<ZagepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ZagepComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ZagepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
