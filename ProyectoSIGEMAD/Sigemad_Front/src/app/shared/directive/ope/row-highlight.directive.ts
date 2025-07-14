import { Directive, ElementRef, HostListener, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appRowHighlight]',
  standalone: true,
})
export class RowHighlightDirective {
  private selectedRow: HTMLElement | null = null;

  @Input() appRowHighlight?: string;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}

  @HostListener('click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const row = target.closest('tr');

    // ⚠️ Si no es una fila o pertenece al thead, ignorar
    if (!row || row.parentElement?.tagName === 'THEAD') {
      return;
    }

    if (this.el.nativeElement.contains(row)) {
      if (this.selectedRow === row) {
        // Si es la misma fila que ya estaba seleccionada, quitar el fondo (toggle off)
        this.renderer.removeStyle(row, 'background-color');
        this.selectedRow = null;
      } else {
        // Si hay otra fila seleccionada, quitarle el fondo
        if (this.selectedRow) {
          this.renderer.removeStyle(this.selectedRow, 'background-color');
        }
        // Marcar la nueva fila seleccionada
        const highlightColor = this.appRowHighlight || '#d0eaff';
        this.renderer.setStyle(row, 'background-color', highlightColor);
        this.selectedRow = row;
      }
    }
  }
}
