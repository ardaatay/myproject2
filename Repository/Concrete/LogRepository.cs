using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class LogRepository(VarlikEnvanteriDbContext context) : ILogRepository
{
    public void AddLog(Log entity)
    {
        using var connection = new NpgsqlConnection(context.Database.GetConnectionString());
        connection.Open();
        using var transaction = connection.BeginTransaction();
        // ADO.NET ile doğrudan log kaydı
        using (var command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            // Tablo ve sütun adları snake_case olduğu için tırnaklamaya gerek yok.
            command.CommandText =
                """
                INSERT INTO logs (method_name, class_name, parameters, executing_time, return_value, error, username)
                VALUES (@MethodName, @ClassName, @Parameters, @ExecutingTime, @ReturnValue, @Error, @Username)
                """;
            command.Parameters.Add(new NpgsqlParameter("MethodName", entity.MethodName));
            command.Parameters.Add(new NpgsqlParameter("ClassName", entity.ClassName));
            command.Parameters.Add(new NpgsqlParameter("Parameters", entity.Parameters));
            command.Parameters.Add(new NpgsqlParameter("ExecutingTime", entity.ExecutingTime));
            command.Parameters.Add(new NpgsqlParameter("ReturnValue", (object?)entity.ReturnValue ?? DBNull.Value));
            command.Parameters.Add(new NpgsqlParameter("Error", (object?)entity.Error ?? DBNull.Value));
            command.Parameters.Add(new NpgsqlParameter("Username", entity.Username));
            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }
}
