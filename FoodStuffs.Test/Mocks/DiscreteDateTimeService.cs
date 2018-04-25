using Core.Model.Services.DateTime;

namespace FoodStuffs.Test.Mocks
{
    /// <summary>
    /// Used for injeccting an explicit time.
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