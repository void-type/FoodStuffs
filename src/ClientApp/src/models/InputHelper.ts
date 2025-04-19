// eslint-disable-next-line @typescript-eslint/ban-types
export function debounce(fn: Function, ms = 300) {
  let debounceTimeoutId: ReturnType<typeof setTimeout>;
  // eslint-disable-next-line func-names
  return function (this: unknown, ...args: unknown[]) {
    clearTimeout(debounceTimeoutId);
    debounceTimeoutId = setTimeout(() => fn.apply(this, args), ms);
  };
}

export function composeFix(event: Event) {
  // Fix for IME input on some Android keyboards
  if (event.target instanceof HTMLInputElement) {
    // eslint-disable-next-line no-param-reassign, @typescript-eslint/no-explicit-any
    (event.target as any).composing = false;
  }
}
