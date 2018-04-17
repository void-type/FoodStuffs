export default function(value) {
  value = parseInt(value);

  if (value > 2147483647) {
    value = 2147483647;
  } else if (value < 0) {
    value = 0;
  }
  return value;
}