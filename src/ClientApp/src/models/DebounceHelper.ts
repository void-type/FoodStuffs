// eslint-disable-next-line @typescript-eslint/ban-types
export default function debounce(fn: Function, ms = 300) {
  let debounceTimeoutId: ReturnType<typeof setTimeout>;
  // eslint-disable-next-line func-names
  return function (this: unknown, ...args: unknown[]) {
    clearTimeout(debounceTimeoutId);
    debounceTimeoutId = setTimeout(() => fn.apply(this, args), ms);
  };
}
