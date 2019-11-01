import moment from 'moment';

const internal = {
  getFormattedMoment(value, formatString) {
    const instant = moment(value);
    if (instant.isValid() === false) {
      return null;
    }
    return instant.format(formatString);
  },
  formats: {
    apiDate: 'YYYY-MM-DD',
    apiDateTime: 'YYYY-MM-DDTHH:mm:ss',
    viewDate: 'L',
    viewDateTime: 'L LT',
  },
};

export default {
  dateForApi(value) {
    return internal.getFormattedMoment(value, internal.formats.apiDate);
  },
  dateTimeForApi(value) {
    return internal.getFormattedMoment(value, internal.formats.apiDateTime);
  },
  dateForView(value) {
    return internal.getFormattedMoment(value, internal.formats.viewDate);
  },
  dateTimeForView(value) {
    return internal.getFormattedMoment(value, internal.formats.viewDateTime);
  },
};
