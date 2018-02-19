using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Timeplan.BL
{
    public static class Utilities
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member.Name;
        }

        public static int DigitPart(string text)
        {
            var result = 0;
            var digitPart = new Regex(@"^\d+", RegexOptions.Compiled);
            int.TryParse(digitPart.Match(text).Value, out result);
            return result != 0 ? result : int.MaxValue;
        }
        
        public static void LogException(string source, string message, EventLogEntryType eventLogEntryType)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                EventLog.WriteEntry(source, message, eventLogEntryType);
            }
        }

    }
}
