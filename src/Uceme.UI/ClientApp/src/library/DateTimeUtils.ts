export class DateTimeUtils {
  static TimeToString(dateAsInt: string): string {
    if (!dateAsInt) {
      return dateAsInt;
    }

    let hours = parseInt(dateAsInt);
    let minutes = Math.ceil((parseFloat(dateAsInt) - hours) * 60);

    hours = minutes === 60 ? hours + 1 : hours;
    minutes = minutes === 60 ? 0 : minutes;

    return `${String(hours).padStart(2, '0')}:${String(minutes).padStart(
      2,
      '0'
    )}`;
  }

  static FormatDate(inputDate: string): string {
    if (!inputDate) {
      return inputDate;
    }

    try {
      const dateString = inputDate.toString();
      const year = parseInt(dateString.substring(0, 4));
      const month = parseInt(dateString.substring(4, 6));
      const day = parseInt(dateString.substring(6, 8));

      const today = new Date(year, month - 1, day);
      /* var options = {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      }; */
      return today.toLocaleDateString('es-ES');
    } catch (e) {
      return inputDate;
    }
  }
}

const dateTimeUtils = new DateTimeUtils();

export default dateTimeUtils;
