namespace Core.Model.Services.DateTime
{
    /// <summary>
    /// A service for getting the current DateTime.
    /// </summary>
    public class NowDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Returns the current DateTime.
        /// </summary>
        public System.DateTime Moment => System.DateTime.Now;
    }
}