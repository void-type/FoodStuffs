using Core.Model.Services.DateTime;

namespace FoodStuffs.Test.Mocks.Services.DateTime
{
    public class DiscreteDateTimeService : IDateTimeService
    {
        public System.DateTime Moment { get; }

        public DiscreteDateTimeService(System.DateTime moment)
        {
            Moment = moment;
        }
    }
}