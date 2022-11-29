using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akhil_CoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("products")]
        public async Task<IActionResult> GetProducts()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:7007/api/v1.0/Products/products");
            var data = await response.Content.ReadAsStringAsync();
            return Ok(data);
        }
    }
}
