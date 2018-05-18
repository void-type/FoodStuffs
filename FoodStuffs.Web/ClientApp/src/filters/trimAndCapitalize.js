export default function (value) {
  return value
    .trim()
    .split(' ')
    .filter(word => word.length > 0)
    .map(word => word[0].toUpperCase() + word.substring(1))
    .join(' ');
}
