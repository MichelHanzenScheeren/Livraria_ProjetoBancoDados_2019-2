using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    public class InitConfig
    {
        public readonly LivrariaMHSContext _context;
        public InitConfig(LivrariaMHSContext context)
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
                "BEGIN " +
                    "DECLARE @IdAutorLivroApagado INT " +
                    "SELECT @IdAutorLivroApagado = AutorID FROM DELETED " +
                    "IF(SELECT COUNT(*) FROM livros WHERE AutorID = @IdAutorLivroApagado) = 0 " +
                        "DELETE FROM AUTORES WHERE ID = @IdAutorLivroApagado " +
                "END"
            );

            //Trigger para Verificar se algo do endereço do cliente deve ser apagado (se não possuir entidade associada)
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER TRIGGER ValidarExistenciaEnderecos " +
                "ON DBO.Clientes AFTER UPDATE, DELETE AS " +
                "BEGIN " +
                    "DECLARE " +
                        "@IdCidadeAntiga INT, " +
                        "@IdRuaAntiga INT, " +
                        "@IdBairroAntigo INT " +
                    "SELECT " +
                        "@IdCidadeAntiga = CidadeID, " +
                        "@IdRuaAntiga = RuaID, " +
                        "@IdBairroAntigo = BairroID FROM DELETED " +

                    "IF(SELECT COUNT(*) FROM Clientes WHERE CidadeID = @IdCidadeAntiga) = 0 " +
                        "DELETE FROM Cidades WHERE ID = @IdCidadeAntiga " +

                    "IF(SELECT COUNT(*) FROM Clientes WHERE RuaID = @IdRuaAntiga) = 0 " +
                        "DELETE FROM Ruas WHERE ID = @IdRuaAntiga " +

                    "IF(SELECT COUNT(*) FROM Clientes WHERE BairroID = @IdBairroAntigo) = 0 " +
                        "DELETE FROM Bairros WHERE ID = @IdBairroAntigo " +
                "END "
            );
        }

        // PROCEDURES
        public void CriarProcedures()
        {
            //Procedure sem retorno que incrementa ou diminui o preço de todos os livros de acordo com uma porcentagem informada
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER PROCEDURE AlterPrecoLivros " +
                "(@PORCENTAGEM DECIMAL(18, 2), @TIPO CHAR(1)) AS " +
                "BEGIN " +
                    "IF(@TIPO = '+') " +
                        "UPDATE LIVROS SET PRECO = ((1 + (@PORCENTAGEM / 100)) * PRECO) " +
                    "ELSE IF(@TIPO = '-') " +
                        "UPDATE LIVROS SET PRECO = ((1 - (@PORCENTAGEM / 100)) * PRECO) " +
                "END "
            );
        }

        public void CriarFunctions()
        {
            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER FUNCTION ValorMedioDasVendas (@DataInicial date, @DataFinal date) " +
                "RETURNS TABLE " +
                "AS RETURN ( " +
                    "SELECT CONVERT(DECIMAL(10, 2), AVG(Quantidade * ValorUnitario)) AS MEDIA " +
                    "FROM Vendas " +
                    "WHERE CAST(Data AS DATE) " +
                    "BETWEEN CAST(@DataInicial AS DATE) " +
                    "AND CAST(@DataFinal AS DATE) " +
                ")"
            );

            _context.Database.ExecuteSqlCommand(
                "CREATE OR ALTER FUNCTION ValorTotalDasVendas (@DataInicial date, @DataFinal date) " +
                "RETURNS TABLE " +
                "AS RETURN ( " +
                    "SELECT CONVERT(DECIMAL(10,2), SUM(Quantidade * ValorUnitario)) AS TOTAL " +
                    "FROM Vendas " +
                    "WHERE CAST(Data AS DATE) " +
                    "BETWEEN CAST(@DataInicial AS DATE) " +
                    "AND CAST(@DataFinal AS DATE) " +
                    ")"
            );
        }
    }
}
