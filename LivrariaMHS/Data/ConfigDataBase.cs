using LivrariaMHS.Models;
using Microsoft.EntityFrameworkCore;

namespace LivrariaMHS.Data
{
    public class ConfigDataBase
    {
        public readonly LivrariaMHSContext _context;
        public ConfigDataBase(LivrariaMHSContext context)
        {
            _context = context;
        }
        public void DataBaseInitializer()
        {
            _context.Database.Migrate();
            CriarTriggers();
            CriarProcedures();
            CriarFunctions();
        }

        // TRIGGERS
        public void CriarTriggers()
        {
            //Trigger para Verificar se um autor deve ser apagado (se não possuir nenhum livro relacionado)
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER TRIGGER ValidarExistenciaAutor " +
                "ON dbo.livros AFTER UPDATE, DELETE AS " +
                    "DECLARE @IdAutorLivroApagado INT " +
                    "SELECT @IdAutorLivroApagado = AutorID FROM deleted " +
                    "DELETE FROM AUTORES " +
                        "WHERE(SELECT COUNT(*) FROM LIVROS WHERE AutorID = @IdAutorLivroApagado) = 0 " +
                        "AND ID = @IdAutorLivroApagado"
            );

            //Trigger para Verificar se algo do endereço do cliente deve ser apagado (se não possuir entidade associada)
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER TRIGGER ValidarExistenciaEnderecos " +
                "ON dbo.clientes AFTER UPDATE, DELETE AS " +
                    "DECLARE @IdCidadeAntiga INT " +
                    "DECLARE @IdRuaAntiga INT " +
                    "DECLARE @IdBairroAntigo INT " +
                    "SELECT @IdCidadeAntiga = CidadeID FROM DELETED " +
                    "SELECT @IdRuaAntiga = RuaID FROM DELETED " +
                    "SELECT @IdBairroAntigo = BairroID FROM DELETED " +
                    "DELETE FROM Cidades " +
                        "WHERE(SELECT COUNT(*) FROM CLIENTES WHERE CidadeID = @IdCidadeAntiga) = 0 " +
                        "AND ID = @IdCidadeAntiga " +
                    "DELETE FROM Ruas " +
                        "WHERE(SELECT COUNT(*) FROM CLIENTES WHERE RuaID = @IdRuaAntiga) = 0 " +
                        "AND ID = @IdRuaAntiga " +
                    "DELETE FROM Bairros " +
                        "WHERE(SELECT COUNT(*) FROM CLIENTES WHERE BairroID = @IdBairroAntigo) = 0 " +
                        "AND ID = @IdBairroAntigo"
            );
        }

        // PROCEDURES
        public void CriarProcedures()
        {
            //Procedure sem retorno que incrementa ou diminui o preço de todos os livros de acordo com uma porcentagem informada
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER PROCEDURE AlterPrecoLivros @PORCENTAGEM DECIMAL(18, 2), @TIPO CHAR(1) AS " +
                    "IF(@TIPO = '+') " +
                        "BEGIN UPDATE LIVROS SET PRECO = ((1 + (@PORCENTAGEM / 100)) * PRECO) END " +
                    "IF(@TIPO = '-') " +
                        "BEGIN UPDATE LIVROS SET PRECO = ((1 - (@PORCENTAGEM / 100)) * PRECO) END"
            );

            //Procedure com retorno que faz a média do valor das vendas realizadas dentro do intervalo informado
            /*_context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER PROCEDURE ValorMedioDasVendas @DataInicial date, @DataFinal date  AS " +
                    "SELECT CONVERT(DECIMAL(10, 2), AVG(Quantidade * ValorUnitario)) " +
                    "FROM Vendas  " +
                    "WHERE convert(CHAR, CAST(Data AS DATE),103) BETWEEN convert(CHAR,@DataInicial,103) AND convert(CHAR,@DataFinal,103)"
            );*/

            //Procedure com retorno que devolve o valor total das vendas dentro do intervalo informado
            /*_context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER FUNCTION ValorTotalDasVendas @DataInicial date, @DataFinal date AS " +
                    "SELECT CONVERT(DECIMAL(10,2), SUM(Quantidade * ValorUnitario)) " +
                    "FROM Vendas " +
                    "WHERE convert(CHAR, CAST(Data AS DATE),103) BETWEEN convert(CHAR,@DataInicial,103) AND convert(CHAR,@DataFinal,103)"
            );*/
        }

        public void CriarFunctions()
        {
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER FUNCTION ValorMedioDasVendas (@DataInicial date, @DataFinal date) " +
                "RETURNS TABLE " +
                "AS RETURN ( " +
                    "SELECT CONVERT(DECIMAL(10, 2), AVG(Quantidade * ValorUnitario)) AS MEDIA " +
                    "FROM Vendas " +
                    "WHERE convert(CHAR, CAST(Data AS DATE), 103) " +
                    "BETWEEN convert(CHAR, @DataInicial, 103) " +
                    "AND convert(CHAR, @DataFinal, 103) " +
                ")"
            );

            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER FUNCTION ValorTotalDasVendas (@DataInicial date, @DataFinal date) " +
                "RETURNS TABLE " +
                "AS RETURN ( " +
                    "SELECT CONVERT(DECIMAL(10,2), SUM(Quantidade * ValorUnitario)) AS TOTAL " +
                    "FROM Vendas " +
                    "WHERE convert(CHAR, CAST(Data AS DATE), 103) " +
                    "BETWEEN convert(CHAR,@DataInicial, 103) " +
                    "AND convert(CHAR, @DataFinal, 103) " +
                    ")"
            );
        }
    }
}
