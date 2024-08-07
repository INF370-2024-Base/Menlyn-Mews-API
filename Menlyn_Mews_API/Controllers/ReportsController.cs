using Humanizer;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _productRepositroy;
       
        public ReportsController(AppDbContext context, IRepositroy productRepositroy)
        {
            _context = context;
            _productRepositroy = productRepositroy;
        }

        [HttpGet]
        [Route("ProductsReport")]
        public async Task<ActionResult<dynamic>> ProductsReport()
        {
            try
            {
                List<dynamic> productsreport = new List<dynamic>();

                var results = await _productRepositroy.GetProductsAsync();

                dynamic productCategory = results
                                        .GroupBy(p => p.ProductCategory.Product_Category_Name)
                                        .Select(c => new
                                        {
                                            Key = c.Key,
                                            ProductCount = c.Count()
                                        });

                dynamic productList = results
                                    .GroupBy(p => new { CategoryName = p.ProductCategory.Product_Category_Name, ProductName = p.Product_Name })
                                    .Select(p => new
                                    {
                                        p.Key.CategoryName,
                                        p.Key.ProductName,
                                    });

                productsreport.Add(productCategory);
                productsreport.Add(productList);

                return productsreport;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("ProductCountReport")]
        public async Task<ActionResult<dynamic>> ProductCountReport()
        {
            try
            {

                var results = await _productRepositroy.GetProductsAsync();

                dynamic products = results.Select(p => new
                {
                    p.Product_Name,
                    p.Quantity_On_Hand,
                    Category_Name = p.ProductCategory.Product_Category_Name,
                    Type_Name = p.ProductType.Product_Type_Name,
                })
                .OrderBy(p => p.Quantity_On_Hand);

                return products;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("SupplierReport")]
        public async Task<ActionResult<dynamic>> SupplierReport()
        {
            try
            {
                List<dynamic> supplierreport = new List<dynamic>();

                var results = await _productRepositroy.GetSuppliersAsync();

                dynamic supplierType = results
                                        .GroupBy(p => p.Supplier_Type.Supplier_Type_Description)
                                        .Select(c => new
                                        {
                                            Key = c.Key,
                                            SupplierCount = c.Count()
                                        });

                dynamic supplierList = results
                                    .GroupBy(p => new { Supplier_Type = p.Supplier_Type.Supplier_Type_Description, Supplier_Name = p.Supplier_Name })
                                    .Select(p => new
                                    {
                                        p.Key.Supplier_Type,
                                        p.Key.Supplier_Name,
                                    });

                supplierreport.Add(supplierType);
                supplierreport.Add(supplierList);

                return supplierreport;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("EventSalesReport")]
        public async Task<ActionResult<dynamic>> EventSalesReport()
        {
            try
            {
                List<dynamic> salesReport = new List<dynamic>();

                var eventBookings = await _productRepositroy.GetEventBookingsAsync();

                var salesByEventType = eventBookings
                    .GroupBy(eb => new { eb.Event_Date.Year, eb.Event_Date.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        MonthYear = $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)} {g.Key.Year}",
                        TotalSales = g.Sum(eb => eb.Event_Price)
                    })
                    .OrderBy(g => g.Year)
                    .ThenBy(g => g.Month);

                // Add the results to the report
                salesReport.Add( salesByEventType );
                return salesReport;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("BookingSalesReport")]
        public async Task<ActionResult<dynamic>> BookingSalesReport()
        {
            try
            {
                List<dynamic> salesReport = new List<dynamic>();

                var bookings = await _productRepositroy.GetRoomBookingsAsync();

                var salesByBookingType = bookings
                    .GroupBy(eb => new { eb.Check_In_Date.Value.Year, eb.Check_In_Date.Value.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        MonthYear = $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)} {g.Key.Year}",
                        TotalSales = g.Sum(eb => eb.Booking_Price)
                    })
                    .OrderBy(g => g.Year)
                    .ThenBy(g => g.Month);

                // Add the results to the report
                salesReport.Add(salesByBookingType);
                return salesReport;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("StockValueItem")]
        public async Task<ActionResult<dynamic>> StockValueItemReport()
        {
            try
            {
                List<dynamic> stockvalue = new List<dynamic>();

                var stockData = await _productRepositroy.GetStockTakesAsync();

                var scatterData = stockData
                    .GroupBy(st => new { st.Stock_Take_Date.Year, st.Stock_Take_Date.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        MonthYear = $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)} {g.Key.Year}",
                        TotalItems = g.Sum(st => st.Total_Items),
                        TotalValue = g.Sum(st => st.Total_Value)
                    })
                    .OrderBy(g => g.Year)
                    .ThenBy(g => g.Month);

                // Add the results to the report
                stockvalue.Add(scatterData);
                return stockvalue;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("RoomsBookedReport")]
        public async Task<ActionResult<dynamic>> RoomsBookedReport()
        {
            try
            {
                List<dynamic> bookings = new List<dynamic>();

                var bookingData = await _productRepositroy.GetRoomBookingsAsync();
                var roomData = await _productRepositroy.GetRoomsAsync();

                var data = bookingData
                    .GroupBy(b => b.Rooms.Room_Number)
                    .Select(b => new
                    {
                        Room_Number = b.Key,
                        Number_Of_Times_Booked = b.Count(),
                    });

                var bookedRoomIds = bookingData.Select(b => b.RoomId).Distinct();
                var unbookedRooms = roomData
                    .Where(r => !bookedRoomIds.Contains(r.RoomId))
                    .Select(r => new
                    {
                        Room_Number = r.Room_Number,
                        Room_Floor = r.Room_Floor,
                        Room_Rate = r.Room_Rate,
                        Room_Description = r.Room_Description
                    });

                bookings.Add(data);
                bookings.Add(unbookedRooms);


                return bookings;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("BookingReviewChart")]
        public async Task<ActionResult<dynamic>> BookingReviewChartController()
        {
            try
            {
                List<dynamic> reviews = new List<dynamic>();
                var reviewData = await _productRepositroy.GetBookingReviewsAsync();

                var graphData = reviewData
                    .GroupBy(br => br.Review_Rating)
                    .Select(br => new
                    {
                        Key = br.Key + " Star Rating",
                        Rating_Count = br.Count(),
                    })
                    .OrderByDescending(br => br.Rating_Count);

                reviews.Add(graphData);
                return reviews;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("BookingReviewReport")]
        public async Task<ActionResult<dynamic>> BookingReviewController()
        {
            try
            {
                var reviewData = await _productRepositroy.GetBookingReviewsAsync();

                dynamic reviews = reviewData.Select(r => new
                {
                    r.Review_Rating,
                    r.Review_Description,
                })
                .OrderByDescending(r => r.Review_Rating);

                return reviews;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("ProductQuantityReport")]
        public async Task<ActionResult<dynamic>> ProductQuantityReport()
        {
            try
            {
                List<dynamic> products = new List<dynamic>();

                var productData = await _productRepositroy.GetProductsAsync();

                var graphData = productData
                    .Select(st => new
                    {
                        ProductName = st.Product_Name,
                        Count = st.Quantity_On_Hand
                    }); ;

                // Add the results to the report
                products.Add(graphData);
                return products;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("EmployeeReport/{gender}")]
        public async Task<ActionResult<dynamic>> EmployeeReport(string gender)
        {
            try
            {
                var normalizedGender = gender.ToUpper();
                List<dynamic> employeeReport = new List<dynamic>();

                var employeeData = await _productRepositroy.GetEmployeesAsync();

                if (normalizedGender == "FEMALE")
                {
                    var graphData = employeeData
                    .Select(st => new
                    {
                        Employee_Full_Name = st.Employee_Name + " " + st.Employee_Surname,
                        Employee_Contact_Number = st.Employee_Contact_Number,
                        Employee_Email = st.Employee_Email_Address,
                        Employee_Gender = st.Employee_Gender,
                    })
                    .Where(st => st.Employee_Gender == "Female");
                    return graphData.ToList();
                }
                else if (normalizedGender == "MALE")
                {
                    var graphData = employeeData
                    .Select(st => new
                    {
                        Employee_Full_Name = st.Employee_Name + " " + st.Employee_Surname,
                        Employee_Contact_Number = st.Employee_Contact_Number,
                        Employee_Email = st.Employee_Email_Address,
                        Employee_Gender = st.Employee_Gender,
                    })
                    .Where(st => st.Employee_Gender == "Male");
                    return graphData.ToList();
                }
                else if (normalizedGender == "OTHER")
                {
                    var graphData = employeeData
                    .Select(st => new
                    {
                        Employee_Full_Name = st.Employee_Name + " " + st.Employee_Surname,
                        Employee_Contact_Number = st.Employee_Contact_Number,
                        Employee_Email = st.Employee_Email_Address,
                        Employee_Gender = st.Employee_Gender,
                    })
                    .Where(st => st.Employee_Gender == "Other");
                    if (graphData == null)
                    {
                        return Ok("Does Not Exist");
                    }
                    else
                    {
                        return graphData.ToList();
                    }
                }
                else
                {
                    var graphData = employeeData
                    .Select(st => new
                    {
                        EmployeeId = st.EmployeeId,
                        Employee_Full_Name = st.Employee_Name + " " + st.Employee_Surname,
                        Employee_Contact_Number = st.Employee_Contact_Number,
                        Employee_Email = st.Employee_Email_Address,
                        Employee_Gender = st.Employee_Gender,
                    });

                    return graphData.ToList();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
        }

    }
}
