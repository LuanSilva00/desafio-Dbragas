# Gerenciamento de Clientes em ASP.NET Core

Bem-vindo ao sistema de gerenciamento de clientes desenvolvido em ASP.NET Core! Este projeto oferece uma solução para importar e listar clientes em um banco de dados relacional, com a capacidade de criar automaticamente tipos de clientes.

## Funcionalidades

- **Importação de Clientes**: O sistema permite a importação de clientes a partir de um arquivo Excel (XLSX). Os clientes podem ser adicionados em massa, facilitando a migração de dados.

- **Criação Automática de Tipos de Clientes**: Se um tipo de cliente não existir no banco de dados durante a importação, o sistema o criará automaticamente. Isso ajuda a manter os tipos de cliente organizados e consistentes.

- **DB Seed**: O projeto inclui um mecanismo de sementes (DB Seed) para popular o banco de dados com dados iniciais. Isso facilita a configuração inicial do sistema.

- **Migrações**: Utiliza migrações para garantir que o banco de dados esteja sempre atualizado com o modelo de dados do sistema.

## Configuração

Siga estas etapas para configurar e executar o projeto em sua máquina:

1. **Clonar o repositório**: Clone este repositório em sua máquina local.

   ```shell
   git clone https://github.com/seu-usuario/seu-projeto.git
