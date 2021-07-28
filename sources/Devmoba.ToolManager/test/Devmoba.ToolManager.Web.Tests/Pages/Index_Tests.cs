using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Devmoba.ToolManager.Pages
{
    public class Index_Tests : ToolManagerWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
