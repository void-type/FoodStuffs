namespace Core.Model.Services.DateTime
{
    /// <summary>
    /// Used for testing with different times.
    /// </summary>
    public class DiscreteDateTimeService : IDateTimeService
    {
        public System.DateTime Moment { get; }

        public DiscreteDateTimeService(System.DateTime moment)
        {
            Moment = moment;
        }
    }
}