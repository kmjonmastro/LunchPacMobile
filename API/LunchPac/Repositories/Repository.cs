using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using LunchPac.Attributes;

namespace LunchPac.Repositories
{
    public class Repository<T>
        where T : class, new()
    {
        private static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString; } }

        private static string TableName
        {
            get
            {
                var tableAttribute = typeof(T).GetCustomAttributes(typeof(Table), false).SingleOrDefault() as Table;

                if (tableAttribute == null)
                    throw new Exception(String.Format("There is no Table attribute set up for {0}.", typeof(T).Name));

                return tableAttribute.TableName;
            }
        }

        private static string PrimaryKeyFieldName
        {
            get
            {
                var primaryKey = typeof(T).GetProperties().SingleOrDefault(p => Attribute.IsDefined(p, typeof(PrimaryKey)));

                if (primaryKey == null)
                    throw new Exception(String.Format("There is no primary key set up for {0}.", typeof(T).Name));

                return primaryKey.Name;
            }
        }

        public static T Select(object id)
        {
            return ExecuteSelect(PrimaryKeyFieldName, id).SingleOrDefault();
        }

        public static T Select<TProp>(Expression<Func<T, TProp>> property, object value)
        {
            var fieldName = ((MemberExpression)property.Body).Member.Name;

            return ExecuteSelect(fieldName, value).SingleOrDefault();
        }

        public static IEnumerable<T> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("SELECT * FROM {0}", TableName);

                    return ReadData(cmd);
                }
            }
        }

        public static IEnumerable<T> SelectMany(object id)
        {
            return ExecuteSelect(PrimaryKeyFieldName, id);
        }

        public static IEnumerable<T> SelectMany<TProp>(Expression<Func<T, TProp>> property, object value)
        {
            var fieldName = ((MemberExpression)property.Body).Member.Name;

            return ExecuteSelect(fieldName, value);
        }

        public static T SelectLast()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("SELECT TOP 1 * FROM {0} ORDER BY {1} DESC", TableName, PrimaryKeyFieldName);

                    return ReadData(cmd).SingleOrDefault();
                }
            }
        }

        public static void Delete(object id)
        {
            ExecuteDelete(PrimaryKeyFieldName, id);
        }

        public static void Delete<TProp>(Expression<Func<T, TProp>> property, object value)
        {
            var fieldName = ((MemberExpression)property.Body).Member.Name;

            ExecuteDelete(fieldName, value);
        }

        public static int Insert(T model)
        {
            var fieldsAndValues = GetFieldsAndValues(model).ToList();
            var fields = String.Join(",", fieldsAndValues.Select(f => f.Key));
            var values = String.Join(",", fieldsAndValues.Select(f => String.Format("@{0}", f.Key)));

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2});SELECT SCOPE_IDENTITY()", TableName, fields, values);

                    foreach (var v in fieldsAndValues)
                    {
                        cmd.Parameters.AddWithValue(v.Key, v.Value);
                    }

                    return (int)(decimal)cmd.ExecuteScalar();
                }
            }
        }

        public static void Update(T model)
        {
            var fieldsAndValues = GetFieldsAndValues(model).ToList();
            var query = new StringBuilder();

            foreach (var v in fieldsAndValues)
            {
                query.AppendFormat("{0} = @{0}, ", v.Key);
            }

            var id = (
                from p in typeof(T).GetProperties()
                where Attribute.IsDefined(p, typeof(PrimaryKey))
                select p.GetValue(model, null)
            ).SingleOrDefault();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("UPDATE {0} SET {1} WHERE {2} = @Id", TableName, query.ToString().Trim().TrimEnd(','), PrimaryKeyFieldName);

                    foreach (var v in fieldsAndValues)
                    {
                        cmd.Parameters.AddWithValue(v.Key, v.Value);
                    }

                    cmd.Parameters.AddWithValue("Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        private static IEnumerable<T> ExecuteSelect(string fieldName, object value)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = @Id", TableName, fieldName);
                    cmd.Parameters.AddWithValue("Id", value);

                    return ReadData(cmd);
                }
            }
        }

        private static void ExecuteDelete(string fieldName, object value)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("DELETE FROM {0} WHERE {1} = @Id", TableName, fieldName);
                    cmd.Parameters.AddWithValue("Id", value);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private static IEnumerable<T> ReadData(SqlCommand command)
        {
            var list = new List<T>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    T model = new T();

                    foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite))
                    {
                        dynamic resultValue = reader[property.Name];

                        if (!(resultValue is DBNull))
                            property.SetValue(model, resultValue, null);
                    }

                    list.Add(model);
                }
            }

            return list;
        }

        private static IEnumerable<KeyValuePair<string, object>> GetFieldsAndValues(T model)
        {
            foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite))
            {
                bool isPrimaryKey = Attribute.IsDefined(property, typeof(PrimaryKey));

                if (!isPrimaryKey)
                    yield return new KeyValuePair<string, object>(property.Name, property.GetValue(model, null));
            }
        }
    }
}
