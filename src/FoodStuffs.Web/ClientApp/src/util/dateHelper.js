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
    viewDate: 'MM-DD-YYYY',
    viewDateTime: 'MM-DD-YYYY HH:mm:ss',
  },
};

export default {
  dateForApi(value) {
    return internal.getFormattedMoment(value, internal.formats.apiDate);
  },
  dateForView(value) {
    return internal.getFormattedMoment(value, internal.formats.viewDate);
  },
  dateTimeForApi(value) {
    return internal.getFormattedMoment(value, internal.formats.apiDateTime);
  },
  dateTimeForView(value) {
    return internal.getFormattedMoment(value, internal.formats.viewDateTime);
  },
};
