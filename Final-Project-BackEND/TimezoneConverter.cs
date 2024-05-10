namespace Final_Project_BackEND
{
    public class TimezoneConverter
    {
        private readonly TimeZoneInfo _defaultTimeZone;

        public TimezoneConverter(TimeZoneInfo defaultTimeZone)
        {
            _defaultTimeZone = defaultTimeZone;
        }

        public DateTime ConvertToDefaultTimeZone(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            DateTime convertedDateTime = TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, _defaultTimeZone);

            // Adjust the year to Thai Buddhist year
            int thaiBuddhistYear = convertedDateTime.Year + 543;
            DateTime thaiBuddhistDateTime = new DateTime(thaiBuddhistYear, convertedDateTime.Month, convertedDateTime.Day);

            return thaiBuddhistDateTime;
        }
    }
}
