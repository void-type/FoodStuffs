using FoodStuffs.Model.Interfaces.Services.DateTime;

namespace FoodStuffs.Web.Services.DateTime
{
    public class NowDateTimeService : IDateTimeService
    {
        public System.DateTime Now => System.DateTime.Now;
    }
}