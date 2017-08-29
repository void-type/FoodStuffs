using FoodStuffs.Model.Interfaces.Services.DateTime;
using System;

namespace FoodStuffs.Test.Mocks.Services
{
    public class DiscreteDateTimeService : IDateTimeService
    {
        public DiscreteDateTimeService(DateTime moment)
        {
            Now = moment;
        }

        public DateTime Now { get; }
    }
}