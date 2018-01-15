using Core.Model.Actions.Responder;
using Core.Model.Services.Data;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Save all changes to data provider.
    /// </summary>
    public class SaveChangesToData : AbstractActionStep
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