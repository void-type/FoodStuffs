using Core.Model.Services.DateTime;
using FoodStuffs.Test.Mocks.Action;
using FoodStuffs.Test.Mocks.Services;
using System;

namespace FoodStuffs.Test.Mocks
{
    public static class MockFactory
    {
        public static IDateTimeService EarlyDateTimeService => new DiscreteDateTimeService(new DateTime(2001, 1, 1, 11,
            11, 11));

        public static IDateTimeService LateDateTimeService => new DiscreteDateTimeService(new DateTime(2002, 2, 2, 22,
            22, 22));

        public static TestActionResponder Responder => new TestActionResponder();
    }
}