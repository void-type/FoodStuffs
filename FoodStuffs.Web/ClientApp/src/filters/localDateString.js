export default function (value) {
  return new Date(value+"Z").toLocaleString();
}