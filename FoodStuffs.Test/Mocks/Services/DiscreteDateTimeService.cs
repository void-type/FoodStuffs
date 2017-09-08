using FoodStuffs.Model.Interfaces.Services.DateTime;
using System;

namespace FoodStuffs.Test.Mocks.Services
{
    public class DiscreteDateTimeService : IDateTimeService
    {
        public DiscreteDateTimeService(DateTime moment)
        {
            Moment = moment;
        }

        public DateTime Moment { get; }
    }
}