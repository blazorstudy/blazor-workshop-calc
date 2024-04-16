using BlazorCalc.Components;
using BlazorCalc.Components.Calc;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// API 연결을 위해서 HttpClient 사용하기 위해 추가.
builder.Services.AddHttpClient();
string currentDate = DateTime.Now.ToString("yyyyMMdd");

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri($" https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=0MKRDFz4g6nOf8Id5lBUgxCfceeVrud1&searchdate={currentDate}&data=AP01")
    });
Console.WriteLine("api success");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// CalcState 추가
builder.Services.AddSingleton<CalcState>();

//API로 가져갈 환율 정보 추가
builder.Services.AddSingleton<GetData1>();

var app = builder.Build();

//API로 부터 데이터 받아오기
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;

var httpClient = services.GetRequiredService<HttpClient>();

HttpResponseMessage response = await httpClient.GetAsync("");


if (response.IsSuccessStatusCode)
{
    string jsonResponse = await response.Content.ReadAsStringAsync();

    // JSON 데이터 파싱
    var exchangeRates = JsonSerializer.Deserialize<ExchangeRate[]>(jsonResponse);

    // 필터링 및 값 추출
    foreach (var rate in exchangeRates)
    {
        if (rate.result == 1 && rate.cur_unit == "USD")
        {
            var dataService = services.GetRequiredService<GetData1>();
            dataService.Rate = exchangeRates.FirstOrDefault(r => r.result == 1 && r.cur_unit == "USD");
            Console.WriteLine($"deal_bas_r: {rate.deal_bas_r}");

        }
    }
}
else
{
    Console.WriteLine("API 호출 실패.");
}

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


//JSON 데이터에 대응하는 환율 클래스 선언.
public class ExchangeRate
{
    public int result { get; set; }
    public string cur_unit { get; set; }
    public string ttb { get; set; }
    public string tts { get; set; }
    public string deal_bas_r { get; set; }
    public string bkpr { get; set; }
    public string yy_efee_r { get; set; }
    public string ten_dd_efee_r { get; set; }
    public string kftc_bkpr { get; set; }
    public string kftc_deal_bas_r { get; set; }
    public string cur_nm { get; set; }

}