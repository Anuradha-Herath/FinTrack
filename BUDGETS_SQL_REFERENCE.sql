-- =============================================
-- Budget Management SQL Script
-- FinTrack Application
-- =============================================

-- Drop existing Budgets table if you need to recreate it
-- WARNING: This will delete all budget data!
-- DROP TABLE IF EXISTS Budgets;

-- Create Budgets table
CREATE TABLE Budgets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    LimitAmount DECIMAL(18,2) NOT NULL,
    Month INT NOT NULL CHECK (Month >= 1 AND Month <= 12),
    Year INT NOT NULL CHECK (Year >= 2000 AND Year <= 2100),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    
    -- Foreign key constraint
    CONSTRAINT FK_Budgets_Users FOREIGN KEY (UserId) 
        REFERENCES Users(UserId) ON DELETE CASCADE,
    
    -- Unique constraint to prevent duplicate budgets for same category/month/year
    CONSTRAINT UQ_Budget_User_Category_Month_Year 
        UNIQUE (UserId, Category, Month, Year)
);

-- Create indexes for better query performance
CREATE INDEX IX_Budgets_UserId ON Budgets(UserId);
CREATE INDEX IX_Budgets_Month_Year ON Budgets(Month, Year);
CREATE INDEX IX_Budgets_Category ON Budgets(Category);

-- =============================================
-- Sample Data (Optional - for testing)
-- =============================================

-- Insert sample budgets for UserId = 1
-- Replace with actual UserId from your Users table
/*
INSERT INTO Budgets (UserId, Category, LimitAmount, Month, Year, CreatedAt)
VALUES 
    (1, 'Food', 5000.00, 10, 2025, GETUTCDATE()),
    (1, 'Transport', 3000.00, 10, 2025, GETUTCDATE()),
    (1, 'Entertainment', 2000.00, 10, 2025, GETUTCDATE()),
    (1, 'Shopping', 4000.00, 10, 2025, GETUTCDATE()),
    (1, 'Healthcare', 1500.00, 10, 2025, GETUTCDATE());
*/

-- =============================================
-- Useful Queries
-- =============================================

-- Get all budgets for a user
-- SELECT * FROM Budgets WHERE UserId = 1 ORDER BY Year DESC, Month DESC, Category;

-- Get budgets with spending data (join with Transactions)
/*
SELECT 
    b.Id,
    b.UserId,
    b.Category,
    b.LimitAmount,
    b.Month,
    b.Year,
    ISNULL(SUM(CASE WHEN t.Type = 'Expense' THEN t.Amount ELSE 0 END), 0) AS SpentAmount,
    b.LimitAmount - ISNULL(SUM(CASE WHEN t.Type = 'Expense' THEN t.Amount ELSE 0 END), 0) AS RemainingAmount,
    CASE 
        WHEN b.LimitAmount > 0 THEN 
            ROUND((ISNULL(SUM(CASE WHEN t.Type = 'Expense' THEN t.Amount ELSE 0 END), 0) / b.LimitAmount) * 100, 2)
        ELSE 0 
    END AS ProgressPercentage
FROM Budgets b
LEFT JOIN Transactions t ON 
    t.UserId = b.UserId 
    AND t.Category = b.Category 
    AND t.Type = 'Expense'
    AND MONTH(t.Date) = b.Month 
    AND YEAR(t.Date) = b.Year
WHERE b.UserId = 1
GROUP BY b.Id, b.UserId, b.Category, b.LimitAmount, b.Month, b.Year
ORDER BY b.Year DESC, b.Month DESC, b.Category;
*/

-- Get budgets that are over budget
/*
SELECT 
    b.Id,
    b.Category,
    b.LimitAmount,
    SUM(t.Amount) AS SpentAmount,
    SUM(t.Amount) - b.LimitAmount AS OverAmount,
    b.Month,
    b.Year
FROM Budgets b
INNER JOIN Transactions t ON 
    t.UserId = b.UserId 
    AND t.Category = b.Category 
    AND t.Type = 'Expense'
    AND MONTH(t.Date) = b.Month 
    AND YEAR(t.Date) = b.Year
WHERE b.UserId = 1
GROUP BY b.Id, b.Category, b.LimitAmount, b.Month, b.Year
HAVING SUM(t.Amount) > b.LimitAmount;
*/

-- Update a budget limit
-- UPDATE Budgets SET LimitAmount = 6000.00, UpdatedAt = GETUTCDATE() WHERE Id = 1;

-- Delete a budget
-- DELETE FROM Budgets WHERE Id = 1;

-- Get budget statistics for a user
/*
SELECT 
    COUNT(*) AS TotalBudgets,
    SUM(LimitAmount) AS TotalBudgetLimit,
    AVG(LimitAmount) AS AvgBudgetLimit,
    MIN(LimitAmount) AS MinBudgetLimit,
    MAX(LimitAmount) AS MaxBudgetLimit
FROM Budgets
WHERE UserId = 1;
*/

-- =============================================
-- Maintenance Queries
-- =============================================

-- Check for duplicate budgets (should return 0 rows if constraint is working)
/*
SELECT UserId, Category, Month, Year, COUNT(*) AS DuplicateCount
FROM Budgets
GROUP BY UserId, Category, Month, Year
HAVING COUNT(*) > 1;
*/

-- View all budgets with user information
/*
SELECT 
    b.Id,
    u.Email AS UserEmail,
    u.Name AS UserName,
    b.Category,
    b.LimitAmount,
    b.Month,
    b.Year,
    b.CreatedAt,
    b.UpdatedAt
FROM Budgets b
INNER JOIN Users u ON b.UserId = u.UserId
ORDER BY b.CreatedAt DESC;
*/
