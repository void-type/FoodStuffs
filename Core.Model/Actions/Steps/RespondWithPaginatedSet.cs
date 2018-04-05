using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.ItemSet;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Actions.Steps
{
    public class RespondWithPaginatedSet<TEntity> : AbstractActionStep
    {
        public RespondWithPaginatedSet(IEnumerable<TEntity> set, int take, int page, string logExtra = null)
        {
            _set = set;
            _take = take;
            _page = page;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var set = _set.Skip((_page - 1) * _take).Take(_take);

            var pagedSet = new PagedItemSet<TEntity>
            {
                Items = set,
                Page = _page,
                Take = _take,
                TotalCount = _set.Count()
            };

            respond.WithItem(pagedSet, $"Page: {_page}, Take: {_take}, TotalCount: {pagedSet.TotalCount}, {_logExtra}");
        }

        private readonly string _logExtra;
        private readonly int _page;
        private readonly IEnumerable<TEntity> _set;
        private readonly int _take;
    }
}