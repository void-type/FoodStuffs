export default function (value) {
  if(value === null || value === undefined) {
    return null;
  }
  return new Date(value+"Z").toLocaleString();
}