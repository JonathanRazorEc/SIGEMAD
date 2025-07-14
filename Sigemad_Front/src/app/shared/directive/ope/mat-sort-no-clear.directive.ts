import { Directive, OnInit, OnDestroy } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appMatSortNoClear]',
  standalone: true,
})
export class MatSortNoClearDirective implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();

  constructor(private matSort: MatSort) {}

  ngOnInit() {
    this.subscription = this.matSort.sortChange.subscribe((event: Sort) => {
      if (event.direction === '') {
        this.matSort.active = event.active;
        this.matSort.direction = 'asc';
        this.matSort.sortChange.emit({
          active: this.matSort.active,
          direction: this.matSort.direction,
        });
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
