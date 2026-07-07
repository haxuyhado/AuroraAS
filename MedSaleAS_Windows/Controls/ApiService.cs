using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WPFModernVerticalMenu
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private string baseAddress = "https://localhost:44367/";

        public ApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Employees Methods
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/employees");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Employee>>();
            }
            throw new Exception($"Ошибка при получении данных сотрудников: {response.StatusCode}");
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Employee>();
            }
            throw new Exception($"Ошибка при получении данных сотрудника: {response.StatusCode}");
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/employees", employee);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Employee>();
            }
            throw new Exception($"Ошибка при создании сотрудника: {response.StatusCode}");
        }

        public async Task UpdateEmployeeAsync(int id, Employee employee)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/employees/{id}", employee);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении сотрудника: {response.StatusCode}");
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/employees/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении сотрудника: {response.StatusCode}");
            }
        }
        #endregion

        #region Positions Methods
        public async Task<List<Position>> GetPositionsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/positions");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Position>>();
            }
            throw new Exception($"Ошибка при получении данных должностей: {response.StatusCode}");
        }

        public async Task<Position> GetPositionAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/positions/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Position>();
            }
            throw new Exception($"Ошибка при получении данных должности: {response.StatusCode}");
        }

        public async Task<Position> CreatePositionAsync(Position position)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/positions", position);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Position>();
            }
            throw new Exception($"Ошибка при создании должности: {response.StatusCode}");
        }

        public async Task UpdatePositionAsync(int id, Position position)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/positions/{id}", position);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении должности: {response.StatusCode}");
            }
        }

        public async Task DeletePositionAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/positions/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении должности: {response.StatusCode}");
            }
        }
        #endregion

        #region Clients Methods
        public async Task<List<Client>> GetClientsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/clients");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Client>>();
            }
            throw new Exception($"Ошибка при получении данных клиентов: {response.StatusCode}");
        }

        public async Task<Client> GetClientAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/clients/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Client>();
            }
            throw new Exception($"Ошибка при получении данных клиента: {response.StatusCode}");
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/clients", client);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Client>();
            }
            throw new Exception($"Ошибка при создании клиента: {response.StatusCode}");
        }

        public async Task UpdateClientAsync(int id, Client client)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/clients/{id}", client);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении клиента: {response.StatusCode}");
            }
        }

        public async Task DeleteClientAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/clients/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении клиента: {response.StatusCode}");
            }
        }
        #endregion

        #region Products Methods
        public async Task<List<Product>> GetProductsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/products");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Product>>();
            }
            throw new Exception($"Ошибка при получении данных продуктов: {response.StatusCode}");
        }

        public async Task<Product> GetProductAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Product>();
            }
            throw new Exception($"Ошибка при получении данных продукта: {response.StatusCode}");
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/products", product);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Product>();
            }
            throw new Exception($"Ошибка при создании продукта: {response.StatusCode}");
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/products/{id}", product);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении продукта: {response.StatusCode}");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении продукта: {response.StatusCode}");
            }
        }
        #endregion

        #region Orders Methods
        public async Task<List<Order>> GetOrdersAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/orders");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Order>>();
            }
            throw new Exception($"Ошибка при получении данных заказов: {response.StatusCode}");
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Order>();
            }
            throw new Exception($"Ошибка при получении данных заказа: {response.StatusCode}");
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/orders", order);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Order>();
            }
            throw new Exception($"Ошибка при создании заказа: {response.StatusCode}");
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/orders/{id}", order);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении заказа: {response.StatusCode}");
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/orders/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении заказа: {response.StatusCode}");
            }
        }
        #endregion

        #region ItemsInOrder Methods
        public async Task<List<ItemsInOrder>> GetItemsInOrderAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/itemsinorder");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<ItemsInOrder>>();
            }
            throw new Exception($"Ошибка при получении данных позиций заказа: {response.StatusCode}");
        }

        public async Task<ItemsInOrder> GetItemInOrderAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/itemsinorder/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ItemsInOrder>();
            }
            throw new Exception($"Ошибка при получении данных позиции заказа: {response.StatusCode}");
        }

        public async Task<ItemsInOrder> CreateItemInOrderAsync(ItemsInOrder item)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/itemsinorder", item);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ItemsInOrder>();
            }
            throw new Exception($"Ошибка при создании позиции заказа: {response.StatusCode}");
        }

        public async Task UpdateItemInOrderAsync(int id, ItemsInOrder item)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/itemsinorder/{id}", item);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении позиции заказа: {response.StatusCode}");
            }
        }

        public async Task DeleteItemInOrderAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/itemsinorder/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении позиции заказа: {response.StatusCode}");
            }
        }
        #endregion

        #region CreateProduct Methods
        public async Task<List<CreateProduct>> GetCreateProductsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/createproduct");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<CreateProduct>>();
            }
            throw new Exception($"Ошибка при получении данных создания продуктов: {response.StatusCode}");
        }

        public async Task<CreateProduct> GetCreateProductAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/createproduct/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<CreateProduct>();
            }
            throw new Exception($"Ошибка при получении данных создания продукта: {response.StatusCode}");
        }

        public async Task<CreateProduct> CreateCreateProductAsync(CreateProduct createProduct)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/createproduct", createProduct);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<CreateProduct>();
            }
            throw new Exception($"Ошибка при создании записи о создании продукта: {response.StatusCode}");
        }

        public async Task UpdateCreateProductAsync(int id, CreateProduct createProduct)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/createproduct/{id}", createProduct);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении записи о создании продукта: {response.StatusCode}");
            }
        }

        public async Task DeleteCreateProductAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/createproduct/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении записи о создании продукта: {response.StatusCode}");
            }
        }
        #endregion
    }
}