using Core.Model.Actions.Responder;
using Core.Model.Data;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Save all changes to persistence.
    /// </summary>
    public class SaveChangesToData : AbstractActionStep
    {
        public SaveChangesToData(IPersistable data)
        {
            _data = data;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            _data.SaveChanges();
        }

        private readonly IPersistable _data;
    }
}