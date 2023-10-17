-- SQL Stored Procedure (pr_GetOrderSummary) to return a summary of orders from the data in the Northwind database
/*
Assume we are using Azure SQL Database
    Instructions
    1. Create a new Database called Northwind
    2. Run the script below 
        https://github.com/microsoft/sql-server-samples/blob/master/samples/databases/northwind-pubs/instnwnd%20(Azure%20SQL%20Database).sql
    3. Run the Script below to create the pr_GetOrderSummary stored proceedure
    4. Use test data provided below the script to test the Stored Proceedure
*/
USE Northwind
CREATE PROCEDURE pr_GetOrderSummary
    @StartDate DATE = '1900-01-19',
    @EndDate DATE = '2030-01-19',
    @EmployeeID INT = NULL,
    @CustomerID NVARCHAR(5) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        e.TitleOfCourtesy + ' ' + e.FirstName + ' ' + e.LastName AS EmployeeFullName,
        s.CompanyName AS ShipperCompanyName,
        c.CompanyName AS CustomerCompanyName,
        COUNT(o.OrderID) AS NumberOfOrders,
        CONVERT(DATE, o.OrderDate) AS [Date],
        SUM(o.Freight) AS TotalFreightCost,
        COUNT(DISTINCT od.ProductID) AS NumberOfDifferentProducts,
        SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalOrderValue
    FROM Orders o
    LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
    JOIN Employees e ON o.EmployeeID = e.EmployeeID
    JOIN Shippers s ON o.ShipVia = s.ShipperID
    JOIN [Order Details] od ON o.OrderID = od.OrderID
    WHERE
        (@StartDate IS NULL OR o.OrderDate >= @StartDate) AND
        (@EndDate IS NULL OR o.OrderDate <= @EndDate) AND
        (@EmployeeID IS NULL OR o.EmployeeID = @EmployeeID) AND
        (@CustomerID IS NULL OR o.CustomerID = @CustomerID)
    GROUP BY
        CONVERT(DATE, o.OrderDate),
        e.TitleOfCourtesy,
        e.FirstName,
        e.LastName,
        s.CompanyName,
        c.CompanyName;

    SET NOCOUNT OFF;
END;



-- exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=NULL , @CustomerID=NULL

-- exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=5 , @CustomerID=NULL

-- exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=NULL , @CustomerID='VINET'

-- exec pr_GetOrderSummary @StartDate='1 Jan 1996', @EndDate='31 Aug 1996', @EmployeeID=5 , @CustomerID='VINET'