# 세션 02: 계산기 만들기

이 세션에서는 Blazor로 계산기 만들기를 진행합니다.

## 1. Blazor 개발환경 구축하기

블레이저를 시작하려면 [blazor.net](https://blazor.net)의 안내를 따라하세요.

* [git 설치하기](https://git-scm.com/downloads)
* [Visual Studio Code 설치하기](https://code.visualstudio.com/download)
* [C# Dev Kit 익스텐션 설치하기](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
* [.NET SDK 8.0 설치하기](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## 2. CalcPage.razor 파일에서 실습하기
### 1. 시작하기

원하는 경로에 레포지토리를 복사하고
```
$ git clone https://github.com/blazorstudy/blazor-workshop-calc.git
```

"save-points" 폴더에 각 세션별로 준비가 되어 있습니다. [session2](https://github.com/blazorstudy/blazor-workshop-calc/tree/main/save-points/session2)를 찾아보세요


### 2. 페이지 라우팅 경로, 렌더링 모드 설정하기

save-points/session2/BlazorCalc_session2/Components/Pages/CalcPage.razor 위치로 이동합니다.

```
@page "/calc-page"
@rendermode InteractiveServer
```
`CalcPage.razor` 파일 상단에 위 내용을 추가해줍니다.


- @page "/calc-page"

  - Razor 페이지의 경로를 지정합니다.

- @rendermode InteractiveServer

  - 사용자와 상호작용하는 동안 서버에 요청을 보내어 렌더링 합니다.


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
- `<label class="display-calc">@this.DisplayValue</label>`: 계산기의 출력 화면을 나타내는 레이블
  
  - 위에 추가했던 `display-calc`라는 CSS 클래스가 여기 적용됩니다.
 
- `<button class="button-number" @onclick="() => clickButton(N)">N</button>` : 0부터 9까지의 숫자를 나타내며, 사용자가 숫자를 입력할 때 클릭하는 버튼

  - 마찬가지로 각 버튼에는 위에 추가했던 button-number라는 CSS 클래스가 적용됩니다.
    
  - `@onclick`: Blazor에서 사용되는 이벤트 처리기 지시문으로, <button>이 클릭되었을 때 실행될 이벤트 핸들러를 지정합니다.
    
  - `() => clickButton(N)`: 이 람다식은 메서드를 생성하여 버튼이 클릭되었을 때 clickButton(N) 메서드를 호출하도록 지정합니다. 여기서 N은 버튼이 클릭될 때 clickButton 메서드에 전달되는 인자값입니다.
    
>> 위와 같이 함수를 정의하고 해당 함수를 참조하는 대신, 직접 람다식으로 함수를 정의하여 사용할 수 있습니다. 코드가 더 간결해지고, 이벤트 핸들러의 목적이 명확해집니다.

- 연산자 버튼: 덧셈(+), 뺄셈(-), 그리고 등호(=) 버튼은 계산기의 기본 연산을 나타내며, 숫자 입력과 마찬가지로 클릭 이벤트에 따라 해당 기능을 수행하는 C# 메서드가 정의되어 있습니다.

- `<button class="button-number" @onclick="clickClear">C</button>` : 

- `display:flex`를 사용하여 버튼들을 수평으로 나란히 배치하고 있습니다. 이를 통해 사용자가 계산기를 사용할 때 편리하게 숫자 및 연산자를 입력할 수 있도록 UI가 구성되어 있습니다.









## 3. 다른 방법으로 계산기 만들기 실습하기


