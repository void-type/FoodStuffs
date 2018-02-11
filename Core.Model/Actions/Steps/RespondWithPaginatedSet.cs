using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.ItemSet;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Actions.Steps
{
    public class RespondWithPaginatedSet<TEntity> : AbstractActionStep
    {
        public RespondWithPaginatedSet(IEnumerable<TEntity> set, int take, int page)
        {
            _set = set;
            _take = take;
            _page = page;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var set = _set.Skip((_page - 1) * _take).Take(_take);

            var pagedSet = new PagedItemSet<TEntity>
            {
                Items = set,
                Page = _page,
                TotalCount = _set.Count()
            };

            respond.WithItem(pagedSet);
        }

        private readonly int _page;
        private readonly IEnumerable<TEntity> _set;
        private readonly int _take;
    }
}