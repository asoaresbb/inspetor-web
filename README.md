# Inspetor Web

Aplicação web em ASP.NET MVC que usa o Selenium WebDriver para inspecionar páginas. Introduz-se um URL e escolhe-se o que ver.

## Funcionalidades

- **Captura de ecrã** da página
- **Modo de leitura**, sem pop-ups de cookies nem publicidade
- **Estrutura de títulos**: a hierarquia dos `h1`, `h2`, `h3`...
- **Ligações externas** encontradas na página, num gráfico

O Selenium corre em modo headless, sem abrir janela. O driver do Chrome é descarregado automaticamente pelo `WebDriverManager`.

## Arquitetura

O projeto segue o padrão MVC, com uma camada de adaptadores entre o domínio e o Selenium:

| Camada | Ficheiros | O que faz |
| --- | --- | --- |
| Controller | `Controllers/HomeController.cs` | Recebe o URL e a ação escolhida e trata os erros |
| Model | `Models/` | Um modelo por funcionalidade, com o resultado a mostrar |
| View | `Views/Home/` | Páginas Razor, com Bootstrap |
| Adaptadores | `Adapters/` | Uma interface por funcionalidade (`IScreenshotService`, `IReadingModeService`, `IHeadingService`, `ILinksService`) e a sua implementação com o Selenium |

Os modelos dependem só das interfaces, por isso o Selenium pode ser trocado, por exemplo por um mock nos testes, sem mexer no resto.

## Como correr

Requer o [.NET SDK 9.0](https://dotnet.microsoft.com/download) e o Google Chrome.

```sh
dotnet run          # arranca em http://localhost:5206
dotnet watch run    # o mesmo, a recarregar quando o código muda
```

Os testes de aceitação, com Selenium e o padrão Page Object, precisam da aplicação a correr. Noutro terminal:

```sh
dotnet test
```

## Contexto

Projeto da unidade curricular de Laboratório de Desenvolvimento de Software, Universidade Aberta, 2025. Feito em par com uma colega.
