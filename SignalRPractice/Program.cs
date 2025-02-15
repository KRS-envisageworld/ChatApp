using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SignalRPractice.Hubs;
using SignalRPractice.Services;

using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ChatService>();
builder.Services.AddSignalR(e =>
{
	e.EnableDetailedErrors = true;
	e.MaximumReceiveMessageSize = 102400000;
});
//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowSpecificOrigins", builder =>
//	{
//		builder
//				.SetIsOriginAllowedToAllowWildcardSubdomains()
//				.SetPreflightMaxAge(TimeSpan.FromHours(24))
//				.WithOrigins("http://localhost:4200")
//				.AllowAnyMethod()
//				.AllowAnyHeader()
//				.AllowCredentials()
//				.Build();
//	});
//});
var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();


app.MapControllers();
app.MapHub<ChatHub>("techtalk");
app.MapFallbackToController("Index", "Fallback");

app.Run();