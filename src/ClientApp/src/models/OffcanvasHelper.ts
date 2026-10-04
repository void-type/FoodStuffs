import { Offcanvas } from 'bootstrap';

// Bootstrap's `lg` breakpoint. Keep in sync with $grid-breakpoints (theme.scss doesn't override it).
const desktopMinWidthPx = 992;

export default class OffcanvasHelper {
  static closeAllOpen() {
    document.querySelectorAll<HTMLElement>('.offcanvas.show').forEach((el) => {
      Offcanvas.getInstance(el)?.hide();
    });
  }

  // Mobile filter flyouts (offcanvas) have their trigger button hidden at desktop widths,
  // so if one is left open while resizing up to desktop, its controls become unreachable.
  static closeOnDesktopResize() {
    const desktopQuery = window.matchMedia(`(min-width: ${desktopMinWidthPx}px)`);

    desktopQuery.addEventListener('change', (event) => {
      if (event.matches) {
        OffcanvasHelper.closeAllOpen();
      }
    });
  }
}
