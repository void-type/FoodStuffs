export function clamp(value, min, max) {
  return Math.max(min, Math.min(value, max));
}

export function numberOrDefault(value, defaultValue = 0) {
  const number = Number(value);
  return !Number.isNaN(number) ? number : defaultValue;
}

export function toInt(value) {
  return Math.floor(numberOrDefault(value), 0);
}
