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

---

save-points/session2/BlazorCalc_session2/Components/Pages/CalcPage.razor 위치로 이동합니다.
```
@page "/calc-page"
@rendermode InteractiveServer
```
@page "/calc-page"
Razor 페이지의 경로를 지정합니다.
@rendermode InteractiveServer
사용자와 상호작용하는 동안 서버에 요청을 보내어 렌더링 합니다.

---

### 3. 스타일 적용하기


## 3. 다른 방법으로 계산기 만들기 실습하기


