# Market List - Monorepo

O **Market List** é um aplicativo multiplataforma concebido para otimizar e agilizar o processo de listas de compras para múltiplos estabelecimentos (como filiais de lojas, lanchonetes ou residências). O sistema consolida automaticamente diferentes demandas em uma única lista unificada de compras, mantendo a rastreabilidade da origem de cada insumo.

Uma das principais premissas do sistema é o funcionamento **offline-first**, garantindo que o usuário consiga gerenciar suas listas e registrar preços mesmo sem conexão com a internet (como dentro de galpões de atacado e feiras).

---

## 📂 Estrutura do Monorepo

Este repositório é organizado no formato de monorepo:

*   **[`Market_List_API/`](./Market_List_API):** Backend RESTful desenvolvido em **C# .NET 8** utilizando Entity Framework Core e PostgreSQL.
*   **[`Market_List_Front/`](./Market_List_Front):** Frontend responsivo desenvolvido em **React (Vite)** com estilização CSS Vanilla e banco de dados local **Dexie.js (IndexedDB)** para suporte offline.
*   **[`helm/`](./helm):** Manifestos de deploy do **Helm** para implantação automatizada em clusters Kubernetes.
*   **[`.github/workflows/deploy.yml`](./.github/workflows/deploy.yml):** Pipeline CI/CD automatizado no **GitHub Actions** para testes, build e publicação de imagens Docker no GitHub Container Registry (GHCR).
*   **[`docs/`](./docs):** Documentação técnica e de negócio detalhada.
    *   [Regras de Negócio](./docs/regras_de_negocio.md): Comportamento de domínios, junção de listas e Modo Compras.
    *   [Arquitetura e Tecnologia](./docs/arquitetura_e_tecnologia.md): Mapeamentos de banco (PostgreSQL e Dexie), mecânica de Sync Queue e infraestrutura Cloud.

---

## ⚡ Inicialização Rápida (Desenvolvimento Local)

A forma mais rápida de rodar toda a aplicação integrada na sua máquina (banco de dados, API e Frontend) é utilizando o **Docker Compose**.

### Pré-requisitos
*   Possuir o [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e rodando.
*   Possuir o Git configurado.

### Executando o Projeto
1.  Clone o repositório na sua máquina (se já não o fez).
2.  Abra a pasta do projeto no terminal e execute o seguinte comando:
    ```bash
    docker-compose up --build
    ```
3.  **Acesso às aplicações:**
    *   **Frontend Web:** Acesse [http://localhost/](http://localhost/)
    *   **Backend API:** Acesse [http://localhost:5000/](http://localhost:5000/) ou a versão HTTPS em [https://localhost:5001/](https://localhost:5001/)
    *   **PostgreSQL:** O banco estará acessível na porta local `5432` (Credenciais: `postgres` / `mysecretpassword`).

---

## ☁️ Deploy e Hospedagem

### Hospedagem Gratuita Temporária (Dev/Testes)
*   **Frontend:** Hospedado gratuitamente na **Vercel** ou **Netlify** a partir da pasta `Market_List_Front`.
*   **Backend:** Hospedado gratuitamente no **Render** utilizando o `Dockerfile` da pasta `Market_List_API`.
*   **Database:** Banco PostgreSQL Serverless gratuito instanciado no **Neon.tech** ou no **Supabase**.

### Migração para Produção (AWS)
O projeto está pronto para ser escalado para a **AWS** (Amazon Web Services) através de:
*   **Amazon EKS (Kubernetes)** rodando os manifestos Helm contidos na pasta `helm`.
*   **Amazon ECS + AWS Fargate** para rodar a API .NET 8 em containers serverless mais baratos e simples de gerenciar.
*   **Amazon RDS PostgreSQL** para banco de dados relacional robusto.
*   **Amazon S3 + CloudFront (CDN)** para servir o frontend React estático a custo extremamente baixo e velocidade global.

---

## 📖 Documentação Detalhada

Para entender o funcionamento profundo do sistema, consulte os links abaixo:
*   👉 **[Manual de Regras de Negócio e Domínio](./docs/regras_de_negocio.md)**
*   👉 **[Especificação de Arquitetura, Dexie.js e Sync Queue](./docs/arquitetura_e_tecnologia.md)**
