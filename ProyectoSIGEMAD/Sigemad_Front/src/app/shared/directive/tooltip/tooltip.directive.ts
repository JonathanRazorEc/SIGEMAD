import { Directive, Input } from '@angular/core';
import { MatTooltip } from '@angular/material/tooltip';

@Directive({
  selector: `[appTooltip]`,
  standalone: true,
  hostDirectives: [MatTooltip],
})
export class TooltipDirective {
  @Input('appTooltip')
  set tooltipText(value: string) {
    this.tooltip.message = value;
  }

  constructor(private tooltip: MatTooltip) {}
}
