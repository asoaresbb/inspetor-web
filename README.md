# Inspetor Web

An ASP.NET MVC web app that uses Selenium WebDriver to inspect web pages. Enter a URL and choose what to see.

## Features

- **Screenshot** of the page
- **Reading mode**: shows only the main content of the page, taken from the first `<article>`, `<main>` or `#content` element found
- **Heading structure**: the hierarchy of `h1`, `h2`, `h3`...
- **Links**: every link found on the page, as a list

The app runs Selenium headless, without opening a browser window. The Chrome driver is downloaded automatically by `WebDriverManager`.

## Architecture

The project follows the MVC pattern, with an adapter layer between the domain and Selenium:

| Layer | Files | What it does |
| --- | --- | --- |
| Controller | `Controllers/HomeController.cs` | Receives the URL and the chosen action, and handles errors |
| Model | `Models/` | One model per feature, holding the result to display |
| View | `Views/Home/` | Razor pages, with Bootstrap |
| Adapters | `Adapters/` | One interface per feature (`IScreenshotService`, `IReadingModeService`, `IHeadingService`, `ILinksService`) and its Selenium implementation |

The models depend only on the interfaces, so Selenium can be swapped out, for example for a mock in tests, without touching the rest.

## Running

Requires the [.NET SDK 9.0 or later](https://dotnet.microsoft.com/download) and Google Chrome.

```sh
dotnet run          # starts on http://localhost:5206
dotnet watch run    # same, reloading when the code changes
```

The acceptance tests, written with Selenium and the Page Object pattern, need the app to be running. In another terminal:

```sh
dotnet test
```

The tests open a visible Chrome window for each test.

## Context

Course project for Software Development Lab (Laboratório de Desenvolvimento de Software), Universidade Aberta, 2025. Built in a pair with a classmate.
