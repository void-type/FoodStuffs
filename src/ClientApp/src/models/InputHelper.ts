export function debounce<T extends (...args: any[]) => any>(fn: T, ms = 300) {
  let debounceTimeoutId: ReturnType<typeof setTimeout>;

  return function (this: unknown, ...args: Parameters<T>) {
    clearTimeout(debounceTimeoutId);
    debounceTimeoutId = setTimeout(() => fn.apply(this, args), ms);
  };
}

export function composeFix(event: Event) {
  // Fix for IME input on some Android keyboards
  if (event.target instanceof HTMLInputElement) {
    (event.target as any).composing = false;
  }
}
