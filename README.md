# Web Page Inspector

An ASP.NET MVC web app that uses Selenium WebDriver to inspect web pages. Enter a URL and choose what to see.

## Features

- **Screenshot** of the page
- **Reading mode**: shows the text of the page's main content (headings, paragraphs, lists, quotes) in a clean reading layout, without links, images or scripts. The main content is every `<article>` on the page, separated by a line, or else the first `<main>`, or else `#content`. Only the text is shown, so nothing from the inspected page runs in the app
- **Heading structure**: the hierarchy of `h1`, `h2`, `h3`..., each level indented further. Headings that aren't shown on the page (for example, for screen readers or inside a closed menu) are marked as hidden, and headings with no text are marked as empty
- **External links**: the links on the page that point to other sites, as a list

The app runs Selenium headless, without opening a browser window. The Chrome driver is downloaded automatically by `WebDriverManager`.

Before reading a page, the app waits for it to finish rendering: after the page loads, it waits until the page has gone 1 second without changes, for up to 10 seconds. This covers single-page apps that show a loading screen and build the content with JavaScript. A page that stays still for more than 1 second before showing its content is read too early.

While a feature is working, a loading message replaces the buttons. When a page has no headings or no external links, the app says so. When a feature fails, the app shows a short message and writes the full error to its log, in the terminal running `dotnet run`.

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

All four features are covered by tests. The tests use fixed pages in `wwwroot/test-pages/`, so they don't need an internet connection or depend on third-party sites. The tests open a visible Chrome window for each test.

## Context

Course project for Software Development Lab (Laboratório de Desenvolvimento de Software), Universidade Aberta, 2025. Built in a pair with a classmate.
