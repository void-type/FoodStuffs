using FoodStuffs.Model.Interfaces.Services.DateTime;

namespace FoodStuffs.Web.Services.DateTime
{
    public class NowDateTimeService : IDateTimeService
    {
        public System.DateTime Moment => System.DateTime.Now;
    }
}