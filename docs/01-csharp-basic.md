# 세션 01: C# 기초

이 세션에서는 워크샵 진행에 필요한 개발 환경 설정을 진행하고 C# 기초 지식을 습득합니다.   
오늘 워크샵에서 사용하는 간단한 HTML도 살짝 맛봅니다.

## 학습 목표
- C# 개발 환경을 구축한다
- 오늘 사용할 C# 기초를 습득한다
- 오늘 사용할 HTML을 이해한다

## 개발 환경 설치
- Windows OS
  <details>
    <summary>Visual Studio 2022 커뮤니티 버전 설치</summary>

    - [설치 가이드](https://learn.microsoft.com/ko-kr/visualstudio/install/install-visual-studio?view=vs-2022&WT.mc_id=MVP_307888)를 따라 설치 진행
    - Visual Studio 2022 커뮤니티 버전 다운로드
    - ASP.NET 및 웹개발 워크로드 선택
  </details>

- macOS
  <details>
    <summary>Visual Studio Code 및 SDK 설치</summary>

    - [설치 가이드](https://code.visualstudio.com/docs/csharp/get-started?WT.mc_id=MVP_307888)를 따라 설치 진행
    - Visual Studio Code 설치
    - C# Dev Kit 확장 설치
    - [.NET SDK](https://dotnet.microsoft.com/ko-kr/download/dotnet/8.0) 설치

  </details>

- DevContainer
  <details>
    <summary> 개발자 컨테이너로 시작 </summary>

    - [Docker Desktop 설치](https://www.docker.com/products/docker-desktop/)
    - [Visual Stduio Code 설치](https://code.visualstudio.com/)
    - [Dev Containers 확장 설치](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers)
    - [Developing inside a Container](https://code.visualstudio.com/docs/devcontainers/containers)

  </details>

  <span style="color:orange">Visual Studio 설치를 추천</span>

## 자료형
### 정수 형식
| C# 형식/키워드 |	범위	| Size	| .NET 형식 |
|:--:|:--:|:--:|:--:|
| sbyte | -128 ~ 127 | 부호 있는 8비트 정수 | System.SByte |
| byte | 0 ~ 255 | 부호 없는 8비트 정수 | System.Byte |
| short | -32,768 ~ 32,767 | 부호 있는 16비트 정수 | System.Int16 |
| ushort | 0 ~ 65,535 | 부호 없는 16비트 정수 | System.UInt16 |
| int | -2,147,483,648 ~ 2,147,483,647 | 부호 있는 32비트 정수 | System.Int32 |
| uint | 0 ~ 4,294,967,295 | 부호 없는 32비트 정수 | System.UInt32 |
| long | -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807 | 부호 있는 64비트 정수 | System.Int64 |
| ulong | 0 ~ 18,446,744,073,709,551,615 | 부호 없는 64비트 정수 | System.UInt64 |
| nint | 플랫폼에 따라 다름(런타임에 계산됨) | 부호 있는 32비트 또는 64비트 정수 | System.IntPtr |
| nuint | 플랫폼에 따라 다름(런타임에 계산됨) | 부호 없는 32비트 또는 64비트 정수 | System.UIntPtr |

- 16진수 : 0x 접두사 사용
- 2진수 : 0b 접두사 사용
- 문자열로 표현
   <details>
      <summary>자릿수에 표현</summary>
      
      double value = 123456.789d;
      Console.WriteLine(value.ToString());
      Console.WriteLine(value.ToString("#,#"));
      Console.WriteLine(value.ToString("#,#.##"));

      // output
      123456.789    : 그대로 표시
      123,457       : 자릿수에 콤마 추가, 정수로 표시(반올림)
      123,456.79    : 자릿수에 콤마 추가, 소수점2자리까지 표시(반올림)
   </details>

### 부동소수점 형식
| C# 형식/키워드 |	근사 범위	| 전체 자릿수 | Size	| .NET 형식 |
|:--:|:--:|:--:|:--:|:--:|
| float | ±1.5 x 10−45 ~ ±3.4 x 1038 | ~6-9개 자릿수 | 4바이트 | System.Single |
| double | ±5.0 × 10−324 ~ ±1.7 × 10308 | ~15-17개 자릿수 | 8바이트 | System.Double |
| decimal | ±1.0 x 10-28 ~ ±7.9228 x 1028 | 28-29개의 자릿수 | 16바이트 | System.Decimal |

- float : f 접미사 사용
- double : d 접미사 사용
- decimal : m 접미사 사용

### 기타 자료형
- bool : 부울 값(true 또는 false)
- char : 유니코드 UTF-16 문자
- enum
  ```
  enum ErrorCode
  {
      None = 0,
      Unknown = 1,
      ConnectionLost = 100,
      OutlierReading = 200
  }
  ```
- Nullable값 형식 : null값을 허용하는 자료형. 기본 자료형에 `?`를 붙여서 사용
  ```
  int? value = null;
  ```
  - 기본값 설정에 편리한 `??`연산자
    ```
    // n1이 nullable일 경우
    int n2 = n1 ?? 45;
    // if( n1 =! null )
    //    n2 = n1;
    // else 
    //    n2 = 45;
    ```
- object : 모든 형식
- string : 문자열
  ```
  string name = "My name is KIM.";
  ```
   <details>
      <summary>다양한 상수 표현</summary>
      
      """
      This is a multi-line
          string literal with the second line indented.
      """

      """""
      This raw string literal has four """", count them: """" four!
      embedded quote characters in a sequence. That's why it starts and ends
      with five double quotes.

      You could extend this example with as many embedded quotes as needed for your text.
      """""

      @"c:\Docs\Source\a.txt"  // rather than "c:\\Docs\\Source\\a.txt"

   </details>

- dynamic : 런타임이 자료형 확인

## Class
### Class 정의
- 흔히 객체라고 이야기되는 대표적인 형식
- 참조 형식, new로 생성, 개체를 할당하기 전에는 null
  ```
  SampleClass sample1 = new SampleClass();
  SampleClass sample2 = sample1;
  SampleClass sample3; // sample3 == null
  ```
- 정의
  ```
  //[access modifier] - [class] - [identifier]
  public class Customer
  {
     // Fields, properties, methods and events go here...
  }
  ```
  - 한정자 : public, protected, private, internal
  - fields/member value : 클래스에 저장하는 값
  - method/member function : 클래스에 구현하는 함수
  - properites : 비공개 field에 대한 접근이나 설정을 정의한 함수 주로 get/set method라고 불림

- 상속 : 클래스 이름 뒤에 선언, 다중상속 지원하지 않음
  ```
  public class Car {}
  public class MyCar : Car {}
  ```

### Class Extension
기존 형식을 상속 받거나 수정하여 다시 컴파일 하지 않아도 메서드를 추가하는 방법을 `확장 메서드`라고 합니다.   
라이브러리로 제공 받거나 한 class 형식에 추가적인 기능을 넣거나 Collections에 기능을 추가할 때 유용합니다.

- 예시
  ```
  var car = new Car() { Name = "내자동차" }; 
  car.printName();
  car.printOfficalName();

  public class Car
  {
      public string Name { get; set; }
      public void printName() { Console.WriteLine($"{this.Name}"); }
  }

  public static class CarExtensions
  {
      public static void printOfficalName(this Car thisCar) { Console.WriteLine($"Model : {thisCar.Name}"); }
  }
  ```
- static으로 선언, Extensions라는 클래스 이름으로 선언, 첫 번째 인수에 확장 클래스 표시
- [공식문서](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods?WT.mc_id=MVP_307888)

## Function
### Console.WriteLine
- 표준 출력에 간단히 표시하기에 편한 함수
- Visual Studio의 콘솔창, Visual Studio Code의 터미널 혹은 콘솔에 출력
- Blazor InteractiveWebAssembly의 경우 브라우저의 콘솔에 표시
```
Console.WriteLine("출력하고 싶은 문구"); 
```
- [공식문서](https://learn.microsoft.com/dotnet/api/system.console.writeline?view=net-7.0&WT.mc_id=MVP_307888)

### 람다 식 (lambda expression)
람다 식을 이용하여 익명 함수를 만들어 사용할 수 있습니다. `=>`연산자를 이용하여 정의합니다. 람다식을 이용하는 것을 선호하지 않을 수 있지만 작성된 코드를 읽을 수는 있어야 하겠습니다.

- 수식으로 만들어진 람다 식
  ```
  (input-parameters) => expression

  Func<int, int> plus1 = (x) => x + 1; ;
  Console.WriteLine(plus1(3));
  // output:
  // 4
  ```

- 블럭으로 만들어진 람다 문
  ```
  Action<string> greet = name =>
  {
      string greeting = $"Hello {name}!";
      Console.WriteLine(greeting);
  };
  greet("World");
  // output:
  // Hello World!
  ```
- 개인적으로 잘 사용하는 방식
  ```
  public class Car
  {
      public int Year { get; set; }
      public string Name { get; set ; }
      public string FullName => $"{Year}식 {Name}";
  }
  ```

- [공식문서](https://learn.microsoft.com/ko-kr/dotnet/csharp/language-reference/operators/lambda-expressions?WT.mc_id=MVP_307888)


# HTML & CSS
## div
HTML에서 section이나 구역을 나누기 위한 tag   
의미적으로 나누기 위해 사용하고 브라우저가 그리는 것은 없습니다.
```
<div>
  <!-- 여기에 HTML을 작성하세요. -->
</div>
```

- [w3school page](https://www.w3schools.com/tags/tag_div.asp)

## button
HTML에서 클릭할 수 있는 버튼을 위한 tag
오늘은 계산기의 버튼으로 사용합니다.
```
<button onclick="클릭시 실행함수>
  버튼에 표시될 글자
</button>
```

- [w3school page](https://www.w3schools.com/tags/tag_button.asp)

## style
HTML element에 스타일을 넣고 싶을 때 사용합니다.

- display:flex : 다양하게 사용할 수 있지만 오늘은 자식 요소를 가로로 배열하는 뜻으로 사용
- height : 요소의 높이를 정의
- width : 요소의 너비의 정의
- background-color : 요소의 배경색을 정의
- font-size : 요소의 글꼴 크기를 정의

## 참고자료
- [C# 배우기 컬렉션](https://aka.ms/lldnkr/csharp)
- [C# 언어 설명서](https://learn.microsoft.com/ko-kr/dotnet/csharp/?WT.mc_id=MVP_307888)
- [Getting Started with C# in VS Code](https://code.visualstudio.com/docs/csharp/get-started?WT.mc_id=MVP_307888)
- [Visual Studio 설명서](https://learn.microsoft.com/visualstudio/windows/?view=vs-2022&WT.mc_id=MVP_307888)