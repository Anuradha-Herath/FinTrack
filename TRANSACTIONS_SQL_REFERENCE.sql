-- =============================================
-- FinTrack Transactions Module - SQL Script
-- =============================================
-- This script documents the database structure for the Transactions module
-- The actual migration is handled by Entity Framework Core

-- =============================================
-- 1. CREATE TRANSACTIONS TABLE
-- =============================================

-- Note: This table is created by the migration
-- The structure shown here is for reference only

/*
CREATE TABLE [dbo].[Transactions] (
    [Id]          INT            IDENTITY(1,1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [Type]        NVARCHAR(MAX)  NOT NULL,      -- 'Income' or 'Expense'
    [Category]    NVARCHAR(MAX)  NOT NULL,      -- e.g., 'Salary', 'Food', 'Transport'
    [Amount]      DECIMAL(18,2)  NOT NULL,
    [Date]        DATETIME2      NOT NULL,
    [Description] NVARCHAR(MAX)  NULL,
    [AccountId]   INT            NULL,
    [CategoryId]  INT            NULL,
    
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    
    CONSTRAINT [FK_Transactions_Users_UserId] 
        FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) 
        ON DELETE NO ACTION,
    
    CONSTRAINT [FK_Transactions_Accounts_AccountId] 
        FOREIGN KEY ([AccountId]) 
        REFERENCES [dbo].[Accounts] ([Id]) 
        ON DELETE SET NULL,
    
    CONSTRAINT [FK_Transactions_Categories_CategoryId] 
        FOREIGN KEY ([CategoryId]) 
        REFERENCES [dbo].[Categories] ([Id])
);

-- Create index for better query performance
CREATE NONCLUSTERED INDEX [IX_Transactions_UserId] 
    ON [dbo].[Transactions]([UserId] ASC);
*/

-- =============================================
-- 2. SAMPLE QUERIES
-- =============================================

-- Get all transactions for a specific user
SELECT * FROM Transactions 
WHERE UserId = @userId 
ORDER BY Date DESC;

-- Get transactions with filters
SELECT * FROM Transactions 
WHERE UserId = @userId 
    AND (@type IS NULL OR Type = @type)
    AND (@category IS NULL OR Category = @category)
    AND (@startDate IS NULL OR Date >= @startDate)
    AND (@endDate IS NULL OR Date <= @endDate)
ORDER BY Date DESC;

-- Get transaction summary for a user
SELECT 
    SUM(CASE WHEN Type = 'Income' THEN Amount ELSE 0 END) AS TotalIncome,
    SUM(CASE WHEN Type = 'Expense' THEN Amount ELSE 0 END) AS TotalExpense,
    SUM(CASE WHEN Type = 'Income' THEN Amount ELSE 0 END) - 
    SUM(CASE WHEN Type = 'Expense' THEN Amount ELSE 0 END) AS Balance
FROM Transactions
WHERE UserId = @userId;

-- Get transactions by category (for reports)
SELECT 
    Category,
    Type,
    COUNT(*) AS TransactionCount,
    SUM(Amount) AS TotalAmount
FROM Transactions
WHERE UserId = @userId
GROUP BY Category, Type
ORDER BY TotalAmount DESC;

-- Get transactions by month (for trends)
SELECT 
    YEAR(Date) AS Year,
    MONTH(Date) AS Month,
    Type,
    SUM(Amount) AS TotalAmount
FROM Transactions
WHERE UserId = @userId
GROUP BY YEAR(Date), MONTH(Date), Type
ORDER BY Year DESC, Month DESC, Type;

-- Get recent transactions (for dashboard)
SELECT TOP 10 
    Id,
    Type,
    Category,
    Amount,
    Date,
    Description
FROM Transactions
WHERE UserId = @userId
ORDER BY Date DESC, Id DESC;

-- =============================================
-- 3. SAMPLE TEST DATA
-- =============================================

-- Note: Replace @userId with actual user ID after login

/*
-- Insert sample income transactions
INSERT INTO Transactions (UserId, Type, Category, Amount, Date, Description)
VALUES 
    (@userId, 'Income', 'Salary', 50000.00, GETDATE(), 'Monthly salary'),
    (@userId, 'Income', 'Freelance', 15000.00, GETDATE(), 'Web development project'),
    (@userId, 'Income', 'Investment', 5000.00, GETDATE(), 'Stock dividends'),
    (@userId, 'Income', 'Business', 20000.00, GETDATE(), 'Product sales'),
    (@userId, 'Income', 'Gift', 3000.00, GETDATE(), 'Birthday gift');

-- Insert sample expense transactions
INSERT INTO Transactions (UserId, Type, Category, Amount, Date, Description)
VALUES 
    (@userId, 'Expense', 'Food', 1200.00, GETDATE(), 'Restaurant lunch'),
    (@userId, 'Expense', 'Transport', 500.00, GETDATE(), 'Taxi fare'),
    (@userId, 'Expense', 'Shopping', 2500.00, GETDATE(), 'Clothing purchase'),
    (@userId, 'Expense', 'Bills', 3000.00, GETDATE(), 'Electricity bill'),
    (@userId, 'Expense', 'Entertainment', 1000.00, GETDATE(), 'Movie tickets'),
    (@userId, 'Expense', 'Healthcare', 2000.00, GETDATE(), 'Doctor visit'),
    (@userId, 'Expense', 'Education', 5000.00, GETDATE(), 'Online course'),
    (@userId, 'Expense', 'Food', 800.00, GETDATE(), 'Grocery shopping');
*/

-- =============================================
-- 4. USEFUL MAINTENANCE QUERIES
-- =============================================

-- Count transactions by type
SELECT Type, COUNT(*) AS Count
FROM Transactions
WHERE UserId = @userId
GROUP BY Type;

-- Find duplicate transactions (same amount, date, category)
SELECT Amount, Date, Category, COUNT(*) AS DuplicateCount
FROM Transactions
WHERE UserId = @userId
GROUP BY Amount, Date, Category
HAVING COUNT(*) > 1;

-- Get transactions with no description
SELECT * FROM Transactions
WHERE UserId = @userId AND Description IS NULL;

-- Get high-value transactions (over 10,000)
SELECT * FROM Transactions
WHERE UserId = @userId AND Amount > 10000
ORDER BY Amount DESC;

-- Get transactions from last 30 days
SELECT * FROM Transactions
WHERE UserId = @userId 
    AND Date >= DATEADD(day, -30, GETDATE())
ORDER BY Date DESC;

-- Get transactions by date range
SELECT * FROM Transactions
WHERE UserId = @userId 
    AND Date BETWEEN '2025-10-01' AND '2025-10-31'
ORDER BY Date DESC;

-- =============================================
-- 5. PERFORMANCE OPTIMIZATION
-- =============================================

-- Additional indexes for better performance (optional)

/*
-- Index for date filtering
CREATE NONCLUSTERED INDEX [IX_Transactions_Date] 
    ON [dbo].[Transactions]([Date] DESC);

-- Composite index for common filters
CREATE NONCLUSTERED INDEX [IX_Transactions_UserId_Type_Date] 
    ON [dbo].[Transactions]([UserId], [Type], [Date] DESC);

-- Index for category filtering
CREATE NONCLUSTERED INDEX [IX_Transactions_Category] 
    ON [dbo].[Transactions]([Category]);
*/

-- =============================================
-- 6. DATA VALIDATION CHECKS
-- =============================================

-- Check for invalid transaction types
SELECT * FROM Transactions
WHERE Type NOT IN ('Income', 'Expense');

-- Check for negative amounts
SELECT * FROM Transactions
WHERE Amount < 0;

-- Check for future dates
SELECT * FROM Transactions
WHERE Date > GETDATE();

-- Check for orphaned transactions (user doesn't exist)
SELECT t.* 
FROM Transactions t
LEFT JOIN Users u ON t.UserId = u.Id
WHERE u.Id IS NULL;

-- =============================================
-- 7. REPORTING QUERIES
-- =============================================

-- Monthly income vs expense comparison
SELECT 
    FORMAT(Date, 'yyyy-MM') AS Month,
    SUM(CASE WHEN Type = 'Income' THEN Amount ELSE 0 END) AS Income,
    SUM(CASE WHEN Type = 'Expense' THEN Amount ELSE 0 END) AS Expense,
    SUM(CASE WHEN Type = 'Income' THEN Amount ELSE -Amount END) AS NetSavings
FROM Transactions
WHERE UserId = @userId
GROUP BY FORMAT(Date, 'yyyy-MM')
ORDER BY Month DESC;

-- Top spending categories
SELECT TOP 10
    Category,
    COUNT(*) AS TransactionCount,
    SUM(Amount) AS TotalAmount,
    AVG(Amount) AS AverageAmount
FROM Transactions
WHERE UserId = @userId AND Type = 'Expense'
GROUP BY Category
ORDER BY TotalAmount DESC;

-- Top income sources
SELECT TOP 10
    Category,
    COUNT(*) AS TransactionCount,
    SUM(Amount) AS TotalAmount,
    AVG(Amount) AS AverageAmount
FROM Transactions
WHERE UserId = @userId AND Type = 'Income'
GROUP BY Category
ORDER BY TotalAmount DESC;

-- Daily spending pattern
SELECT 
    DATENAME(weekday, Date) AS DayOfWeek,
    AVG(Amount) AS AverageExpense
FROM Transactions
WHERE UserId = @userId AND Type = 'Expense'
GROUP BY DATENAME(weekday, Date), DATEPART(weekday, Date)
ORDER BY DATEPART(weekday, Date);

-- =============================================
-- 8. CLEANUP QUERIES (USE WITH CAUTION)
-- =============================================

/*
-- Delete all transactions for a specific user (BE CAREFUL!)
DELETE FROM Transactions WHERE UserId = @userId;

-- Delete transactions older than 1 year
DELETE FROM Transactions 
WHERE UserId = @userId 
    AND Date < DATEADD(year, -1, GETDATE());

-- Delete transactions with zero amount
DELETE FROM Transactions 
WHERE UserId = @userId AND Amount = 0;
*/

-- =============================================
-- END OF SQL SCRIPT
-- =============================================

-- Notes:
-- 1. All queries use parameterized @userId for security
-- 2. Use Entity Framework Core for CRUD operations in production
-- 3. These queries are for reference, testing, and manual operations only
-- 4. Always backup database before running DELETE queries
-- 5. The actual table is managed by EF Core migrations
