using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;
        public SQLiteDatabaseHelper(string path) 
        {
        _conn = new SQLiteAsyncConnection(path);   
        _conn.CreateTableAsync<Produto>().Wait();
        EnsureDataCadastroColumn();
        }

        void EnsureDataCadastroColumn()
        {
            try
            {
                _conn.ExecuteAsync(
                    "ALTER TABLE Produto ADD COLUMN DataCadastro DATETIME NOT NULL DEFAULT '0001-01-01 00:00:00'")
                    .Wait();
            }
            catch
            {
                // Coluna ja existe.
            }

            _conn.ExecuteAsync(
                "UPDATE Produto SET DataCadastro = ? WHERE DataCadastro IS NULL OR DataCadastro = '0001-01-01 00:00:00' OR DataCadastro = '0001-01-01T00:00:00'",
                DateTime.Today)
                .Wait();
        }
        public Task<int> Insert(Produto p) 
        {
            return _conn.InsertAsync(p);
        }
        public Task<int> Update(Produto p) 
        { 
            string sql = "UPDATE Produto SET Descricao = ?, Quantidade = ?, Preco = ?, DataCadastro = ? WHERE Id = ?";
            return _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.DataCadastro, p.Id);
        }
        public Task<int> Delete(int id) 
        { 
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll() 
        { 
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * FROM Produto WHERE descricao  LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }
        public async Task<List<Produto>> GetByPeriod(DateTime dataInicial, DateTime dataFinal)
        {
            List<Produto> produtos = await _conn.Table<Produto>().ToListAsync();

            return produtos
                .Where(p => p.DataCadastro.Date >= dataInicial.Date && p.DataCadastro.Date <= dataFinal.Date)
                .OrderBy(p => p.DataCadastro)
                .ToList();
        }
    }
}
