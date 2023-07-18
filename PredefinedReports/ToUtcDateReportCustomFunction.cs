using System;
using System.Globalization;
using System.Linq;
using DevExpress.Data.Filtering;

namespace ErrorSample.PredefinedReports
{
    public class ToUtcDateReportCustomFunction : ICustomFunctionOperatorFormattable
    {
        public string Name => "ToUtcDate";

        public Type ResultType(params Type[] operands)
        {
            return typeof(DateTime);
        }

        public object Evaluate(params object[] operands)
        {
            var dateTimes = operands.OfType<DateTime>().ToArray();

            if (dateTimes.Length == 0)
                return null;

            return dateTimes[0].ToUniversalTime();
        }

        public string Format(Type providerType, params string[] operands)
        {
            // Example of operand : "convert(datetime, '2023-02-09 00:00:00', 120)"
            if (!(operands is { Length: 1 }))
                throw new ArgumentException();

            const string dateFormat = "yyyy-MM-dd HH:mm:ss";
            var dateTimeString = operands[0].Substring(operands[0].IndexOf("'", StringComparison.Ordinal) + 1, dateFormat.Length);

            return DateTime.TryParseExact(dateTimeString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dateTime)
                ? operands[0].Replace(dateTimeString, dateTime.ToUniversalTime().ToString(dateFormat))
                : throw new ArgumentException();
        }
    }
}