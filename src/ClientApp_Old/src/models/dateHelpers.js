import moment from 'moment';

function getFormattedMoment(value, formatString) {
  const instant = moment(value);
  if (instant.isValid() === false) {
    return null;
  }
  return instant.format(formatString);
}

const formats = {
  apiDate: 'YYYY-MM-DD',
  apiDateTime: 'YYYY-MM-DDTHH:mm:ss',
  viewDate: 'L',
  viewDateTime: 'L LT',
};

export function dateForApi(value) {
  return getFormattedMoment(value, formats.apiDate);
}

export function dateTimeForApi(value) {
  return getFormattedMoment(value, formats.apiDateTime);
}

export function dateForView(value) {
  return getFormattedMoment(value, formats.viewDate);
}

export function dateTimeForView(value) {
  return getFormattedMoment(value, formats.viewDateTime);
}
