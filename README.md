# API Administrativa onde é possivel cadastrar os produtos do E-commerce. 

<div style="background-color: #f0f0f0; height:20vh; display: flex; flex-direction: row; justify-content: center;">
<img src="https://github.com/marckvaldo/Mshop.API.Product/blob/main/images/eshoponcontainers-reference-application-architecture.png">
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
- monitoramento
- kubernet

<h4> 
	🚧  API produtos 🚀 Em construção...  🚧
</h4>

### Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:

[Git](https://git-scm.com)<br/>
[Dot.net (6.0)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0). <br/>
[Mysql](https://www.mysql.com/downloads/).<br/>
[RabbitMQ](https://www.rabbitmq.com/download.html). (Recomendo fortmento caso você esteja no windows executar no docker)<br/>
[Elasticsearch](https://www.elastic.co/pt/elasticsearch). <br/>
[Kibana](https://www.elastic.co/pt/kibana). <br/>

Além disso é bom ter um editor de código para trabalhar como [VSCode](https://code.visualstudio.com/)
ou se prefeir uma IDE, o velhor e bom [Visual Estudio 2022](https://visualstudio.microsoft.com/pt-br/downloads/).

### 🎲 Rodando

```bash
# Clone este repositório
$ git clone https://github.com/marckvaldo/Mshop.API.Product MShop

# Acesse a pasta do projeto no terminal/cmd
$ cd MShop/env

# levante os docker 
$ docker-compose up -d

# O serviço inciará na porta:5000 - acesse <http://localhost:5000/swagger/index.html>
# pronto temos a API funcionando.
```
### 🚀 Algumas imagens

#### API
<img src="https://github.com/marckvaldo/Mshop.API.Product/blob/main/images/Images.jpg">

### 🛠 Configuração
Todas as configurações do projeto estão em Mshop.API.Product\src\MShop.ProductAPI\appsettings.Development.json

### 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [ASP.net](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Mysql](https://www.mysql.com/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Docker](https://www.docker.com/)


#📝 Licença
Este projeto esta sobe a licença MIT.

Feito com ❤️ por Marckvaldo Wallas 👋🏽 Entre em contato (marckvaldo@hotmail.com, marckvaldowallas@gmail.com)

