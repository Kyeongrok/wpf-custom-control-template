# WpfCustomControlTemplate

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

`dotnet new` 명령 한 줄로 **WPF CustomControl 기반 프로젝트**를 즉시 생성하는 템플릿입니다.

Support / Main / Forms 3단계 레이어 구조와 커스텀 크롬 윈도우가 처음부터 포함되어 있어 반복 작업 없이 바로 기능 개발에 집중할 수 있습니다.

## 이 템플릿이 필요한 이유

WPF CustomControl 프로젝트를 새로 시작할 때마다 아래 작업을 반복하게 됩니다.

- 솔루션과 프로젝트 4개 생성 및 참조 설정
- `ThemeInfo` 어트리뷰트, `Generic.xaml` 작성
- `DefaultStyleKeyProperty.OverrideMetadata` 등록
- `WindowChrome` 기반 커스텀 크롬 윈도우 구현
- `OnApplyTemplate` + `PART_` 버튼 패턴 구현
- DI 컨테이너 설정 (`Microsoft.Extensions.Hosting`)

이 템플릿은 위 내용을 모두 포함한 채 생성됩니다. 빌드 후 바로 실행되는 상태로 시작합니다.

## 설치

```bash
dotnet new install WpfCustomControlTemplate
```

> 로컬 `.nupkg`로 설치하는 경우:
> ```bash
> dotnet new install ./bin/Release/WpfCustomControlTemplate.1.0.0.nupkg
> ```

## 사용법

```bash
dotnet new wpf-cc -n MyProjectName
```

`MyProjectName` 자리에 원하는 이름을 넣으면 프로젝트 이름, 네임스페이스, 파일명이 모두 자동으로 치환됩니다.

### 예시

```bash
dotnet new wpf-cc -n FactoryMonitor
cd FactoryMonitor
dotnet build
dotnet run --project FactoryMonitor
```

## 생성되는 프로젝트 구조

```
MyProjectName/
├── MyProjectName.sln
│
├── MyProjectName/                         ← 진입점 (WinExe)
│   ├── MyProjectName.csproj
│   ├── App.cs                             ← IHost DI 설정
│   └── Starter.cs                         ← [STAThread] Main
│
├── MyProjectName.Support/                 ← 공통 커스텀 컨트롤 라이브러리
│   ├── MyProjectName.Support.csproj
│   ├── AssemblyInfo.cs                    ← ThemeInfo 어트리뷰트
│   ├── UI/Units/
│   │   └── MyProjectNameWindow.cs         ← 기본 WindowChrome 창
│   └── Themes/
│       ├── Generic.xaml
│       └── Units/MyProjectNameWindow.xaml
│
├── MyProjectName.Main/                    ← 도메인별 커스텀 컨트롤 추가
│   ├── MyProjectName.Main.csproj
│   ├── AssemblyInfo.cs
│   └── Themes/Generic.xaml
│
└── MyProjectName.Forms/                   ← 뷰 / 윈도우
    ├── MyProjectName.Forms.csproj
    ├── AssemblyInfo.cs
    ├── AppServices.cs                     ← DI 서비스 접근 헬퍼
    ├── UI/Views/
    │   └── MainWindow.cs                  ← 닫기/최대화/최소화 + 버튼
    └── Themes/
        ├── Generic.xaml
        └── UI/Views/MainWindow.xaml
```

## 초기 실행 화면

생성 직후 실행하면 아래 구성의 윈도우가 표시됩니다.

```
┌─────────────────────────────────────────── ─ □ ✕ ┐
│ MyProjectName                                     │
│                                                   │
│                  [ Click Me ]                     │
│                                                   │
└───────────────────────────────────────────────────┘
```

- **타이틀바** — 드래그로 이동, `WindowChrome`으로 네이티브 크롬 제거
- **최소화 / 최대화 / 닫기** — `PART_MinimizeButton`, `PART_MaximizeButton`, `PART_CloseButton`
- **Click Me 버튼** — `MessageBox.Show` 호출 예시 (`PART_CenterButton`)
- **리사이즈** — `ResizeBorderThickness="5"` 로 가장자리 드래그 리사이즈 가능

## CustomControl 패턴

이 템플릿은 WPF CustomControl의 표준 패턴을 따릅니다.

### 1. 컨트롤 등록 (`AssemblyInfo.cs`)

```csharp
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
)]
```

### 2. 스타일 키 등록 (C# 클래스)

```csharp
static MyProjectNameWindow()
{
    DefaultStyleKeyProperty.OverrideMetadata(typeof(MyProjectNameWindow),
        new FrameworkPropertyMetadata(typeof(MyProjectNameWindow)));
}
```

### 3. 스타일 정의 (`Themes/Units/MyProjectNameWindow.xaml`)

```xml
<Style TargetType="{x:Type units:MyProjectNameWindow}">
    <Setter Property="WindowChrome.WindowChrome">
        <Setter.Value>
            <WindowChrome CaptionHeight="40" ResizeBorderThickness="5" .../>
        </Setter.Value>
    </Setter>
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate ...>
                ...
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

### 4. Generic.xaml에 등록

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="/MyProjectName.Support;component/Themes/Units/MyProjectNameWindow.xaml"/>
</ResourceDictionary.MergedDictionaries>
```

### 5. `PART_` 패턴으로 자식 요소 연결 (`OnApplyTemplate`)

```csharp
public override void OnApplyTemplate()
{
    base.OnApplyTemplate();

    var closeButton = GetTemplateChild("PART_CloseButton") as Button;
    if (closeButton != null)
        closeButton.Click += (s, e) => Close();
}
```

## 레이어 구조 설명

| 프로젝트 | 역할 | 참조 |
|---|---|---|
| `Support` | 앱 전체에서 쓰는 기반 컨트롤 (기본 창, 공통 유닛) | 없음 |
| `Main` | 도메인 특화 커스텀 컨트롤 추가 | Support |
| `Forms` | 화면 구성 (Window, View, ViewModel) | Main, Support |
| `(루트)` | 진입점, DI 설정 | Forms |

새 커스텀 컨트롤을 추가할 때는 `Support` 또는 `Main` 프로젝트에 C# 클래스와 XAML 파일을 추가하고, 해당 프로젝트의 `Themes/Generic.xaml`에 경로를 등록합니다.

## 요구사항

- .NET 8.0 이상
- Windows (WPF는 Windows 전용)

```bash
dotnet --version
```

## 템플릿 제거

```bash
dotnet new uninstall WpfCustomControlTemplate
```

## 라이선스

MIT License
