import { FormatFechaCESTPipe } from './format-fecha-cest.pipe';

describe('FormatFechaCESTPipe', () => {
  let pipe: FormatFechaCESTPipe;

  beforeEach(() => {
    pipe = new FormatFechaCESTPipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should format UTC date string to CEST correctly', () => {
    const utcString = '2025-05-22T07:00:00Z'; // equivale a 09:00 CEST
    const formatted = pipe.transform(utcString);
    expect(formatted).toBe('22/05/2025 09:00');
  });

  it('should return empty string when value is undefined or empty', () => {
    expect(pipe.transform(undefined)).toBe('');
    expect(pipe.transform('')).toBe('');
  });

  it('should respect a custom format', () => {
    const utcString = '2025-05-22T07:00:00Z';
    const custom = pipe.transform(utcString, 'DD-MM-YYYY HH:mm:ss');
    expect(custom).toBe('22-05-2025 09:00:00');
  });
});
