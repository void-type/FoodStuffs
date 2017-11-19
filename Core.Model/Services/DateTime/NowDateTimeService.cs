namespace Core.Model.Services.DateTime
{
    public class NowDateTimeService : IDateTimeService
    {
        public System.DateTime Moment => System.DateTime.Now;
    }
}