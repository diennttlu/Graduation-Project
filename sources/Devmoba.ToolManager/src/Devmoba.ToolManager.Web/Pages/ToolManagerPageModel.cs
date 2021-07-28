using Devmoba.ToolManager.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Devmoba.ToolManager.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class ToolManagerPageModel : AbpPageModel
    {
        private static readonly JsonSerializerSettings CamelCaseSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        protected ToolManagerPageModel()
        {
            LocalizationResourceType = typeof(ToolManagerResource);
        }

        protected string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, CamelCaseSerializerSettings);
        }
    }
}