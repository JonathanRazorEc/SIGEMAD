import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiSelectAutocompleteComponent } from './multi-select-autocomplete.component';

describe('MultiSelectAutocompleteComponent', () => {
  let component: MultiSelectAutocompleteComponent;
  let fixture: ComponentFixture<MultiSelectAutocompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiSelectAutocompleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiSelectAutocompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
