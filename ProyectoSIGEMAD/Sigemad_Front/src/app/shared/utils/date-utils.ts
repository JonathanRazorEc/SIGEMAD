import moment from 'moment';

const getCurrentCESTDate = (format = 'yyyy-MM-DDTHH:mm'): string => {
  return moment().local().format(format);
};

const getCurrentUTCDate = (): string => {
  return new Date().toISOString();
};

const fromCestToUtc = (date: string | Date): string | null => {
  return date ? new Date(date).toISOString() : null;
};

const fromUtcToCest = (date: string | Date, format = 'YYYY-MM-DDTHH:mm'): string | null => {
  return date ? moment.utc(date).local().format(format) : null;
};

const formatDate = (date: string | Date, format: string) => {
  return moment(date).format(format);
};

const truncarFecha = (date: string | Date | number): number => {
  const d = new Date(date);
  d.setHours(0, 0, 0, 0);
  return d.getTime();
};

export const DateUtils = {
  getCurrentCESTDate,
  getCurrentUTCDate,
  fromCestToUtc,
  fromUtcToCest,
  formatDate,
  truncarFecha,
};
