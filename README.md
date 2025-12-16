# Sistema de Estacionamento - Desafio DIO & TIVIT (Improved Version)

![Status](https://img.shields.io/badge/Status-Evoluído-success)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![Blazor](https://img.shields.io/badge/Blazor-Wasm-blue)
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

### Comparativo: O que mudou?

| Característica | Proposta Original (Console) | Minha Implementação (Fullstack) |
| :--- | :--- | :--- |
| **Interface** | Menu de Texto (Console) | **Blazor WebAssembly** |
| **Lógica** | Classe local `Estacionamento.cs` | **API RESTful** (.NET 9 Controller) |
| **Persistência** | `List<string>` (Memória Volátil) | **SQL Server** + **Entity Framework Core** |
| **Estrutura** | Monolito Simples | **Docker Compose** (Multi-container) |
| **Modelagem** | Apenas Placa (string) | Entidade `Veiculo` (ID, Placa, Horas, Timestamp) |

## 🛠 Arquitetura da Solução

O projeto agora opera com três serviços principais orquestrados:

1.  **Backend (API):**
    * Feito com base na imagem oficial do .NET SDK 9.0 da Microsoft
    * Substitui a classe `Estacionamento` original por um `Controller` de API.
    * Implementa o cálculo de cobrança e regras de negócio.
    * Conecta-se ao SQL Server via Entity Framework.
3.  **Frontend (Client):**
    * Feito com base na imagem oficial do .NET SDK 9.0 da Microsoft.
    * Aplicação Blazor WebAssembly que consome a API.
    * Permite a visualização em tempo real dos veículos estacionados.
5. **Banco de dados**:
    * Feito com base na imagem oficial do SQL Server da Microsoft.
    * Integração feita com o Entity Framework.
    * Substitui o uso de memória local, fazendo persistir os dados dos veículos.

## ⚙️ Como Executar

A infraestrutura foi desenhada para ser executada via Docker, eliminando a necessidade de configurar o banco de dados manualmente na sua máquina.

### Pré-requisitos
* [Docker](https://www.docker.com/products/docker-desktop) instalado.

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone [https://github.com/SEU-USUARIO/NOME-DO-REPO.git](https://github.com/SEU-USUARIO/NOME-DO-REPO.git)
