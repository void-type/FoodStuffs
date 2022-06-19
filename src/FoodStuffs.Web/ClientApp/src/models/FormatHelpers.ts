export function clamp(value: number, min: number, max: number) {
  return Math.max(min, Math.min(value, max));
}

export function toNumber(value: string | number | undefined | null, defaultValue = 0) {
  const number = Number(value);
  return !Number.isNaN(number) ? number : defaultValue;
}

export function toInt(value: string | number | undefined | null, defaultValue = 0) {
  return Math.floor(toNumber(value, defaultValue));
}

export function trimAndTitleCase(value: string) {
  return value
    .trim()
    .split(' ')
    .filter((word) => word.length > 0)
    .map((word) => word[0].toUpperCase() + word.substring(1).toLowerCase())
    .join(' ');
}

export function isNil(value: string | null | undefined) {
  return value === null || value === undefined || value === '';
}
