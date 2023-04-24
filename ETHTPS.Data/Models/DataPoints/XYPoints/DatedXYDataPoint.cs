using System;

using ETHTPS.Data.Core.Extensions;

namespace ETHTPS.Data.Core.Models.DataPoints.XYPoints
{
    public class DatedXYDataPoint : XYDataPointBase<DateTime>
    {
        public DatedXYDataPoint() : base() { }
        public DatedXYDataPoint(DateTime x, double y) : base(x, y)
        {

        }

        public override DatedXYDataPoint ToDatedXYDataPoint() => this;
        public override NumericXYDataPoint ToNumericXYDataPoint() => new(X.ToUnixTime(), Y);
        public override StringXYDataPoint ToStringXYDataPoint() => new(X.ToString(), Y);
    }
}
