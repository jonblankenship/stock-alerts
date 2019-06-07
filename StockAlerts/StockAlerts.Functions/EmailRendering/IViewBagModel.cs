using System.Dynamic;

namespace StockAlerts.Functions.EmailRendering
{
    // https://github.com/lukencode/FluentEmail/blob/8682007fbbf30669f2d35334a55bc3003568c1f0/src/Renderers/FluentEmail.Razor/IViewBagModel.cs
    public interface IViewBagModel
    {
        ExpandoObject ViewBag { get; }
    }
}
