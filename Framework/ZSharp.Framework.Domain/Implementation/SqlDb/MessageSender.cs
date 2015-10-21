using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using ZSharp.Framework.SqlDb;

namespace ZSharp.Framework.Domain
{   
    public class MessageSender : IMessageSender
    {
        private readonly IDbConnectionFactory connectionFactory;
        private readonly string schemaName;
        private readonly string insertQuery;

        public MessageSender(string schemaName, string tableName)
        {
            this.connectionFactory = new CustomConnectionFactory("localhost", "ConferenceDb");
            this.schemaName = schemaName;
            this.insertQuery = string.Format("INSERT INTO {0} (Body, MessageType, DeliveryDate, CorrelationId) VALUES (@Body, @MessageType, @DeliveryDate, @CorrelationId)", tableName);
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        public void Send(Message message)
        {
            using (var connection = this.connectionFactory.CreateConnection(this.schemaName))
            {
                connection.Open();

                InsertMessage(message, connection);
            }
        }

        /// <summary>
        /// Sends a batch of messages.
        /// </summary>
        public void Send(IEnumerable<Message> messages)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var connection = this.connectionFactory.CreateConnection(this.schemaName))
                {
                    connection.Open();

                    foreach (var message in messages)
                    {
                        this.InsertMessage(message, connection);
                    }
                }

                scope.Complete();
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Does not contain user input.")]
        private void InsertMessage(Message message, DbConnection connection)
        {
            using (var command = (SqlCommand)connection.CreateCommand())
            {
                command.CommandText = this.insertQuery;
                command.CommandType = CommandType.Text;

                command.Parameters.Add("@Body", SqlDbType.NVarChar).Value = message.Body;
                command.Parameters.Add("@MessageType", SqlDbType.NVarChar).Value = message.MessageType;
                command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = message.DeliveryDate.HasValue ? (object)message.DeliveryDate.Value : DBNull.Value;
                command.Parameters.Add("@CorrelationId", SqlDbType.NVarChar).Value = (object)message.CorrelationId ?? DBNull.Value;

                command.ExecuteNonQuery();
            }
        }
    }
}
