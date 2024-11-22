

using LoadDWVentas.Data.Context;
using LoadDWVentas.Data.Entities.DwVentas;
using LoadDWVentas.Data.Interfaces;
using LoadDWVentas.Data.Result;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace LoadDWVentas.Data.Services
{
    public class DataServiceDwVentas : IDataServiceDwVentas
    {
        private readonly NorwindContext _norwindContext;
        private readonly DbSalesContext _salesContext;

        public DataServiceDwVentas(NorwindContext norwindContext,
                                   DbSalesContext salesContext)
        {
            _norwindContext = norwindContext; 
            _salesContext = salesContext;
        }

        public async Task<OperactionResult> LoadDHW()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                // await LoadDimEmployee();
                // await LoadDimProductCategory();
                // await LoadDimCustomers();
                // await LoadDimShippers();
                //await LoadFactSales();
                //await LoadFactCustomerServed();

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el DWH Ventas. {ex.Message}";
            }

            return result;
        }

        private async Task<OperactionResult> LoadDimEmployee()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                //Obtener los empleados de la base de datos de norwind.
                var employees = await _norwindContext.Employees.AsNoTracking().Select(emp => new DimEmployee()
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeName = string.Concat(emp.FirstName, " ", emp.LastName)
                }).ToListAsync();




                // Carga la dimension de empleados.

                await _salesContext.DimEmployees.AddRangeAsync(employees);

                await _salesContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando la dimension de empleado {ex.Message}";
            }


            return result;
        }

        private async Task<OperactionResult> LoadDimProductCategory()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                // Obtener las products categories de norwind //

                var productCategories = await (from product in _norwindContext.Products
                                               join category in _norwindContext.Categories on product.CategoryId equals category.CategoryId
                                               select new DimProductCategory()
                                               {
                                                   CategoryId = category.CategoryId,
                                                   ProductName = product.ProductName,
                                                   CategoryName = category.CategoryName,
                                                   ProductId = product.ProductId
                                               }).AsNoTracking().ToListAsync();


                // Carga la dimension de Products Categories.

                await _salesContext.DimProductCategories.AddRangeAsync(productCategories);
                await _salesContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de producto y categoria. {ex.Message}";
            }
            return result;
        }

        private async Task<OperactionResult> LoadDimCustomers()
        {
            OperactionResult operaction = new OperactionResult() { Success = false };


            try
            {
                // Obtener clientes de norwind

                var customers = await _norwindContext.Customers.Select(cust => new DimCustomer()
                {
                    CustomerId = cust.CustomerId,
                    CustomerName = cust.CompanyName

                }).AsNoTracking()
                  .ToListAsync();

                // Carga dimension de cliente.

                await _salesContext.DimCustomers.AddRangeAsync(customers);
                await _salesContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error: {ex.Message} cargando la dimension de clientes.";
            }
            return operaction;
        }

        private async Task<OperactionResult> LoadDimShippers()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var shippers = await _norwindContext.Shippers.Select(ship => new DimShipper()
                {
                    ShipperId = ship.ShipperID,
                    ShipperName = ship.CompanyName
                }).ToListAsync();


                await _salesContext.DimShippers.AddRangeAsync(shippers);
                await _salesContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando la dimension de shippers { ex.Message } ";
            }
            return result;
        }

        private async Task<OperactionResult> LoadFactSales() 
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var ventas = await _norwindContext.Vwventas.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de ventas {ex.Message} ";
            }

            return result;
        }


        private async Task<OperactionResult> LoadFactCustomerServed()
        {
            OperactionResult result = new OperactionResult() { Success = true };

            try
            {
                var customerServed = await _norwindContext.VwServedCustomers.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de clientes atendidos {ex.Message} ";
            }
            return result;
        }
    }
}
