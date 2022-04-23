export default class FormatHelpers {
  static clamp(value: number, min: number, max: number) {
    return Math.max(min, Math.min(value, max));
  }

  static numberOrDefault(value: string | number, defaultValue = 0) {
    const number = Number(value);
    return !Number.isNaN(number) ? number : defaultValue;
  }

  static toInt(value: string | number) {
    return Math.floor(this.numberOrDefault(value));
  }

  static trimAndTitleCase(value: string) {
    return value
      .trim()
      .split(' ')
      .filter((word) => word.length > 0)
      .map((word) => word[0].toUpperCase() + word.substring(1).toLowerCase())
      .join(' ');
  }
}
