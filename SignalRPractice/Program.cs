using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SignalRPractice.EntityContext;
using SignalRPractice.Hubs;
using SignalRPractice.Interfaces;
using SignalRPractice.Services;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(jsonOption => {
	jsonOption.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
	jsonOption.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
	jsonOption.JsonSerializerOptions.DefaultIgnoreCondition= JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("ChatDatabase"));
});
builder.Services.AddSingleton<ChatService>();
builder.Services.AddTransient<IUser,UserService>();
builder.Services.AddTransient<IToken, TokenService>();

builder.Services.AddSignalR(e =>
{
	e.EnableDetailedErrors = true;
	e.MaximumReceiveMessageSize = 102400000;
});
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigins", builder =>
	{
		builder
				.SetIsOriginAllowedToAllowWildcardSubdomains()
				.SetPreflightMaxAge(TimeSpan.FromHours(24))
				.WithOrigins("http://localhost:4200")
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				.Build();
	});
});


var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();


app.MapControllers();
app.MapHub<ChatHub>("techtalk");
//app.MapFallbackToController("Index", "Fallback");

app.Run();