using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DAL;

namespace MAPPER
{
    public abstract class AbstractMapper<T> : IGenericMapper<T> where T : class
    {
        protected readonly DBConnection _dbConnection;

        public AbstractMapper()
        {
            _dbConnection = new DBConnection();
        }

        // Abstract methods to be implemented by specific mappers
        protected abstract string GetTableName();
        protected abstract string GetPrimaryKeyName();
        protected abstract T Map(SqlDataReader reader);
        protected abstract void AddParametersForInsert(SqlCommand command, T entity);
        protected abstract void AddParametersForUpdate(SqlCommand command, T entity);
        protected abstract string GetInsertQuery();
        protected abstract string GetUpdateQuery();

        public T GetById(int id)
        {
            T entity = null;
            string query = $"SELECT * FROM {GetTableName()} WHERE {GetPrimaryKeyName()} = @Id";

            using (var connection = _dbConnection.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = Map(reader);
                        }
                    }
                }
            }
            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            var list = new List<T>();
            string query = $"SELECT * FROM {GetTableName()}";

            using (var connection = _dbConnection.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Map(reader));
                        }
                    }
                }
            }
            return list;
        }

        public void Add(T entity)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                using (var command = new SqlCommand(GetInsertQuery(), connection))
                {
                    AddParametersForInsert(command, entity);
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve auto-increment ID
                    command.CommandText = "SELECT SCOPE_IDENTITY();";
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        var newId = Convert.ToInt32(result);
                        SetEntityId(entity, newId);
                    }
                }
            }
        }

        public void Update(T entity)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                using (var command = new SqlCommand(GetUpdateQuery(), connection))
                {
                    AddParametersForUpdate(command, entity);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            string query = $"DELETE FROM {GetTableName()} WHERE {GetPrimaryKeyName()} = @Id";

            using (var connection = _dbConnection.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // Virtual method so child classes can override if they need to set the auto-generated ID back
        protected virtual void SetEntityId(T entity, int id)
        {
            // By default, try to find an "Id" property and set it
            var propInfo = typeof(T).GetProperty("Id");
            if (propInfo != null && propInfo.CanWrite)
            {
                propInfo.SetValue(entity, id);
            }
        }
    }
}
