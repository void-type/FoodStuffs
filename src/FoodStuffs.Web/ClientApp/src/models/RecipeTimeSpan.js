import { toInt } from './formatters';

function pluralizeUnit(value, single, plural) {
  return value === 1 ? single : plural;
}

function stringify(value, unit) {
  return value !== 0 ? `${value} ${unit}` : null;
}

export default class RecipeTimeSpan {
  constructor(minutes = 0, hours = 0) {
    const totalMinutes = toInt(minutes) + toInt(hours) * 60;
    this.totalMinutes = Math.max(totalMinutes, 0);
  }

  toHours() {
    return toInt(this.totalMinutes / 60);
  }

  hourUnit() {
    return pluralizeUnit(this.toHours(), 'hour', 'hours');
  }

  minuteUnit() {
    return pluralizeUnit(this.toMinutes(), 'minute', 'minutes');
  }

  toMinutes() {
    return this.totalMinutes % 60;
  }

  totalMinutes() {
    return this.totalMinutes;
  }

  toString() {
    if (this.totalMinutes === 0) {
      return '0 minutes';
    }

    return [
      stringify(this.toHours(), this.hourUnit()),
      stringify(this.toMinutes(), this.minuteUnit()),
    ]
      .filter((v) => v !== null)
      .join(' and ');
  }
}
