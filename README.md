# POC DynamoDb :sunglasses:

- POC para estudos de como funciona o dynamodb.

## Estrelinha! :star:

Se esse projeto te ajudou em alguma coisa, taquele pau na estrelinha \o/

## Dependências Externas ❗

- Necessario conta no AWS


## Variáveis de ambiente ❗

- Configurar as variáveis de ambiente do aws no arquivo appsettings.


## Anotações 📑

- Funciona com base no conceito de tabelas que funcionam como um catalogo de itens ou agregado de itens e um item é um catalogo/agregado de atributos.

- Os itens da tabela nem sempre precisam ter os mesmos atributos entretanto eles precisam ter um atributo que identifique exclusivamente o item e esse é a "Partition Key" ou identificador de partição.

- Então todo item inserido no DynamoDB precisa ter uma partition key, eu também posso incluir um parametro opicional chamado "Sort Key" ou "Range Key, consiste em uma chave de classificação que me proporciona a "habilidade" de executar consultas de intervalo complexas em itens nessas partições.

- Entenda partições como pastas ou um bucket cheios de itens e a chave de classificação ordena esses itens dentro.

---

- Exemplo: Imagine cenario onde exista uma tabela de pedidos e minha partition key e o id do cliente, e a chave de classificação seria a data dos pedidos e eu precise consultar os pedidos nas ultimas 24 horas... o resultado desse consulta seria altamente performatica e de baixo custo graças as separações feitas.

