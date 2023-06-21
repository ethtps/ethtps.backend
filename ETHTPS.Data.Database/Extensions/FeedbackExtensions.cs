namespace ETHTPS.Data.Integrations.MSSQL.Extensions
{
    public static class FeedbackExtensions
    {
        public static int GetFeedbackTypeID(this EthtpsContext source, string feedbackType, bool createIfItDoesNotExist = true)
        {
            var selector = (Func<FeedbackTypes, bool>)((x) => x.Name.Equals(feedbackType, StringComparison.InvariantCultureIgnoreCase));
            if (!source.FeedbackTypes.Any(selector))
            {
                if (!createIfItDoesNotExist) throw new InvalidOperationException("What am I supposed to do here if I can't return nulls?");
                source.FeedbackTypes.Add(new()
                {
                    Name = feedbackType
                });
                source.SaveChanges();
            }
            return source.FeedbackTypes.First(selector).Id;
        }
    }
}
