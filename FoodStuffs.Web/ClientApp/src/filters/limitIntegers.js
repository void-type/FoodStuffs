export default function (value) {
  const intValue = parseInt(value, 10);

  if (intValue > 2147483647) {
    return 2147483647;
  } else if (intValue < 0) {
    return 0;
  }

  return intValue;
}
