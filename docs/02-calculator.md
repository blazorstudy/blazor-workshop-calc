# 세션 02: 계산기 만들기

이 세션에서는 Blazor로 계산기 만들기를 진행합니다.
<br/><br/>
## 1. Blazor 개발환경 구축하기

블레이저를 시작하려면 [blazor.net](https://blazor.net)의 안내를 따라하세요.

* [git 설치하기](https://git-scm.com/downloads)
* [Visual Studio Code 설치하기](https://code.visualstudio.com/download)
* [C# Dev Kit 익스텐션 설치하기](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
* [.NET SDK 8.0 설치하기](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
<br/><br/>
## 2. CalcPage.razor 파일에서 실습하기
### 1. 시작하기

원하는 경로에 레포지토리를 복사하고
```
$ git clone https://github.com/blazorstudy/blazor-workshop-calc.git
```

"save-points" 폴더에 각 세션별로 준비가 되어 있습니다. [session2](https://github.com/blazorstudy/blazor-workshop-calc/tree/main/save-points/session2)를 찾아보세요
<br/><br/>
### 2. 페이지 라우팅 경로, 렌더링 모드 설정하기

save-points/session2/BlazorCalc/Components/Pages/CalcPage.razor 위치로 이동합니다.

```
@page "/calc-page"
@rendermode InteractiveServer
```
`CalcPage.razor` 파일 상단에 위 내용을 추가해줍니다.


- @page "/calc-page"

  - Razor 페이지의 경로를 지정합니다.

- @rendermode InteractiveServer

  - 사용자와 상호작용하는 동안 서버에 요청을 보내어 렌더링 합니다.
<br/><br/>
### 3. 스타일 적용하기

계산 결과가 표시되는 부분과, 계산기 버튼의 스타일을 적용해줍니다.

```
<style>
    .display-calc {
        width: 320px;
        text-align: right;
        font-size: 32px;
    }

    .button-number {
        height: 80px;
        width: 80px;
        background-color: lightskyblue;
        font-size: 24px;
    }
</style>

```
- .display-calc {...} : 계산 결과가 표시되는 부분에 대한 CSS 클래스
- .button-number {...} : 계산기 버튼에 대한 CSS 클래스  
<br/><br/>
### 4. 계산기 UI

계산기의 UI 부분 코드를 추가해봅시다.

```
<h3>CalcPage</h3>

<label class="display-calc">@this.DisplayValue</label>

<div style="display:flex">
    <button class="button-number" @onclick="() => clickButton(7)">7</button>
    <button class="button-number" @onclick="() => clickButton(8)">8</button>
    <button class="button-number" @onclick="() => clickButton(9)">9</button>
    <button class="button-number" @onclick="clickClear">C</button>
</div>

<div style="display:flex">
    <button class="button-number" @onclick="() => clickButton(4)">4</button>
    <button class="button-number" @onclick="() => clickButton(5)">5</button>
    <button class="button-number" @onclick="() => clickButton(6)">6</button>
    <button class="button-number" @onclick="clickPlus">+</button>
</div>

<div style="display:flex">
    <button class="button-number" @onclick="() => clickButton(1)">1</button>
    <button class="button-number" @onclick="() => clickButton(2)">2</button>
    <button class="button-number" @onclick="() => clickButton(3)">3</button>
    <button class="button-number" @onclick="clickMinus">-</button>
</div>

<div style="display:flex">
    <button class="button-number"></button>
    <button class="button-number" @onclick="() => clickButton(0)">0</button>
    <button class="button-number"></button>
    <button class="button-number" @onclick="clickResult">=</button>
</div>
```
- `<label class="display-calc">@this.DisplayValue</label>` : 계산기의 출력 화면을 나타내는 레이블
  
  - 위에 추가했던 `display-calc`라는 CSS 클래스가 여기 적용됩니다.


- `<button class="button-number" @onclick="() => clickButton(N)">N</button>` : 0부터 9까지의 숫자를 나타내며, 사용자가 숫자를 입력할 때 클릭하는 버튼

  - 마찬가지로 각 버튼에는 위에 추가했던 `button-number`라는 CSS 클래스가 적용됩니다.
    
  - `@onclick` : Blazor에서 사용되는 이벤트 처리기 지시문
    
    -  버튼이 클릭되었을 때 실행될 이벤트 핸들러를 지정합니다.
    
  - `() => clickButton(N)` : 이 람다식은 버튼이 클릭되었을 때 clickButton(N) 메서드를 호출하도록 지정합니다.
  
  > 함수를 정의하고 해당 함수를 참조하는 대신, 위와 같이이 직접 람다식으로 함수를 정의하여 사용할 수 있습니다. 코드가 더 간결해지고, 이벤트 핸들러의 목적이 명확해집니다.
<br/>
- 연산자 버튼 및 초기화 버튼 : 덧셈(+), 뺄셈(-), 등호(=), 초기화(C) 버튼은 클릭 이벤트에 따라 해당 기능을 수행하는 C# 메서드가 정의되어 있습니다.
<br/><br/>

### 5. 각 메서드 정의하기

이제 `@code{...}` 안의 내용을 채워보겠습니다.

```
    private enum CalcOp
    {
        None,
        Plus,
        Minus,
    }
    private int DisplayValue { get; set; } = 0;
    private int StoredValue { get; set; } = 0;
    private CalcOp op = CalcOp.None;
```

- `enum CalcOp` : 계산기의 연산자를 나타냅니다. Plus(덧셈), Minus(뺄셈), None(연산자 없음) 세 가지 값을 가질 수 있습니다.

- `DisplayValue` : 현재 사용자에게 표시되는 숫자를 나타냅니다.

- `StoredValue` : 현재 계산기에 저장된 숫자를 나타냅니다.

- `op` : 현재 선택된 연산자를 나타냅니다. Plus가 선택되면 덧셈이, Minus가 선택되면 뺄셈이 수행됩니다. 위 코드에서 초기값으로 None(연산자 없음)이 들어가있습니다.

---
1. clickButton(int value) 메서드 추가하기
   
숫자 버튼이 클릭되었을 때 호출되는 메서드입니다. 

```
    private void clickButton(int value)
    {
        DisplayValue = DisplayValue * 10 + value;

        writeStatus();
    }
```

현재 DisplayValue에 사용자가 클릭한 숫자를 추가합니다.


---
2. clickPlus() 메서드 추가하기

덧셈 버튼이 클릭되었을 때 호출되는 메서드입니다.

```
    private void clickPlus()
    {
        this.op = CalcOp.Plus;
        StoredValue = DisplayValue;
        DisplayValue = 0;

        writeStatus();
    }
```

현재 DisplayValue를 StoredValue에 저장하고, DisplayValue를 초기화한 후에 현재 연산자를 Plus로 설정합니다.

---
3. clickMinus() 메서드 추가하기

뺄셈 버튼이 클릭되었을 때 호출되는 메서드입니다. 

```
    private void clickMinus()
    {
        this.op = CalcOp.Minus;
        StoredValue = DisplayValue;
        DisplayValue = 0;

        writeStatus();
    }
```

Plus와 비슷하지만, 현재 연산자를 Minus로 설정합니다.

---
4. clickResult() 메서드 추가하기

결과(=) 버튼이 클릭되었을 때 호출되는 메서드입니다.

```
    private void clickResult()
    {
        if (op == CalcOp.Plus)
        {
            DisplayValue += StoredValue;
        }
        else if (op == CalcOp.Minus)
        {
            DisplayValue = StoredValue - DisplayValue;
        }

        StoredValue = 0;
        op = CalcOp.None;

        writeStatus();
    }
```

현재 선택된 연산자에 따라 DisplayValue와 StoredValue를 이용하여 계산을 수행하고, 결과를 DisplayValue에 저장합니다.

---
5. clickClear() 매서드 추가하기

초기화(C) 버튼이 클릭되었을 때 호출되는 메서드입니다. 

```
    private void clickClear()
    {
        StoredValue = 0;
        op = CalcOp.None;
        DisplayValue = 0;

        writeStatus();
    }
```

DisplayValue, StoredValue, 그리고 연산자를 초기화하여 계산기를 초기 상태로 되돌립니다.

---
6. writeStatus() 매서드 추가하기
   
현재 계산기의 상태를 콘솔에 출력하는 메서드입니다. 

```
    private void writeStatus()
    {
        Console.WriteLine($"DisplayValue[{this.DisplayValue}] StoredValue[{this.StoredValue}] op[{this.op}]");
    }
```

현재 DisplayValue, StoredValue, 그리고 선택된 연산자를 출력합니다. 

<br/>

### 🎉계산기가 완성되었습니다!!🎉

<br/>

## 3. 다른 방법으로 계산기 만들기 실습하기

이번엔 컴포넌트 기반 아키텍처를 활용하여 여러 개의 컴포넌트를 조합하여 하나의 계산기를 구성하는 방식으로 계산기 만들기 실습을 해보겠습니다.

이 방식으로 계산기를 만들게 되면 각 컴포넌트는 독립적으로 개발되며, 필요에 따라 재사용할 수 있습니다. 또한 특정 기능에 문제가 발생한 경우 해당 컴포넌트만 수정하면 되며, 다른 컴포넌트에는 영향을 주지 않습니다.
<br/><br/>
### 1. 시작하기

`Calc` 폴더에 5 가지 파일을 추가해줍니다.
- `CalcButton.razor` : 계산기에서 숫자나 연산자를 표시하는 버튼 컴포넌트
- `CalcButton.razor.css` : 버튼 컴포넌트의 스타일을 정의하는 CSS 파일
- `CalcLable.razor` : 계산기의 상태와 계산 결과를 표시하는 라벨 컴포넌트
- `CalcLable.razor.css` :  라벨 컴포넌트의 스타일을 정의하는 CSS 파일
- `CalcState.cs` : 사용자의 입력을 처리하고 계산을 수행하는 로직이 담긴 계산기의 상태를 관리하는 클래스
<br/><br/>
### 2. CalcButton.razor 파일 내용 구성하기

계산기에 숫자나 연산자를 나타내는 버튼을 표현하기 위한 컴포넌트를 만들어보겠습니다.

**save-points/session2/BlazorCalc/Components/Calc/CalcButton.razor** 위치로 이동합니다.

```
@inject CalcState calcState;

<button class="calc" @onclick="ClickButton">@DataValue</button>

@code {

    [Parameter]
    public string DataValue { get; set; } = "0";

    private void ClickButton()
    {
        calcState.clickButton(DataValue);
    }
}
```

- `@inject` : CalcState 서비스를 calcState라는 이름의 필드로 주입하는 기능을 합니다.
  
  -  calcState 필드를 통해 CalcState 클래스의 메서드나 속성을 호출합니다.

- `<button class="calc" @onclick="ClickButton">@DataValue</button>` : 클릭 이벤트가 발생하면 `ClickButton` 메서드가 호출됩니다.
  - 버튼의 텍스트는 `DataValue`로부터 가져옵니다.
<br/><br/>
### 3. 버튼 스타일 적용하기

**save-points/session2/BlazorCalc/Components/Calc/CalcButton.razor.css** 위치로 이동합니다.

```
.calc {
    height: 80px;
    width: 80px;
    background-color: lightskyblue;
    font-size: 24px;
}
```

`CalcButton.razor` 파일의 스타일을 지정합니다.
<br/><br/>
### 4. CalcLable.razor 파일 내용 구성하기

계산기의 상태나 계산 결과를 표시하기 위한 컴포넌트를 만들어보겠습니다.

**save-points/session2/BlazorCalc/Components/Calc/CalcLable.razor** 위치로 이동합니다.

```
@inject CalcState calcState;

<label class="calc">@calcState.DisplayValue</label>

@code
{
    protected override void OnInitialized()
    {
        base.OnInitialized();
        calcState.Notify += () => InvokeAsync(StateHasChanged); 
    }
}
```

- `<label class="calc">@calcState.DisplayValue</label>` : @calcState.DisplayValue를 통해 CalcState 서비스의 DisplayValue 속성을 가져와서 버튼 입력값이나 계산 결과를 라벨의 내용으로 사용합니다.

- `@code {...}`: OnInitialized 메서드를 재정의하여 컴포넌트가 초기화될 때 calcState의 Notify 이벤트에 이벤트 핸들러를 추가하고 있습니다.

- `calcState.Notify += ...` : Notify 이벤트에 새로운 이벤트 핸들러를 추가합니다. 이렇게 하면 Notify 이벤트가 발생할 때마다 추가된 이벤트 핸들러가 호출됩니다.

  - `() => InvokeAsync(StateHasChanged)` : 이 람다식은 InvokeAsync(StateHasChanged)를 호출합니다. 상태가 변경될 때마다 StateHasChanged 메서드를 호출하여 UI를 업데이트하도록 요청합니다.
    
    - StateHasChanged 메서드는 Blazor에게 UI를 업데이트하도록 요청하는 역할을 합니다.
<br/><br/>
### 5. 계산 결과 표시하는 라벨 스타일 적용하기
### 6. CalcState.cs 파일 내용 구성하기
### 7. CalcComponent.razor 에서 컴포넌트 조합하여 계산기 완성하기

<br/>

### 🎉계산기가 완성되었습니다!!🎉

<br/>
