# Gerenciador de Indústria de Carnes

## 📦 Projeto Full Stack para Gerenciamento de Carnes, Compradores e Pedidos

Este é um sistema completo de gerenciamento de compras e pedidos para uma indústria de carnes, com **frontend em React (JavaScript)** e **backend em ASP.NET Core (.NET 8)**, utilizando **SQL Server** como banco de dados.

---

## ✅ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (Express, Developer ou LocalDB)
- Node.js (versão 16 ou superior)
- Visual Studio 2022 ou superior

---

## 🚀 Como rodar o projeto

### 1. Executar o Frontend

1. Abra o terminal e navegue até a pasta:  
   `frontend-gerenciador`
2. Instale as dependências:  
   npm install
3. Execute o projeto React:  
   npm run dev
4. O frontend estará acessível via:  
   http://localhost:5173

O frontend foi desenvolvido com:

- **React (JavaScript)**
- **Tailwind CSS** (para estilização responsiva)
- **Flowbite React** (componentes prontos e acessíveis)

Todos os formulários possuem **validações de campos obrigatórios e formatos** antes do envio dos dados.

⚠️ O backend precisa estar rodando para que as requisições funcionem corretamente.

---

### 2. Criar e Configurar o Banco de Dados SQL

1. Crie um banco de dados SQL local em sua máquina (por exemplo, `GerenciadorDb`).
2. Abra o arquivo:

   Gerenciador.API/appsettings.Development.json

3. Atualize a DefaultConnection com a string de conexão do seu SQL Server. Exemplo:

   "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GerenciadorDB;Trusted_Connection=True;MultipleActiveResultSets=true"

   Basta trocar o "Database=GerenciadorDB" para o nome do seu banco de dados local.

---

### 3. Executar o Backend

1. Abra a solução no Visual Studio:  
   GerenciadorBackend.sln
2. Ao executar o projeto (Gerenciador.API), ele realizará automaticamente:

   - Criação das tabelas no banco de dados.
   - Inserção de dados iniciais (carnes, compradores, etc.).
   - Execução dos testes unitários que validam toda a camada de serviços (CarneService, CompradorService, PedidoService), garantindo o funcionamento antes mesmo de compilar a API.

3. Rode a aplicação com o perfil da API selecionado.

---

## 🧩 Estrutura do Projeto

### Backend

- **Gerenciador.API**
  API REST principal com endpoints de CRUD de Carnes, Compradores e Pedidos.
- **Gerenciador.Service**
  Contém as regras de negócio e serviços principais:
  - CarneService.cs
  - CompradorService.cs
  - PedidoService.cs
- **Gerenciador.Data**
  Acesso a dados com Entity Framework Core.
- **Gerenciador.Model**
  Entidades de domínio (Carne, Comprador, Pedido).
- **Gerenciador.Testes**
  Projetos de testes unitários:
  - CarneServiceTeste.cs
  - CompradorServiceTeste.cs
  - PedidoServiceTeste.cs
  - DataMocks para dados de exemplo.

---

## 🧩 Funcionalidades

- CRUD completo de Carnes, Compradores e Pedidos
- Conversão automática de moedas (Real, Dólar, Euro) via AwesomeAPI
- Validação de dados (campos obrigatórios, preços positivos)
- Modal de confirmação de exclusão
- Filtro de pedidos por comprador e data
- Feedback visual de sucesso e erro
- Interfaces responsivas com Tailwind CSS e Flowbite React

---

## ⚙️ Tecnologias Utilizadas

- ASP.NET Core (.NET 8)
- Entity Framework Core
- React (JavaScript)
- Tailwind CSS
- Flowbite React
- SQL Server
- Axios
- AwesomeAPI (cotações de moedas)

---

## 🎯 Observações

- Certifique-se de que a API está rodando antes de iniciar o frontend.
- A porta padrão da API é definida no launchSettings.json.
- Todos os testes unitários são executados automaticamente ao iniciar o backend.
