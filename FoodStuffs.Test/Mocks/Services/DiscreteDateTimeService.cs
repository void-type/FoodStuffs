using Core.Model.Services.DateTime;
using System;

namespace FoodStuffs.Test.Mocks.Services
{
    public class DiscreteDateTimeService : IDateTimeService
    {
        public DateTime Moment { get; }

        public DiscreteDateTimeService(DateTime moment)
        {
            Moment = moment;
        }
    }
}