using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Interfaces.Services.Data.Core;

namespace FoodStuffs.Model.Actions.Core.Steps
{
    /// <summary>
    /// Save all changes to data provider.
    /// </summary>
    public class SaveChangesToData : ActionStep
    {
        public SaveChangesToData(IDataService data)
        {
            _data = data;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            _data.SaveChanges();
        }

        private readonly IDataService _data;
    }
}