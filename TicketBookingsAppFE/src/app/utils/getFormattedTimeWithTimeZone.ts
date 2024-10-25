export function getFormattedTimeWithTimezone(dateString: string): string {
  const date = new Date(dateString);

  // Get the full time string with long timezone name (e.g., "India Standard Time")
  const timeWithLongTimezone = date.toLocaleTimeString('en-US', {
    timeZoneName: 'long',
  });

  // Get the time string with short timezone name (e.g., "GMT+5:30")
  const timeWithShortTimezone = date.toLocaleTimeString('en-US', {
    timeZoneName: 'short',
  });

  // Extract just the short timezone (e.g., "GMT+5:30") from the second string
  const shortTimezone = timeWithShortTimezone.slice(
    timeWithShortTimezone.indexOf('GMT')
  );

  return `${timeWithLongTimezone} ${shortTimezone}`;
}
