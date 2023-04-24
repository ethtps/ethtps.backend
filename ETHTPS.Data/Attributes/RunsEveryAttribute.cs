using System;

namespace ETHTPS.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class RunsEveryAttribute : Attribute
    {
        public RunsEveryAttribute(string cronExpression)
        {
            CronExpression = cronExpression;
        }

        public string CronExpression { get; set; }
    }
}
