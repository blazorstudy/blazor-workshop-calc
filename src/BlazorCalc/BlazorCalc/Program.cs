using BlazorCalc.Components;
using BlazorCalc.Components.Calc;

//API 받으려고 사용한 것.
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;

var builder = WebApplication.CreateBuilder(args);
// API 연결을 위해서 HttpClient 사용하기 위해 HttpClient 서비스 추가.

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// CalcState 추가
builder.Services.AddSingleton<CalcState>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


