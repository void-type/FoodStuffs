namespace Core.Model.Services.DateTime
{
    /// <summary>
    /// Used for testing with explicit times.
    /// </summary>
    public class DiscreteDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Returns the explicit moment set on construction.
        /// </summary>
        public System.DateTime Moment { get; }

        public DiscreteDateTimeService(System.DateTime moment)
        {
            Moment = moment;
        }
    }
}