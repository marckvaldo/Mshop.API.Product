# API de catalado de produtos 

<div style="background-color: #f0f0f0; height:20vh; display: flex; flex-direction: row; justify-content: center;">
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Grafana.png">
</div>

## Descrição do Projeto
Esse projeto tem o intuito de aplicar conceitos importantes como;
- Teste automatizado (TDD)
- Cacheamento com Redis
- Arquitetura hexagonal
- Clean code
- SOLID.
- Design Patterns
- Event Domain

Pretendo aplicar essa API em uma arquitetura de microserviço em um futuro próximo, aplicandos conceitos de Microserviço como 
- Messageria
- API Gateway
- Autenticação com keycloak

<h4> 
	🚧  API produtos 🚀 Em construção...  🚧
</h4>

### Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:

[Git](https://git-scm.com)<br/>
[Dot.net (6.0)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0). <br/>
[Mysql](https://www.mysql.com/downloads/).<br/>
[Redis](https://redis.io/download/). (Recomendo fortmento caso você esteja no windows executar no docker)<br/>

Além disso é bom ter um editor de código para trabalhar como [VSCode](https://code.visualstudio.com/)
ou se prefeir uma IDE, o velhor e bom [Visual Estudio 2022](https://visualstudio.microsoft.com/pt-br/downloads/).

### 🎲 Rodando

```bash
# Clone este repositório
$ git clone <https://github.com/marckvaldo/Mshop.API.Product>

# Acesse a pasta do projeto no terminal/cmd
$ cd Mshop.API.Product/env

# levante os docker 
$ docker-compose up -d

# executar as migrations 
$ cd Mshop.API.Product/src/MShop.Repository
dotnet ef --startup-project ../MShop.ProductAPI/ database update

# O serviço inciará na porta:5000 - acesse <http://localhost:5000>
# pronto tempos a API funcionando.
```
### 🚀 Algumas imagens

#### API
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Docker.png">

### 🛠 Configuração
Todas as configurações do projeto estão em Mshop.API.Product\src\MShop.ProductAPI\appsettings.Development.json

### 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [ASP.net](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Mysql](https://www.mysql.com/)
- [Redis](https://redis.io/)
- [Docker](https://www.docker.com/)


#📝 Licença
Este projeto esta sobe a licença MIT.

Feito com ❤️ por Marckvaldo Wallas 👋🏽 Entre em contato (marckvaldo@hotmail.com, marckvaldowallas@gmail.com)

