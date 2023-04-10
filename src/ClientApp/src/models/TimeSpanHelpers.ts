import { isNil, toInt } from '@/models/FormatHelpers';

function stringifyUnit(value: number, unitSingle: string, unitPlural = '') {
  const unit = value === 1 || isNil(unitPlural) ? unitSingle : unitPlural;
  return value !== 0 ? `${value} ${unit}` : '';
}

export function clampHours(totalMinutes: number) {
  return toInt(totalMinutes / 60);
}

export function clampMinutes(totalMinutes: number) {
  return totalMinutes % 60;
}

export function getTotalMinutes(minutes = 0, hours = 0) {
  const totalMinutes = toInt(minutes) + toInt(hours) * 60;
  return Math.max(totalMinutes, 0);
}

export function toTimeSpanString(totalMinutes: number) {
  if (!totalMinutes) {
    return '0 minutes';
  }

  return [
    stringifyUnit(clampHours(totalMinutes), 'hour', 'hours'),
    stringifyUnit(clampMinutes(totalMinutes), 'minute', 'minutes'),
  ]
    .filter((v) => v !== null && v !== '')
    .join(' and ');
}
