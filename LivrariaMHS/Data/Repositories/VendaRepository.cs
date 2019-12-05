using Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Model.Attributes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Data.Repositories
{
    public class VendaRepository : Repository<Venda>
    {
        public VendaRepository(LivrariaMHSContext context) : base(context)
        {
        }

        public string ValorMedioDasVendas(DateTime inicio, DateTime fim)
        {
            var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM ValorMedioDasVendas(@DataInicial, @DataFinal)";
                command.Parameters.AddWithValue("@DataInicial", inicio.Date);
                command.Parameters.AddWithValue("@DataFinal", fim.Date);
                return command.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
            

        public string ValorTotalDasVendas(DateTime inicio, DateTime fim)
        {
                var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
                try
                {
                    string init = inicio.ToShortDateString();
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM ValorTotalDasVendas(@DataInicial, @DataFinal)";
                    command.Parameters.AddWithValue("@DataInicial", inicio.Date);
                    command.Parameters.AddWithValue("@DataFinal", fim.Date);
                    return command.ExecuteScalar().ToString();
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }

        
    }
}
