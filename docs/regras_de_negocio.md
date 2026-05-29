# Documento de Regras de Negócio - Market List

Este documento detalha o funcionamento funcional, as regras de domínio e as diretrizes de negócio que regem o ecossistema do **Market List**.

---

## 1. Visão Geral do Domínio

O Market List foi concebido para resolver o problema de logística de compras de insumos para múltiplos estabelecimentos (ex: uma rede de lanchonetes ou filiais). 

Tradicionalmente, um gerente de compras precisava consolidar manualmente as necessidades de cada ambiente (Loja 1, Loja 2, Loja 3), anotando itens repetidos, somando quantidades e indo ao mercado com várias listas físicas, o que gerava confusão e desperdício de tempo. 

O Market List centraliza, unifica e automatiza esse processo de forma offline-first, permitindo que as compras ocorram em locais com conexões instáveis (como galpões de atacado e feiras).

---

## 2. Entidades de Domínio

### 2.1. Usuário (Usuario)
Representa a conta do usuário no sistema. 
*   **Regras:**
    *   Deve possuir um e-mail único ativo e validado.
    *   Suporta autenticação por e-mail/senha ou via provedores externos (Login Social com Google e Facebook).
    *   Um usuário pode estar associado a uma ou mais **Unidades (Ambientes)**.

### 2.2. Unidade / Ambiente (Unidade)
Representa as origens das demandas de compras (ex: "Loja Principal", "Filial Barra", "Depósito Centro").
*   **Regras:**
    *   Uma unidade pertence a um ou mais usuários (relação muitos para muitos gerenciada por `UsuarioUnidade`).
    *   Cada unidade possui o seu próprio conjunto de **Listas de Compras** ativas e histórico.

### 2.3. Mercado / Estabelecimento (Sessao)
Representa o local físico de compras onde os itens são adquiridos (ex: "Ceasa", "Assaí Atacadista", "Zona Sul").
*   **Regras:**
    *   Usado para categorizar onde cada produto do catálogo deve ser comprado.
    *   Permite filtrar a lista mestra de compras durante o "Modo Compras" para exibir apenas os produtos daquele mercado específico.

### 2.4. Setor / Corredor (Setor)
Representa as seções internas de um mercado específico (ex: "Frios", "Hortifrúti", "Limpeza", "Bebidas").
*   **Regras:**
    *   Um setor é obrigatoriamente vinculado a um único **Mercado** (`NIdSessao`).
    *   Usado para ordenar e filtrar os produtos no Modo Compras, permitindo que o comprador pegue todos os itens de um mesmo corredor de uma vez antes de passar para o próximo.

### 2.5. Item / Produto (Item)
Representa um produto cadastrado no catálogo do usuário.
*   **Regras:**
    *   O item é associado a um **Ambiente (Unidade)** de origem.
    *   Possui obrigatoriamente um nome e uma **Unidade de Medida** (ex: UN, KG, LT, CX, PCT) com tamanho máximo de 3 caracteres.
    *   É associado a um **Mercado** padrão para indicar onde costuma ser comprado.
    *   Pode ser opcionalmente associado a um **Setor** do mercado para facilitação tátil.

### 2.6. Lista de Compras (Lista)
Representa uma lista individual criada para um determinado Ambiente.
*   **Regras:**
    *   É criada dentro do contexto de uma **Unidade (Ambiente)**.
    *   Contém uma coleção de itens vinculados a quantidades específicas através de `ItemLista`.
    *   Não pode conter o mesmo produto do catálogo repetido em uma mesma lista (a quantidade deve ser somada).

---

## 3. Fluxo de Junção de Listas (List Merging)

A funcionalidade principal do sistema é o agrupamento de listas de compras.

### 3.1. Processo de Consolidação
*   O usuário seleciona 2 ou mais listas de compras ativas de diferentes ambientes.
*   O sistema cria um **Agrupador** (Lista Mestra) com um nome descritivo (ex: "Compras Quarta Ceasa").
*   O algoritmo de junção varre todos os itens das listas selecionadas e os unifica pelo nome do item.
*   As quantidades são somadas automaticamente para gerar a quantidade consolidada final.

### 3.2. Rastreabilidade de Origem (Tags)
*   O agrupamento **não pode** ocultar a necessidade individual de cada loja.
*   Ao visualizar o item consolidado, o sistema exibe tags visuais informando quanto daquele item pertence a cada ambiente original.
    *   *Exemplo:* O item consolidado **Cebola (Kg)** exibe a quantidade total **15 KG** e as tags de origem `[Loja 1: 5 KG]` e `[Loja 2: 10 KG]`.
*   Essa regra garante que, após a compra, o distribuidor saiba exatamente como fracionar as sacolas de entrega para cada loja física.

---

## 4. Modo Compras e Registro de Preços

Durante o ato da compra física, o aplicativo entra no **Modo Compras**.

### 4.1. Comportamento e Navegação
*   A tela exibe apenas os itens pendentes daquela lista ou agrupamento de listas.
*   Exibe abas de **Setores** (ex: "Frios", "Padaria") no topo para navegação rápida de acordo com os itens presentes.
*   O comprador pode filtrar a lista pelo **Mercado** onde está comprando.

### 4.2. Registro de Preços
*   Ao pegar um produto e colocá-lo no carrinho, o comprador toca no checkbox circular e o marca como **Comprado** (`StatusItemListaId = 3`).
*   Ao fazer o *check*, um campo de entrada numérica é exibido para o preenchimento do **Preço Unitário Real** do produto na prateleira.
*   O sistema multiplica a quantidade total do item pelo preço digitado e atualiza o **Totalizador Financeiro** no rodapé da tela em tempo real.
*   O preço digitado é gravado imediatamente e de forma atômica no banco de dados local.

---

## 5. Funcionamento Offline-First

Uma premissa de negócio crucial é que o aplicativo deve funcionar **sem internet**.
*   Todas as ações (criar listas, cadastrar produtos, ajustar preços, marcar itens como comprado, deletar elementos) são salvas imediatamente no banco local do dispositivo.
*   O sistema não bloqueia o fluxo de trabalho se a API retornar erro de rede.
*   Assim que o sinal de internet é detectado, a fila de alterações local (Sync Queue) é processada sequencialmente em segundo plano para persistir os dados no banco de dados central PostgreSQL, garantindo a integridade dos dados históricos.
