# API de catalado de produtos 

<div style="background-color: #f0f0f0; height:20vh; display: flex; flex-direction: row; justify-content: center;">
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Grafana.png">
</div>

![GitHub](https://img.shields.io/github/license/marckvaldo/docker-php)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/marckvaldo/docker-php)


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
[Dot.net (6.0)](https://docs.docker.com/engine/install/). 
[Mysql](https://docs.docker.com/engine/install/).

Além disso é bom ter um editor para trabalhar com o código como [VSCode](https://code.visualstudio.com/)
ou se prefeir um IDE o velhor e bom [Visual Estudio 2022](https://docs.docker.com/engine/install/).

### 🎲 Rodando

```bash
# Clone este repositório
$ git clone <https://github.com/marckvaldo/docker-monitor.git>

# Acesse a pasta do projeto no terminal/cmd
$ cd Msho.API.Product/env

# levante os docker 
$ docker-compose up -d

# O serviço inciará na porta:5000 - acesse <http://localhost:5000>
# pronto tempos a API funcinando.
```
### 🚀 Algumas imagens

#### Painel Docker
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Docker.png">

#### Painel Mysql
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Mysql.png">
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Mysql2.png">

#### Painel Nginx
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Nginx.png">

#### Painel Sistema
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Sistema.png">
<img src="https://github.com/marckvaldo/docker-monitor/blob/main/imagens/Sistema2.png">

### 🛠 Configuração
Todas as configurações do projeto estão no arquivo .env

Atenção Especial para as variaveis 
STATUS_NGINX & MYSQL_STRING 
onde tiver host-nginx e host-mysql substituir para o host da maquina que está executando os respequitivos serviços

Na variavel LOG_NGINX vai colocar o caminho do arquivo log "access.log" do nginx

### 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [Grafana](https://grafana.com/)
- [InfluxDb](https://www.influxdata.com/)
- [Telegaf](https://docs.influxdata.com/telegraf/v1.19/)
- [Docker](https://www.docker.com/)


#📝 Licença
Este projeto esta sobe a licença MIT.

Feito com ❤️ por Marckvaldo Wallas 👋🏽 Entre em contato (marckvaldo@hotmail.com, marckvaldowallas@gmail.com)
