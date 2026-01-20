# Sistema de Estacionamento - Desafio DIO & TIVIT (Improved Version)

![Status](https://img.shields.io/badge/Status-Evolving-success)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![Blazor](https://img.shields.io/badge/Blazor-WASM-blue)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED)

## 📖 O Contexto

Este projeto foi proposto originalmente como o desafio final do módulo de **Fundamentos de .NET** do Bootcamp **TIVIT** da [DIO](https://www.dio.me/).

### 🎯 O Desafio Original (Escopo)
A proposta inicial era construir uma **Aplicação Console** para gerenciar veículos, seguindo estas especificações:
* Criar uma classe `Estacionamento`.
* Usar uma `List<string>` para armazenar apenas as placas dos veículos em memória.
* Implementar um menu interativo no console (`Console.ReadLine`) com as opções: Cadastrar, Remover (com cálculo de valor) e Listar.

---

## 🚀 A Evolução do Projeto

Após concluir a implementação básica em console, que se encontra disponível em uma outra branch deste repositório, decidi utilizar este cenário para aplicar outros conhecimentos adquiridos ao longo do bootcamp e em estudos paralelos (especialmente **Blazor** e **Arquitetura de Microsserviços**).

Transformei a aplicação console monolítica em uma solução **Fullstack Containerizada**.


## 📹 Demo da aplicação:


https://github.com/user-attachments/assets/88e3dba4-1303-44d3-a173-681ccfb9c1ad


## 📖 Documentação da API
   * Página estática da [documentação](https://gbad8.github.io/trilha-net-fundamentos-desafio/).
   * Para testar os endpoints, execute a aplicação utilizando o passo a passo da sessão abaixo "Como Executar"


### Comparativo: O que mudou?

| Característica | Proposta Original (Console) | Minha Implementação (Fullstack) |
| :--- | :--- | :--- |
| **Interface** | Menu de Texto (Console) | **Blazor WebAssembly** |
| **Lógica** | Classe local `Estacionamento.cs` | **API RESTful** (.NET 10 Controller) |
| **Persistência** | `List<string>` (Memória Volátil) | **Azure SQL Edge [¹]** + **Entity Framework Core** |
| **Estrutura** | Monolito Simples | **Docker Compose** (Multi-container) |
| **Modelagem** | Apenas Placa (string) | Entidade `Veiculo` (ID, Placa, Horas, Preço, etc.) |

[¹]: Utilizei no início o SQL Server, no entanto, sua imagem pede muita memória RAM e exige configurações adicionais para aqueles que testarão a aplicação por meio do Docker Desktop no Windows. Portanto, optei por transicionar para o Azure SQL Edge, mesmo sabendo que o serviço foi descontinuado pela Microsoft em setembro de 2025. Em produção, ele seria trocado pelo Azure SQL Database.

## 🛠 Arquitetura da Solução

O projeto agora opera com três serviços principais orquestrados:

1.  **Backend (API):**
    * Feito com base na imagem oficial do [.NET SDK 10.0](https://hub.docker.com/r/microsoft/dotnet-sdk) da Microsoft
    * Substitui a classe `Estacionamento` original por uma classe `Controller`.
    * Implementa o cálculo de cobrança e regras de negócio.
    * Conecta-se ao banco de dados via Entity Framework.
3.  **Frontend (Client):**
    * Feito com base na imagem oficial do [.NET SDK 9.0](https://hub.docker.com/r/microsoft/dotnet-sdk) da Microsoft.
    * Aplicação Blazor WebAssembly que consome a API.
    * Utiliza a biblioteca [MudBlazor](https://mudblazor.com).
    * Permite a visualização/interação em tempo real dos veículos estacionados.
5. **Banco de dados**:
    * Feito com base na imagem oficial do [Azure SQL Edge](https://hub.docker.com/r/microsoft/azure-sql-edge) da Microsoft.
    * Integração feita com o Entity Framework.
    * Substitui o uso de memória volátil, fazendo persistir os dados dos veículos.



## ⚙️ Como Executar

A infraestrutura foi desenhada para ser executada via Docker, eliminando a necessidade de configurar o banco de dados manualmente na sua máquina.

### Pré-requisitos
* [Docker](https://www.docker.com/products/docker-desktop) instalado.

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/gbad8/trilha-net-fundamentos-desafio.git
   ```

2. **Suba o ambiente:** Na pasta src/ do projeto (onde está o arquivo compose.yml), execute:
   ```
   docker compose up
   ```
3. **Acesse as interfaces:**
   * **Frontend (Aplicação):** [http://localhost:5001](http://localhost:5001)
   * **Backend (Scalar/Docs):** [http://localhost:8000/scalar](http://localhost:8000/scalar/)
  
## 🗺️ Roadmap de Evolução

Este projeto foi desenvolvido como um desafio técnico inicial. Tenho plena consciência de que a aplicação ainda carece de implementações necessárias para atingir um nível de produção.

No momento, meu foco principal está voltado ao desenvolvimento dos projetos subsequentes do **Bootcamp DIO/TIVIT**, mas este repositório receberá atualizações contínuas assim que os próximos projetos do bootcamp forem concluídos.

### 🏗️ Próximas Implementações:

* **Validações:** Implementar lógica para validação de formatos de placa (Padrão Mercosul e Antigo).
* **Testes Unitários 🚧 (Em andamento):** Cobertura de testes com xUnit para garantir a confiabilidade das regras de negócio.
* **Tratamento de Exceções:** Implementação de um middleware global para tratamento de erros e logs.
* **Deploy na Nuvem:** Configuração de CI/CD e hospedagem da API/Web App no Azure ou AWS (estou estudando para o AZ-900, então o deploy servirá para praticar os conceitos aprendidos em minha preparação). 
