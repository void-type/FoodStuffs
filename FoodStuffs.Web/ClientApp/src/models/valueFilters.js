export default {
  trimAndCapitalize(value) {
    return value.trim()
      .split(" ")
      .filter(word => word.length > 0)
      .map(word => word[0].toUpperCase() + word.substring(1))
      .join(" ");
  },
  limitIntegers(value) {
    value = parseInt(value);

    if (value > 2147483647) {
      value = 2147483647;
    } else if (value < 0) {
      value = 0;
    }
    return value;
  }
}