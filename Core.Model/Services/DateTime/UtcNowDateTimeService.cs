namespace Core.Model.Services.DateTime
{
    /// <summary>
    /// A service for getting the current DateTime.
    /// </summary>
    public class UtcNowDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Returns the current DateTime.
        /// </summary>
        public System.DateTime Moment => System.DateTime.UtcNow;
    }
}