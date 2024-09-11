# Sobre o CashFlow

Esta **API**, desenvolvida utilizando .NET 8, adota os princípios do **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir que os usuários registrem suas despesas, detalhando informações como título, data e hora, descrição, valor e tipo de pagamento, com os dados sendo armazenados de forma segura em um banco de dados **MySQL**.

## Arquitetura

A arquitetura da API baseia-se em **REST**, utilizando **métodos HTTP padrão** para uma comunicação eficiente e simplificada. Além disso, é complementada por uma documentação **Swagger**, que proporciona uma interface gráfica interativa para que os desenvolvedores possam explorar e testar os endpoints de maneira fácil.

## Pacotes NuGet utilizados

- #### `AutoMapper`

  É o responsável pelo mapeamento entre os objetos de domínio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual.

- #### `FluentAssertions`

  É utilizado nos testes de unidade para tornar as verificações mais legíveis, ajudando a escrever testes claros e compreensíveis. Para as validações.

- #### `FluentValidation`

  É usado para implementar regras de validação de forma simples e intuitiva nas classes de requisições, mantendo o código limpo e fácil de mander.

- #### `EntityFramework`
  atua como um ORM (Object-Relational Mapper) que simplifica as interações com o banco de dados, permitindo o uso de objetos .NET para manipular dados diretamente, sem a necessidade de lidar com consultas SQL.

## Features

- **Domain-Driven Design (DDD)**: Estrutura modular que facilita o entendimento e a manutenção do domínio da aplicação
- **Testes de unidade**: Testes abrangentes com FluentAssertions para garantir a funcionalidade e a qualidade.
- **Geração de Relatórios**: Capacidade de exportar relatórios detalhados para **PDF e Excel**, oferecendo uma análise visual e eficaz das despezas.
- **RESTful API com Documentação Swagger**: Interface documentada que facilita a integração e o teste por parte dos desenvolvedores.

## Construido com

![badge-dot-net]
![badge-mysql]
![badge-swagger]
![bedge-rider]

## Getting Started

Para obter uma cópia local funcionando, siga estes passos simples:

### Requisitos

- [Visual Studio][visual-studio] versão 2022+ ou [Rider 2024][rider]
- Windows 10+ ou Linux/MacOS com [.NET SDK][dot-net-sdk] instalado
- MySql Server

### Instalação

1. Clone o repositório:

   ```sh
   git@github.com:brunoknop/CashFlow.git
   ```

2. Preencha as informações no arquivo `appsettings.Development.json`
3. Execute a API e aproveite o seu teste :)

<!-- Links -->

[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[rider]: https://www.jetbrains.com/rider/
[visual-studio]: https://visualstudio.microsoft.com/pt-br/vs/

<!-- Badges -->

[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[bedge-rider]: https://img.shields.io/badge/Rider-854cff?style=for-the-badge&logo=rider&logoColor=white
