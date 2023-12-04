using System;

class Program
{
    static Clerk clerk = new Clerk();

    static void Main(string[] args)
    {
        while (true)  // Boucle infinie pour garder le programme en cours d'exécution
        {
            Console.WriteLine("1. Ajouter un client");
            Console.WriteLine("2. Supprimer un client");
            Console.WriteLine("3. Mettre à jour un client");
            Console.WriteLine("4. Afficher les clients");
            Console.WriteLine("5. Quitter");
            Console.WriteLine("6. Afficher les clients par ordre alphabétique");
            Console.WriteLine("7. Afficher les clients par ville");
            Console.WriteLine("8. Afficher les meilleurs clients");
            Console.WriteLine("9. Créer une nouvelle commande");
            Console.WriteLine("10. Ajouter un article à une commande");
            Console.WriteLine("11. Clôturer une commande");
            Console.Write("Entrez votre choix : ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCustomer();
                    break;
                case "2":
                    DeleteCustomer();
                    break;
                case "3":
                    UpdateCustomer();
                    break;
                case "4":
                    DisplayCustomers();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                case "6":
                    DisplayCustomersOrderedByName();
                    break;
                case "7":
                    DisplayCustomersByCity();
                    break;
                case "8":
                    DisplayBestCustomers();
                    break;
                case "9":
                    CreateNewOrder();
                    break;
                case "10":
                    AddItemToOrder();
                    break;
                case "11":
                    CloseOrder();
                    break;
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    break;
            }
        }
    }

    static void AddCustomer()
    {
        Console.Write("Entrez le prénom : ");
        var firstName = Console.ReadLine();
        Console.Write("Entrez le nom : ");
        var lastName = Console.ReadLine();
        Console.Write("Entrez le numéro de téléphone : ");
        var phoneNumber = Console.ReadLine();
        Console.Write("Entrez l'adresse : ");
        var address = Console.ReadLine();
        Console.Write("Entrez la ville : ");
        var city = Console.ReadLine();

        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
            City =  city
        };

        clerk.AddCustomer(customer);
        Console.WriteLine("Client ajouté avec succès.");
    }

    static void DeleteCustomer()
    {
        Console.Write("Entrez l'ID du client à supprimer : ");
        var customerId = int.Parse(Console.ReadLine());
        clerk.DeleteCustomer(customerId);
        Console.WriteLine("Client supprimé avec succès.");
    }

    static void UpdateCustomer()
    {
        Console.Write("Entrez l'ID du client à mettre à jour : ");
        var customerId = int.Parse(Console.ReadLine());
        Console.Write("Entrez le nouveau prénom : ");
        var firstName = Console.ReadLine();
        Console.Write("Entrez le nouveau nom : ");
        var lastName = Console.ReadLine();
        Console.Write("Entrez le nouveau numéro de téléphone : ");
        var phoneNumber = Console.ReadLine();
        Console.Write("Entrez la nouvelle adresse : ");
        var address = Console.ReadLine();
        Console.Write("Entrez la nouvelle Ville : ");
        var city = Console.ReadLine();

        var customer = new Customer
        {
            CustomerId = customerId,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
            City = city
        };

        clerk.UpdateCustomer(customer);
        Console.WriteLine("Client mis à jour avec succès.");
    }

    static void DisplayCustomers()
    {
        var customers = clerk.GetCustomers();
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId}: {customer.FirstName} {customer.LastName}, {customer.PhoneNumber}, {customer.Address}, {customer.City }");
        }
    }

    static void DisplayCustomersOrderedByName()
    {
        var customers = clerk.GetCustomersOrderedByName();
        DisplayCustomers(customers);
    }

    static void DisplayCustomersByCity()
    {
        Console.Write("Entrez la ville : ");
        var city = Console.ReadLine();
        var customers = clerk.GetCustomersByCity(city);
        DisplayCustomers(customers);
    }

    static void DisplayBestCustomers()
    {
        var customers = clerk.GetBestCustomers();
        DisplayCustomers(customers);
    }

    static void DisplayCustomers(List<Customer> customers)
    {
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId}: {customer.FirstName} {customer.LastName}, {customer.PhoneNumber}, {customer.Address}, {customer.City}");
        }
    }

    static void CreateNewOrder()
    {
        Console.Write("Entrez l'ID du client : ");
        var customerId = int.Parse(Console.ReadLine());
        var orderId = clerk.CreateNewOrder(customerId);
        Console.WriteLine($"Commande créée avec succès. ID de commande : {orderId}");
    }

    static void AddItemToOrder()
    {
        Console.Write("Entrez l'ID de la commande : ");
        var orderId = int.Parse(Console.ReadLine());
        Console.Write("Entrez le nom du produit : ");
        var productName = Console.ReadLine();
        Console.Write("Entrez le prix unitaire : ");
        var unitPrice = decimal.Parse(Console.ReadLine());
        Console.Write("Entrez la quantité : ");
        var quantity = int.Parse(Console.ReadLine());

        clerk.AddOrderItem(orderId, productName, unitPrice, quantity);
        Console.WriteLine("Article ajouté avec succès.");
    }

    static void CloseOrder()
    {
        Console.Write("Entrez l'ID de la commande : ");
        var orderId = int.Parse(Console.ReadLine());
        clerk.CloseOrder(orderId);
        Console.WriteLine("Commande clôturée avec succès.");
    }

}
