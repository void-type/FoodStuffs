import { toInt } from './formatters';

function pluralizeUnit(value: number, single: string, plural: string) {
  return value === 1 ? single : plural;
}

function stringify(value: number, unit: string) {
  return value !== 0 ? `${value} ${unit}` : '';
}

export default class RecipeTimeSpan {
  public totalMinutes: number;

  constructor(minutes = 0, hours = 0) {
    const totalMinutes = toInt(minutes) + toInt(hours) * 60;
    this.totalMinutes = Math.max(totalMinutes, 0);
  }

  toHours() {
    return toInt(this.totalMinutes / 60);
  }

  getHourUnit() {
    return pluralizeUnit(this.toHours(), 'hour', 'hours');
  }

  toMinutes() {
    return this.totalMinutes % 60;
  }

  getMinuteUnit() {
    return pluralizeUnit(this.toMinutes(), 'minute', 'minutes');
  }

  toString() {
    if (this.totalMinutes === 0) {
      return '0 minutes';
    }

    return [
      stringify(this.toHours(), this.getHourUnit()),
      stringify(this.toMinutes(), this.getMinuteUnit()),
    ]
      .filter((v) => v !== null)
      .join(' and ');
  }
}
