# 💰 Budget Management Module - Complete Documentation

## 📋 Overview

The Budget Management module allows users to create, track, and manage spending budgets for different categories. It provides real-time progress tracking, alerts for over-budget scenarios, and comprehensive filtering and sorting capabilities.

---

## 🎯 Features

### ✅ Core Features
- ✨ Create monthly budgets for different spending categories
- 📊 Real-time progress tracking with visual indicators
- 🎨 Color-coded status (On Track, Warning, Over Budget)
- ✏️ Edit budget limits
- 🗑️ Delete budgets
- 📈 Automatic calculation of spent amounts from transactions
- 🔍 Filter by month, year, and status
- 🔄 Sort by category, progress, limit, spent amount, or date
- 📱 Responsive design for all devices

### 🎨 UI Components
1. **Budget Cards**: Visual representation of each budget with progress bars
2. **Budget Modal**: Form for creating and editing budgets
3. **Summary Cards**: Overview statistics (total budgets, limits, spent, alerts)
4. **Filters**: Month, year, and status filtering
5. **Sort Options**: Multiple sorting criteria

---

## 🏗️ Architecture

### Backend (.NET Core Web API)

#### Models

**Budget.cs**
```csharp
public class Budget
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Category { get; set; }     // e.g., "Food", "Transport"
    public decimal LimitAmount { get; set; }  // Budget limit
    public int Month { get; set; }            // 1-12
    public int Year { get; set; }             // e.g., 2025
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

#### DTOs

**BudgetDto.cs** (for requests)
- Category (required, max 50 chars)
- LimitAmount (required, > 0)
- Month (required, 1-12)
- Year (required, 2000-2100)

**BudgetResponseDto.cs** (for responses)
- All Budget properties
- SpentAmount (calculated from transactions)
- RemainingAmount (LimitAmount - SpentAmount)
- ProgressPercentage ((SpentAmount / LimitAmount) * 100)
- Status ("on-track", "warning", "over-budget")

#### API Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/budgets` | Get all budgets for logged-in user | ✅ |
| GET | `/api/budgets/{id}` | Get specific budget | ✅ |
| POST | `/api/budgets` | Create new budget | ✅ |
| PUT | `/api/budgets/{id}` | Update budget | ✅ |
| DELETE | `/api/budgets/{id}` | Delete budget | ✅ |

#### Repository Pattern

**IBudgetRepository.cs**
- `GetAllByUserIdAsync(int userId)` - Get all user budgets
- `GetByIdAsync(int id, int userId)` - Get specific budget
- `AddAsync(Budget budget)` - Create budget
- `UpdateAsync(Budget budget)` - Update budget
- `DeleteAsync(int id, int userId)` - Delete budget
- `GetSpentAmountAsync(userId, category, month, year)` - Calculate spent amount
- `BudgetExistsAsync(userId, category, month, year)` - Check for duplicates

#### Service Layer

**BudgetService.cs**
- Implements business logic
- Calculates progress, remaining amount, and status
- Validates duplicate budgets
- Maps entities to DTOs

#### Security
- All endpoints protected with `[Authorize]` attribute
- UserId extracted from JWT claims
- User can only access their own budgets

---

### Frontend (React + Tailwind CSS)

#### Components

**1. Budgets.jsx** (Main Page)
- Budget list management
- Filter and sort functionality
- Summary statistics
- Modal management
- API integration

**2. BudgetCard.jsx**
- Visual budget representation
- Progress bar with color coding
- Category icons
- Edit/Delete actions

**3. BudgetModal.jsx**
- Form for create/edit operations
- Input validation
- Category dropdown
- Month/Year selectors
- Loading states

**4. LoadingSpinner.jsx**
- Loading indicator

#### State Management
```javascript
const [budgets, setBudgets] = useState([]);          // All budgets
const [filteredBudgets, setFilteredBudgets] = useState([]); // Filtered budgets
const [isModalOpen, setIsModalOpen] = useState(false);
const [selectedBudget, setSelectedBudget] = useState(null);
const [filterMonth, setFilterMonth] = useState('all');
const [filterYear, setFilterYear] = useState('all');
const [filterStatus, setFilterStatus] = useState('all');
const [sortBy, setSortBy] = useState('category');
```

#### Services

**budgetService.js**
```javascript
import api from './api';

export const budgetService = {
  getAll: async () => { /* ... */ },
  getById: async (id) => { /* ... */ },
  create: async (budget) => { /* ... */ },
  update: async (id, budget) => { /* ... */ },
  delete: async (id) => { /* ... */ },
};
```

---

## 🗄️ Database Schema

```sql
CREATE TABLE Budgets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    LimitAmount DECIMAL(18,2) NOT NULL,
    Month INT NOT NULL CHECK (Month >= 1 AND Month <= 12),
    Year INT NOT NULL CHECK (Year >= 2000 AND Year <= 2100),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    
    CONSTRAINT FK_Budgets_Users FOREIGN KEY (UserId) 
        REFERENCES Users(UserId) ON DELETE CASCADE,
    
    CONSTRAINT UQ_Budget_User_Category_Month_Year 
        UNIQUE (UserId, Category, Month, Year)
);
```

### Indexes
- `IX_Budgets_UserId` - For user queries
- `IX_Budgets_Month_Year` - For date filtering
- `IX_Budgets_Category` - For category filtering

---

## 🎨 Color Coding & Status

### Progress Status
- **On Track** (< 80%): 🟢 Green
- **Warning** (80-100%): 🟡 Yellow
- **Over Budget** (> 100%): 🔴 Red

### Status Calculation
```csharp
string status;
if (progressPercentage >= 100)
    status = "over-budget";
else if (progressPercentage >= 80)
    status = "warning";
else
    status = "on-track";
```

---

## 📝 Usage Examples

### Creating a Budget

**Frontend:**
```javascript
const newBudget = {
  category: "Food",
  limitAmount: 5000,
  month: 10,
  year: 2025
};

await budgetService.create(newBudget);
```

**Backend Request:**
```http
POST /api/budgets
Authorization: Bearer <token>
Content-Type: application/json

{
  "category": "Food",
  "limitAmount": 5000,
  "month": 10,
  "year": 2025
}
```

**Backend Response:**
```json
{
  "id": 1,
  "userId": 1,
  "category": "Food",
  "limitAmount": 5000.00,
  "month": 10,
  "year": 2025,
  "spentAmount": 3200.00,
  "remainingAmount": 1800.00,
  "progressPercentage": 64.00,
  "status": "on-track",
  "createdAt": "2025-10-07T10:00:00Z",
  "updatedAt": null
}
```

### Updating a Budget

**Frontend:**
```javascript
const updatedBudget = {
  category: "Food",
  limitAmount: 6000, // Increased limit
  month: 10,
  year: 2025
};

await budgetService.update(budgetId, updatedBudget);
```

### Deleting a Budget

**Frontend:**
```javascript
await budgetService.delete(budgetId);
```

---

## 🔧 Configuration

### Categories
Predefined categories in `BudgetModal.jsx`:
```javascript
const CATEGORIES = [
  'Salary', 'Food', 'Transport', 'Entertainment',
  'Shopping', 'Healthcare', 'Education', 'Bills',
  'Rent', 'Insurance', 'Investment', 'Other'
];
```

### API Base URL
Configured in `api.js`:
```javascript
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
});
```

---

## 🧪 Testing

### Manual Testing Checklist

#### Create Budget
- [ ] Can create budget with valid data
- [ ] Validation works for empty fields
- [ ] Validation works for invalid amounts (≤ 0)
- [ ] Cannot create duplicate budget (same category, month, year)
- [ ] Success message displays
- [ ] Budget list refreshes

#### Read Budgets
- [ ] All budgets load on page load
- [ ] Spent amount calculates correctly from transactions
- [ ] Progress bar displays correctly
- [ ] Status color coding works
- [ ] Summary cards show correct totals

#### Update Budget
- [ ] Can edit existing budget
- [ ] Form pre-fills with current data
- [ ] Validation works on update
- [ ] Cannot update to duplicate (different budget with same details)
- [ ] Changes reflect immediately
- [ ] Success message displays

#### Delete Budget
- [ ] Confirmation dialog appears
- [ ] Budget deletes successfully
- [ ] List updates after deletion
- [ ] Success message displays

#### Filters
- [ ] Month filter works
- [ ] Year filter works
- [ ] Status filter works
- [ ] Combining filters works
- [ ] "All" option shows all budgets

#### Sorting
- [ ] Sort by category works (alphabetical)
- [ ] Sort by progress works (highest first)
- [ ] Sort by limit works (highest first)
- [ ] Sort by spent works (highest first)
- [ ] Sort by date works (newest first)

#### Error Handling
- [ ] Network errors display message
- [ ] 404 errors handled gracefully
- [ ] Duplicate budget errors show specific message
- [ ] Invalid data errors display

---

## 🚀 Quick Start

### Backend Setup

1. **Update Database:**
   ```bash
   cd FinTrack.API
   dotnet ef migrations add UpdateBudgetModel
   dotnet ef database update
   ```

2. **Run API:**
   ```bash
   dotnet run
   ```

### Frontend Setup

1. **Install Dependencies** (if needed):
   ```bash
   cd fintrack-frontend
   npm install
   ```

2. **Run Development Server:**
   ```bash
   npm run dev
   ```

3. **Access Application:**
   - Open browser: `http://localhost:5173`
   - Login with valid credentials
   - Navigate to "Budgets" from sidebar

---

## 🎯 Future Enhancements

### Possible Features
- 📊 Budget analytics and charts
- 📧 Email alerts for over-budget scenarios
- 📱 Push notifications
- 🔄 Recurring budgets (auto-create for next month)
- 📈 Budget recommendations based on spending patterns
- 💡 Budget goals and targets
- 📅 Multi-month budget view
- 🏷️ Custom category creation
- 📤 Export budgets to CSV/PDF
- 🔔 Weekly/monthly budget reports

---

## 🐛 Known Issues

None currently. Please report issues on GitHub.

---

## 📞 Support

For questions or issues:
- Check documentation
- Review code comments
- Test with sample data
- Check browser console for errors
- Verify API is running and accessible

---

## 📄 License

Part of FinTrack Application - Personal Finance Management System

---

**Created:** October 2025  
**Last Updated:** October 2025  
**Version:** 1.0.0
