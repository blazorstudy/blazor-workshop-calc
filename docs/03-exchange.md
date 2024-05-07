# 세션 03. 환율 계산기 만들기

이 세션에서는 오늘의 환율을 한국수출입은행 OpenAPI로 얻어 환율을 계산해 봅니다.

## 0. API란?

1. API : Application Progamming Interface의 줄임말입니다.
   - 어떠한 응용 프로그램에서 데이터를 주고 받기 위한 방법으로,
   - Interface : 어떤 장치끼리의 정보를 교환하기 위한 수단과 방법을 의미합니다.
   - ex) 회사 내의 서버 데이터와 "인터넷을 사용해 회사 웹사이트를 보는 사용자" 간의 상호작용이 가능하게 중간 다리 역할을 해줍니다.

## 1. OpenAPI 가져오기

[한국수출입은행의 OpenAPI](https://www.koreaexim.go.kr/ir/HPHKIR020M01?apino=2&viewtype=C&searchselect=&searchword=)를 가져옵니다.

- OpenAPI란?

  - 누구나 사용할 수 있도록 공개된 API (회사에서 공개 가능한 API를 누구 사용할 수 있게 함)
  - public API 라고 할 수 있습니다.

- 한국수출입은행 OpenAPI 요청 흐름

  - 요청변수 (사용자 설정)

    : authkey, searchdate, data

  - URL 형태로 razor 파일 내에서 API 요청

    : (예시) https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=AUTHKEY1234567890&searchdate=20180102&data=AP01

    : 사용자가 설정한 요청변수 값을 URL에 넣으면 값을 얻을 수 있습니다.

**지금부터 함께 OpenAPI를 가져와봅시다!**

## 2. CalcExchangerate.razor 파일에서 실습하기

1. appsettings.development.json에서 OpenAPI 인증키 변수 추가

- BlazorCalc/appsettings.Development.json 위치로 이동합니다.

- appsettings.Development.json 파일

  - ASP.NET Core 웹 애플리케이션의 개발 환경 설정 파일입니다.
  - razor 파일에 직접 인증키를 넣을 수 있지만 이는 보안상의 위험이 있어, 설정 파일에 정보를 저장합니다.
  - 개발 환경 설정 파일에 API 인증키를 넣어줍니다.

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "ApiAuthKey": "0MKRDFz4g6nOf8Id5lBUgxCfceeVrud1"
    }
    ```

---

<br/>

2. API를 불러오고 데이터를 처리하기 위한 클래스를 추가합니다.

   BlazorCalc/Components/Pages/CalcExchangerate.razor 위치로 이동합니다.

   환율 계산기 페이지에서 필요한 여러 서비스와 클래스를 가져옵니다.

   ```cs
    @page "/calc-ExchangeRate"
    @rendermode InteractiveServer
    @inject IConfiguration Configuration
    @inject HttpClient HttpClient

    @using System.Net.Http
    @using System.Text.Json;
    @using System.Threading.Tasks;
   ```

- @page "/calc-ExchangeRate"
  - Razor 페이지의 경로를 지정합니다.
- @rendermode InteractiveServer
  - 사용자와 상호작용하는 동안 서버에 요청을 보내어 렌더링 합니다.
- @inject IConfiguration Configuration
  - 개발 환경 설정 파일(appsettings.json 등)에 저장된 구성 값을 가져오는데 사용됩니다.
  - inject : 서비스의 기능 또는 데이터에 접근하게 합니다, 즉, razor 페이지에서 사용할 변수를 선언해줍니다.
- @inject HttpClient HttpClient
  - HttpClient 서비스를 사용합니다.
- 나머지
  - System 네임스페이스를 사용할 수 있게 가져옵니다. (기본적인 시스템 기능과 관련된 클래스와 기능들을 포함하는 네임스페이스 입니다.)
  - Using : C#의 네임스페이스를 페이지에 가져옵니다. 네임스페이스에 속한 클래스를 사용할 수 있게 합니다.

---

<br/>

3. ExchangeRate class 만들기

   API를 요청했을 때, 받은 정보를 저장할 class를 만들어줍니다

   ```cs
   public class ExchangeRate
   {
       public int result { get; set; }
       public string cur_unit { get; set; }
       public string cur_nm { get; set; }
       public string ttb { get; set; }
       public string tts { get; set; }
       public string deal_bas_r { get; set; }
       public string bkpr { get; set; }
       public string yy_efee_r { get; set; }
       public string ten_dd_efee_r { get; set; }
       public string kftc_deal_bas_r { get; set; }
       public string kftc_bkpr { get; set; }
   }
   ```

---

<br/>

4. 사용할 변수 추가

   ```cs
   private string User_Select_Currency { get; set; } = "USD";
   private float ValueToDisplay { get; set; } = 0;
   private float ValueUserClicked { get; set; } = 0;

   private float ConvertToCal_USD {get; set;} = 0;
   private float ConvertToCal_JPY { get; set; } = 0;
   private float ConvertToCal_EUR { get; set; } = 0;

   private string UnitWON { get; set; } = "\u20A9";
   private string UnitChanged { get; set; } = "$";

   private string CurrencyValue_USD { get; set; } = "";
   private string CurrencyValue_JPY { get; set; } = "";
   private string CurrencyValue_EUR { get; set; } = "";
   ```

---

<br/>

5. getExchangeJson 함수 추가하기

   API를 불러와서 각 변수에 해당하는 값을 할당해줍시다. private 함수로 다른 클래스에서 접근 불가하다고 생각하면 됩니다. 비동기 메서드로 해당 함수가 호출될 때 다른 작업도 수행가능합니다. Task는 .NET에서 비동기 작업을 나타내는 클래스입니다.

   - 비동기(Asynchronous) vs 동기(Synchronous)
     - 비동기식 작업
       1. 한 작업이 완료되기를 기다리지 않고 다음 작업을 시작할 수 있습니다.
       2. 보통 비동기 메서드를 호출하고 await 를 사용하여 비동기 메서드들 사이에서 해당 작업이 완료될 때까지 기다립니다.
     - 동기식 작업
       1. 순차적으로 진행되면서, 한 작업이 완료되기 전까지 다음 작업은 시작되지 않습니다.
       2. 매서드를 호출하고 해당 메서드가 완료될 때까지 기다립니다.

   ```cs
   private async Task getExchangeJson() {

   }
   ```

    <br/>

   currentDate 변수에 DateTime.Now.ToString 메서드를 사용하여 오늘의 날짜를 저장합니다. authKey에는 1. 에서 개발 환경 설정 파일에 저장해놓은 API 인증키 값을 저장합니다. requestUrl에 API 요청 URL을 저장합니다.

   ```cs
   string currentDate = DateTime.Now.ToString("yyyyMMdd");
   string authKey = Configuration["ApiAuthKey"] ?? string.Empty;
   string requestUrl = $"https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey={authKey}&searchdate={currentDate}&data=AP01";
   ```

    <br/>

   requestUrl로부터 API를 요청합니다. 요청이 성공하면 받아온 값을 ExchangeRate 클래스 형태로 저장합니다.

   ```cs
   var exchangeRates = await HttpClient.GetFromJsonAsync<ExchangeRate[]>(requestUrl);
   ```

    <br/>

   받아온 데이터로 부터 usd, eur, jpy 변수에 각각 달러, 유로, 엔화 데이터를 저장합니다. (exchangeRates 에서 괄호 안 조건을 만족하는 첫번째 값을 저장합니다.)

   ```cs
   var usd = exchangeRates?.FirstOrDefault(r => r.result == 1 && r.cur_unit == "USD");
   var eur = exchangeRates?.FirstOrDefault(r => r.result == 1 && r.cur_unit == "EUR");
   var jpy = exchangeRates?.FirstOrDefault(r => r.result == 1 && r.cur_unit == "JPY(100)");
   ```

    <br/>

   가져온 정보가 null 아닌 유효한 정보라면, 환율 계산기 페이지에 사용할 수 있게 각 변수에 오늘의 환율 정보를 할당합니다.

   ```cs
   if (usd != null)
   {
       ConvertToCal_USD = float.Parse(usd.deal_bas_r);
       CurrencyValue_USD = $"1$ = {usd.deal_bas_r}₩";
   }
   else
   {
       Console.WriteLine("USD 정보를 가져올 수 없습니다.");
   }
   if (eur != null)
   {
       ConvertToCal_EUR = float.Parse(eur.deal_bas_r);
       CurrencyValue_EUR = $"1€ = {eur.deal_bas_r}\u20A9";
   }
   else
   {
       Console.WriteLine("EUR 정보를 가져올 수 없습니다.");
   }
   if (jpy != null)
   {
       ConvertToCal_JPY = float.Parse(jpy.deal_bas_r);
       CurrencyValue_JPY = $"100¥ = {jpy.deal_bas_r}\u20A9";
   }
   else
   {
       Console.WriteLine("JPY 정보를 가져올 수 없습니다.");
   }
   ```

---

<br/>

6. OnInitialized 함수 추가하기

   환율 페이지를 클릭했을 때 바로 오늘의 환율을 표시해야합니다. 따라서, 페이지가 시작할 때 자동으로 호출되는 OnInitializedAsync() 함수를 오버라이딩해서 위에서 추가했던 getExchageJson() 함수를 페이지가 시작될 때 호출합니다.

   ```cs
       protected override async Task OnInitializedAsync()
       {
           await getExchangeJson();
       }
   ```

---

<br/>

7. clickButton 함수 추가

   버튼을 클릭 했을 때, 값을 바꿔주고, API를 호출하며, 사용자가 선택한 콤보박스 값에 맞추어 값을 바꿔줍니다.

   ```cs
   private async void clickButton(int value){
       ValueUserClicked = ValueUserClicked * 10 + value;
       await getExchangeJson();
       switch (User_Select_Currency)
       {
           case "USD":
               ValueToDisplay = ValueUserClicked/ConvertToCal_USD;
               break;
           case "JP":
               ValueToDisplay= ValueUserClicked/ConvertToCal_JPY*100;
               break;
           case "EUR":
               ValueToDisplay= ValueUserClicked/ConvertToCal_EUR;
               break;
           default:
               Console.WriteLine("Not supported");
               break;
       }
       StateHasChanged();
       writeStatus();
   }
   ```

---

<br/>

8. wirteStatus 함수 추가

   받아온 값과 환율을 계산해서 보여지는 값을 콘솔창에서 확인하기 위해 writeStatus() 함수를 추가해서 확인합니다.

   ```cs
   private void writeStatus()
   {
       Console.WriteLine($"ValueToDisplay[{this.ValueToDisplay}], USDcurrency[{this.CurrencyValue_USD}], EURcurrency[{this.CurrencyValue_EUR}], JPYcurrency[{this.CurrencyValue_JPY}]");
   }
   ```

---

<br/>

9. selectBox 함수 추가

   콤보박스 값이 바뀌었을 때 호출되는 함수를 만들어 봅시다. 사용자가 콤보박스 값을 바꿀 때 UnitChanged 값을 바꾸고, 사용자가 입력한 값과 변환한 값을 모두 초기화 해줍니다.

   ```cs
       private async Task selectBox(ChangeEventArgs e)
       {
           User_Select_Currency = e.Value.ToString();
           switch (User_Select_Currency)
           {
               case "USD":
                   UnitChanged = "$";
                   ValueToDisplay = 0;
                   ValueUserClicked = 0;
                   break;
               case "JP":
                   UnitChanged = "¥";
                   ValueToDisplay = 0;
                   ValueUserClicked = 0;
                   break;
               case "EUR":
                   UnitChanged = "€";
                   ValueToDisplay = 0;
                   ValueUserClicked = 0;
                   break;
               default:
                   UnitChanged = " ";
                   ValueToDisplay = 0;
                   ValueUserClicked = 0;
                   break;
           }
           await Task.CompletedTask;
       }
   ```

---

<br/>

10. clickClear 함수 추가

    "c"를 눌렀을 때 모든 값을 초기화할 수 있는 함수를 추가해줍니다

    ```cs
        private void clickClear()
        {
            ValueToDisplay = 0;
            ValueUserClicked = 0;
            writeStatus();
        }
    ```

<h3>환율 계산 페이지를 OpenAPI를 이용해 만들어 보았습니다!</h3>
<br/>

### 참고 레퍼런스

---

1. [inject 지시문](https://eodevelop.tistory.com/86)
2. [system 네임스페이스](https://learn.microsoft.com/ko-kr/dotnet/api/system?view=net-8.0)
3. [task 클래스](https://learn.microsoft.com/ko-kr/dotnet/api/system.threading.tasks.task-1?view=net-8.0)
4. [비동기 vs 동기 프로그래밍 예시](https://learn.microsoft.com/ko-kr/dotnet/csharp/asynchronous-programming/)
5. [API](https://helloworld-88.tistory.com/21)
6. [API 추가 설명](https://velog.io/@gil0127/API%EB%9E%80-%EA%B0%9C%EB%85%90-%EC%A0%95%EB%A6%AC%EC%99%80-%ED%8F%AC%ED%8A%B8%ED%8F%B4%EB%A6%AC%EC%98%A4%EC%97%90-%EC%9C%A0%EC%9A%A9%ED%95%9C-%EB%8C%80%EB%B0%95-%EC%82%AC%EC%9D%B4%ED%8A%B8-%EA%B3%B5%EC%9C%A0)
