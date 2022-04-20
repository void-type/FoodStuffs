import moment from 'moment';

function getFormattedMoment(value: Date | string, formatString: string) {
  const instant = moment(value);
  if (instant.isValid() === false) {
    return null;
  }
  return instant.format(formatString);
}

const formatStrings = {
  apiDate: 'YYYY-MM-DD',
  apiDateTime: 'YYYY-MM-DDTHH:mm:ss',
  viewDate: 'L',
  viewDateTime: 'L LT',
};

export function dateForApi(value: Date) {
  return getFormattedMoment(value, formatStrings.apiDate);
}

export function dateTimeForApi(value: Date) {
  return getFormattedMoment(value, formatStrings.apiDateTime);
}

export function dateForView(value: Date) {
  return getFormattedMoment(value, formatStrings.viewDate);
}

export function dateTimeForView(value: Date) {
  return getFormattedMoment(value, formatStrings.viewDateTime);
}
