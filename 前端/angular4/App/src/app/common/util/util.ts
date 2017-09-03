export function toInteger(value: any): number {
  return parseInt(`${value}`, 10);
}

export function toString(value: any): string {
  return (value !== undefined && value !== null) ? `${value}` : '';
}

export function getValueInRange(value: number, max: number, min = 0): number {
  return Math.max(Math.min(value, max), min);
}

export function isString(value: any): value is string {
  return typeof value === 'string';
}

export function isNumber(value: any): value is number {
  return !isNaN(toInteger(value));
}

export function isInteger(value: any): value is number {
  return typeof value === 'number' && isFinite(value) && Math.floor(value) === value;
}

export function isDefined(value: any): boolean {
  return value !== undefined && value !== null;
}

export function padNumber(value: number) {
  if (isNumber(value)) {
    return `0${value}`.slice(-2);
  } else {
    return '';
  }
}

export function regExpEscape(text) {
  return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&');
}

export class String {
  public static Empty: string = "";

  public static IsNullOrWhiteSpace(value: string): boolean {
    try {
      if (value == null || value == 'undefined')
        return true;

      return value.toString().replace(/\s/g, '').length < 1;
    }
    catch (e) {
      console.log(e);
      return false;
    }
  }

  public static Format(format, ...args): string {
    try {
      return format.replace(/{(\d+(:\w*)?)}/g, function (match, i) { //0
        var s = match.split(':');
        if (s.length > 1) {
          i = i[0];
          match = s[1].replace('}', ''); //U
        }

        var arg = String.parsePattern(match, args[i]);
        return typeof arg != 'undefined' && arg != null ? arg : String.Empty;
      });
    }
    catch (e) {
      console.log(e);
      return String.Empty;
    }
  }

  private static parsePattern(match, arg): string {
    if (arg == null || arg == undefined)
      return arg;

    switch (match) {
      case 'L':
        arg = arg.toLowerCase();
        break;
      case 'U':
        arg = arg.toUpperCase();
        break;
      case 'd':
        var splitted = arg.split('-');
        if (splitted.length <= 1)
          return arg;

        var day = splitted[splitted.length - 1];
        var month = splitted[splitted.length - 2];
        var year = splitted[splitted.length - 3];
        day = day.split('T')[0];
        day = day.split(' ')[0];

        arg = day + '.' + month + '.' + year;
        break;
      case 's':
        var splitted = arg.replace(',', '').split('.');
        if (splitted.length <= 1)
          return arg;

        var time = splitted[splitted.length - 1].split(' ');
        if (time.length > 1)
          time = time[time.length - 1];

        var year = splitted[splitted.length - 1].split(' ')[0];
        var month = splitted[splitted.length - 2];
        var day = splitted[splitted.length - 3];

        arg = year + "-" + month + "-" + day;
        if (time.length > 1)
          arg += "T" + time;
        else
          arg += "T" + "00:00:00";
        break;
      case 'n': //Tausender Trennzeichen
        if (isNaN(parseInt(arg)) || arg.length <= 3)
          break;

        arg = arg.toString();
        var mod = arg.length % 3;
        var output = (mod > 0 ? (arg.substring(0, mod)) : String.Empty);
        for (var i = 0; i < Math.floor(arg.length / 3); i++) {
          if ((mod == 0) && (i == 0))
            output += arg.substring(mod + 3 * i, mod + 3 * i + 3);
          else
            output += '.' + arg.substring(mod + 3 * i, mod + 3 * i + 3);
        }
        arg = output;
        break;
      default:
        break;
    }

    return arg;
  }

}

export class StringBuilder {
  public Values = [];

  constructor(value: string = String.Empty) {
    this.Values = new Array(value);
  }

  public ToString() {
    return this.Values.join('');
  }
  public Append(value: string) {
    this.Values.push(value);
  }
  public AppendFormat(value: string, ...args) {
    this.Values.push(String.Format(value, args));
  }
  public Clear() {
    this.Values = [];
  }
}