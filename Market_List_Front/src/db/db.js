import Dexie from 'dexie';

export const db = new Dexie('MarketListDB');

// Definição do schema das tabelas locais
// Usamos "id" como chave primária auto-incremento local,
// e "idRemote" para mapear com o ID correspondente retornado pelo backend (PostgreSQL).
db.version(1).stores({
  listas: '++id, idRemote, nome, unidadeId, syncStatus',
  itens: '++id, idRemote, nome, sessaoId, unidadeId, setorId, syncStatus',
  itensLista: '++id, idRemote, listaId, itemId, syncStatus',
  ambientes: '++id, idRemote, nome, syncStatus',
  mercados: '++id, idRemote, nome, syncStatus',
  setores: '++id, idRemote, nome, sessaoId, syncStatus'
});

// Helper para verificar se há dados pendentes de sincronização
export async function getUnsyncedCount() {
  const tables = ['listas', 'itens', 'itensLista', 'ambientes', 'mercados', 'setores'];
  let count = 0;
  for (const tableName of tables) {
    const unsynced = await db[tableName].where('syncStatus').notEqual('synced').count();
    count += unsynced;
  }
  return count;
}
