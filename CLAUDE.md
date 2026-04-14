# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Comportamento do Claude Code

- Respostas sucintas: reportar apenas o que foi alterado, testado ou implementado — sem narrar etapas intermediárias

## Comandos Comuns

```bash
# Build
dotnet build

# Executar a API
dotnet run

# Executar com watch (reload automático)
dotnet watch run

# Restaurar pacotes
dotnet restore

# Publicar
dotnet publish -c Release
```

## Arquitetura

Este é um projeto **ASP.NET Core Web API (.NET 8)** que atua como camada de API de um sistema ERP maior. A lógica de negócio, entidades e DbContext estão em projetos externos referenciados.

### Dependências de Projeto Externo

O projeto depende de dois módulos do repositório `ERP_simples`:
- `../ERP_simples/ModuloCadastro/` — Entidades (ex: `UsuarioEntity`), DTOs, Services e DbContext (`ConfigurarContexto()`)
- `../ERP_simples/ModuloConfiguracoes/` — Configurações compartilhadas

Para o projeto compilar, o repositório `ERP_simples` deve estar clonado no mesmo nível de diretório que `API_simples`.

### Estrutura Interna

```
Controllers/
  Usuario/         # Controllers agrupados por domínio
    AuthController.cs    # POST /v1/auth/login (público)
    UsuarioController.cs # GET /v1/GetUsuarios (requer JWT)
Services/
  TokenService.cs  # Geração de JWT (HMAC-SHA256, expiração 1h)
JwtOptions.cs      # Classe de configuração JWT (Key, Issuer, Audience)
Program.cs         # Configuração da aplicação e DI
```

### Padrões de Autenticação

- **Autenticação**: JWT Bearer com BCrypt para hashing de senhas
- **Autorização**: Atributo `[Authorize]` nos controllers
- **Claims**: `ClaimTypes.Name` (email), `ClaimTypes.Role` (perfil), `permissao` (claims customizados por permissão)

### Injeção de Dependência (Program.cs)

- `JwtOptions` → Transient
- `UsuarioService` (do ModuloCadastro) → Scoped
- DbContext configurado via `ConfigurarContexto()` do ModuloCadastro

## Configuração

### JWT

As credenciais JWT ficam em `appsettings.json` na seção `"Jwt"` (Key, Issuer, Audience). Em desenvolvimento, use `dotnet user-secrets` para não commitar credenciais.

### Banco de Dados

MySQL via `Pomelo.EntityFrameworkCore.MySql`. String de conexão em `appsettings.json` → `"ConnectionStrings:DefaultConnection"`.

## Convenções de Rota

- Prefixo `v1/` em todos os endpoints
- Controllers agrupados por domínio em subpastas dentro de `Controllers/`
