-- =========================================================================
-- Stored Procedure: sp_GetRevenueByCashier
-- Mục đích: Kết xuất báo cáo doanh thu ca làm việc theo thu ngân và khoảng thời gian (TC34)
-- =========================================================================

CREATE OR ALTER PROCEDURE sp_GetRevenueByCashier
    @CashierUsername NVARCHAR(100) = NULL,
    @FromDate DATETIME2 = NULL,
    @ToDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        COALESCE(NULLIF(@CashierUsername, ''), N'Tất cả') AS CashierUsername,
        (SELECT TOP 1 FullName FROM Users WHERE (@CashierUsername IS NOT NULL AND @CashierUsername <> '' AND Username = @CashierUsername)) AS CashierFullName,
        COUNT(o.OrderId) AS TotalOrders,
        ISNULL(SUM(o.TotalAmount), 0) AS TotalRevenue,
        @FromDate AS FromDate,
        @ToDate AS ToDate
    FROM Orders o
    LEFT JOIN Users u ON o.UserId = u.UserId
    WHERE (@CashierUsername IS NULL OR @CashierUsername = '' OR u.Username = @CashierUsername)
      AND (@FromDate IS NULL OR o.OrderDate >= @FromDate)
      AND (@ToDate IS NULL OR o.OrderDate <= @ToDate)
      AND (o.Status <> 'Cancelled');
END;
GO
