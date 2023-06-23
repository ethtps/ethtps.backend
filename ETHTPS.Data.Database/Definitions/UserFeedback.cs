// ReSharper disable file CheckNamespace

using ETHTPS.Data.Integrations.MSSQL.Extensions;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class UserFeedback
{
    public UserFeedback() { }

    public UserFeedback(EthtpsContext sourceContext) : this(sourceContext, "UserFeedback")
    {

    }

    public UserFeedback(EthtpsContext sourceContext, string feedbackType)
    {
        this.Type = sourceContext.GetFeedbackTypeID(feedbackType);
    }
}