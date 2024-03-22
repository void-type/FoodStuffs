using System.Runtime.CompilerServices;
using VoidCore.Model.Events;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Events;

public abstract class CustomEventHandlerAbstract<TRequest, TResponse> : EventHandlerAbstract<TRequest, TResponse>
{
    protected string GetTag(object? specification = null, [CallerMemberName] string methodName = "unknown")
    {
        var specName = specification != null ?
            $"({specification.GetType().GetFriendlyTypeName()})" :
            string.Empty;

        return $"EF query called from: {GetType().GetFriendlyTypeName()}.{methodName}{specName}";
    }
}
