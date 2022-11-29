using Akhil_CoreWebApp.Areas.Identity.Data;
using Akhil_CoreWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Akhil_CoreWebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'Akhil_CoreWebAppContextConnection' not found.");

builder.Services.AddDbContext<Akhil_CoreWebAppContext>(options =>
    options.UseSqlServer(connectionString,options => options.EnableRetryOnFailure()));

builder.Services.AddDefaultIdentity<Akhil_CoreWebAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<Akhil_CoreWebAppContext>();

// Add services to the container.
builder.Services.AddAuthorization();

/*Creates a JWT Token for Authentication - START*/
/*========================================================================================================*/
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my Secret Token Key"));
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = key
        };
    });
/*========================================================================================================*/
/*Creates a JWT Token for Authentication - END*/

builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.Run();
