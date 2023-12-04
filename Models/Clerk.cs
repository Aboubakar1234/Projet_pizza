using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Projet_pizza.Models;

public class Clerk
{
    public string Name { get; set; }
    public string Description { get; set; }


    private readonly string _connectionString = "Server=localhost;Database=pizza;User ID=root;Password=Nourr@2005;";

    public void AddCustomer(Customer customer)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Customers (FirstName, LastName, PhoneNumber, Address, City) VALUES (@FirstName, @LastName, @PhoneNumber, @Address, @City)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@City", customer.City);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteCustomer(int customerId)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateCustomer(Customer customer)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Address = @Address, City = @City WHERE CustomerId = @CustomerId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@City", customer.City);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Customers";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        Address = reader.GetString(reader.GetOrdinal("Address")),
                        City = reader.GetString(reader.GetOrdinal("City"))
                    };
                    customers.Add(customer);
                }
            }
        }
        return customers;
    }

    public List<Customer> GetCustomersOrderedByName()
    {
        List<Customer> customers = new List<Customer>();
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Customers ORDER BY LastName, FirstName";  // Tri par nom
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        Address = reader.GetString(reader.GetOrdinal("Address")),
                        City = reader.GetString(reader.GetOrdinal("City"))
                    };
                    customers.Add(customer);
                }
            }
        }
        return customers;
    }

    public List<Customer> GetCustomersByCity(string city)
    {
        List<Customer> customers = new List<Customer>();
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Customers WHERE City = @City";  // Filtrage par ville
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@City", city);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            City = reader.GetString(reader.GetOrdinal("City"))
                        };
                        customers.Add(customer);
                    }
                }
            }
        }
        return customers;
    }

    public List<Customer> GetBestCustomers()
    {
        List<Customer> customers = new List<Customer>();
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"
                SELECT Customers.*, SUM(OrderItems.UnitPrice * OrderItems.Quantity) AS TotalAmount
                FROM Customers
                JOIN Orders ON Customers.CustomerId = Orders.CustomerId
                JOIN OrderItems ON Orders.OrderId = OrderItems.OrderId
                GROUP BY Customers.CustomerId
                ORDER BY TotalAmount DESC;
                ";  // Tri par montant total des commandes
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        Address = reader.GetString(reader.GetOrdinal("Address")),
                        City = reader.GetString(reader.GetOrdinal("City"))
                    };
                    customers.Add(customer);
                }
            }
        }
        return customers;
    }

    public void AddOrder(Order order)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Orders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate); SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                command.Parameters.AddWithValue("@OrderDate", DateTime.Now);  // Supposons que la date de la commande est maintenant
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    order.OrderId = Convert.ToInt32(result);  // Stockez l'ID de la commande nouvellement créée dans l'objet Order
                }
            }
        }
    }


    public void AddOrderItem(OrderItem item)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO OrderItems (OrderId, ProductName, UnitPrice, Quantity) VALUES (@OrderId, @ProductName, @UnitPrice, @Quantity)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderId", item.OrderId);
                command.Parameters.AddWithValue("@ProductName", item.ProductName);
                command.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                command.ExecuteNonQuery();
            }
        }
    }

    public int CreateNewOrder(int customerId)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Orders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate); SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                command.Parameters.AddWithValue("@OrderDate", DateTime.Now);  // Supposons que la date de la commande est maintenant
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);  // Retourne l'ID de la commande nouvellement créée
                }
                else
                {
                    throw new Exception("Erreur lors de la création de la commande.");
                }
            }
        }
    }

    public void AddOrderItem(int orderId, string productName, decimal unitPrice, int quantity)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO OrderItems (OrderId, ProductName, UnitPrice, Quantity) VALUES (@OrderId, @ProductName, @UnitPrice, @Quantity)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@ProductName", productName);
                command.Parameters.AddWithValue("@UnitPrice", unitPrice);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.ExecuteNonQuery();
            }
        }
    }

    public void CloseOrder(int orderId)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE Orders SET Status = 'Completed' WHERE OrderId = @OrderId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.ExecuteNonQuery();
            }
        }
    }


}
