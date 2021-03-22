export class DateTimeUtils {
  static TimeToString(dateAsInt: string): string {
    if (!dateAsInt) {
      return dateAsInt;
    }

    var hours = parseInt(dateAsInt);
    var minutes = Math.ceil((parseFloat(dateAsInt) - hours) * 60);

    hours = minutes === 60 ? hours + 1 : hours;
    minutes = minutes === 60 ? 0 : minutes;

    return (
      String(hours).padStart(2, '0') + ':' + String(minutes).padStart(2, '0')
    );
  }

  static FormatDate(inputDate: string): string {
    if (!inputDate) {
      return inputDate;
    }

    try {
      var dateString = inputDate.toString();
      var year = parseInt(dateString.substring(0, 4));
      var month = parseInt(dateString.substring(4, 6));
      var day = parseInt(dateString.substring(6, 8));

      var today = new Date(year, month - 1, day);
      var options = {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      };
      return today.toLocaleDateString('es-ES');
    } catch (e) {
      debugger;
      return inputDate;
    }
  }
}

const dateTimeUtils = new DateTimeUtils();

export default dateTimeUtils;
