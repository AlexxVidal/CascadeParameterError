using System;
using System.Linq;
using DevExpress.XtraReports.Expressions;

namespace ErrorSample.PredefinedReports
{
    public class ToLocalDateReportCustomFunction : ReportCustomFunctionOperatorBase
    {
        public override string FunctionCategory => "Custom";
        public override int MinOperandCount => 1;
        public override int MaxOperandCount => 1;
        public override string Name => "ToLocalDate";

        public override object Evaluate(params object[] operands)
        {
            var dateTimeOffsets = operands.OfType<DateTimeOffset>().ToArray();

            if (dateTimeOffsets.Length == 0)
                return null;

            return dateTimeOffsets[0].LocalDateTime;
        }

        public override bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            if (operandIndex >= operandCount)
                return false;
            return type == typeof(DateTimeOffset);
        }
    }
}