# FinTrack Dashboard Documentation

## 📊 Overview

The Dashboard is the main overview page that users see after logging into FinTrack. It provides a comprehensive financial summary with interactive charts, recent transactions, and quick action buttons.

## 🎯 Features Implemented

### 1. **Header Section**
- **Welcome Message**: Displays personalized greeting with user's name
- **Current Date**: Shows current date in readable format (e.g., "Monday, October 7, 2025")
- **Total Balance**: Prominent display of user's net balance
- **Dark Mode Toggle**: Switch between light and dark themes

### 2. **Summary Cards**
Three key financial metrics displayed as cards:
- **💰 Total Income**: Shows total income with green color scheme
- **💸 Total Expenses**: Shows total expenses with red color scheme
- **💼 Net Balance**: Shows net balance (Income - Expenses) with blue color scheme

Each card features:
- Icon representation
- Large, readable amount display
- Optional trend indicators (percentage change from previous period)
- Hover effects and shadows

### 3. **Charts Section**

#### Income vs Expenses Bar Chart
- **Library**: Recharts
- **Type**: Bar Chart
- **Data**: Monthly comparison of income vs expenses
- **Features**:
  - X-axis: Months (Jan-Jun)
  - Y-axis: Amount in dollars
  - Green bars for income
  - Red bars for expenses
  - Interactive tooltips
  - Grid lines for easy reading
  - Responsive design

#### Expense Breakdown Pie Chart
- **Library**: Recharts
- **Type**: Pie Chart
- **Data**: Expenses categorized by type
- **Categories**:
  - Food & Dining
  - Transportation
  - Shopping
  - Bills & Utilities
  - Entertainment
  - Others
- **Features**:
  - Color-coded segments
  - Percentage labels
  - Interactive tooltips
  - Responsive design

### 4. **Recent Transactions Table**
- Displays latest 10 transactions
- **Columns**:
  - Date (formatted as "Oct 06")
  - Type (Income/Expense badge)
  - Category
  - Description
  - Amount (color-coded: green for income, red for expenses)
- **Features**:
  - Sortable columns
  - Hover effects
  - "View All" button to navigate to full transactions page
  - Empty state message when no transactions exist

### 5. **Quick Actions Section**
Four gradient action buttons for quick navigation:
- **➕ Add Transaction**: Navigate to transactions page
- **💰 Create Budget**: Navigate to budgets page
- **🎯 Set Goal**: Navigate to goals page
- **📊 View Reports**: Navigate to reports page

### 6. **Sidebar Navigation**
- Fixed left sidebar
- Menu items:
  - Dashboard
  - Transactions
  - Budgets
  - Goals
  - Reports
  - Profile
- Active route highlighting
- Logout button at bottom

## 🔧 Technical Implementation

### Components Created

1. **Dashboard.jsx** (`src/pages/Dashboard.jsx`)
   - Main dashboard component
   - Handles data fetching and state management
   - Implements responsive layout

2. **Sidebar.jsx** (`src/components/Sidebar.jsx`)
   - Navigation component
   - Route highlighting
   - Logout functionality

3. **SummaryCard.jsx** (`src/components/SummaryCard.jsx`)
   - Reusable card component for summary metrics
   - Customizable colors and icons
   - Trend indicators

4. **LoadingSpinner.jsx** (`src/components/LoadingSpinner.jsx`)
   - Loading state component
   - Customizable size and message

### Services Created

**dashboardService.js** (`src/services/dashboardService.js`)
- `getSummary()`: Fetch financial summary
- `getIncomeExpenseSummary()`: Fetch monthly income/expense data
- `getCategoryExpenseSummary()`: Fetch category breakdown
- `getRecentTransactions(limit)`: Fetch recent transactions
- `getUserProfile()`: Fetch user profile information

## 🌐 API Integration

### Expected Backend Endpoints

```javascript
// Financial Summary
GET /api/reports/summary
Response: {
  totalIncome: 15000,
  totalExpenses: 8500,
  netBalance: 6500
}

// Income vs Expenses by Month
GET /api/reports/income-expense-summary
Response: [
  { month: "Jan", income: 5000, expenses: 3200 },
  { month: "Feb", income: 5500, expenses: 2800 },
  ...
]

// Expense Breakdown by Category
GET /api/reports/transactions-by-category
Response: [
  { name: "Food & Dining", value: 2500, percentage: 29.4 },
  { name: "Transportation", value: 1800, percentage: 21.2 },
  ...
]

// Recent Transactions
GET /api/transactions?limit=10
Response: [
  {
    id: 1,
    date: "2025-10-06",
    type: "Expense",
    category: "Food & Dining",
    description: "Grocery shopping",
    amount: -125.50
  },
  ...
]

// User Profile
GET /api/auth/profile
Response: {
  name: "John Doe",
  email: "john@example.com",
  ...
}
```

### Authentication
All API requests include the JWT token in the Authorization header:
```javascript
headers: {
  Authorization: `Bearer ${token}`
}
```

## 📦 Dependencies

```json
{
  "recharts": "^2.x.x",      // For charts
  "react-router-dom": "^7.x", // For navigation
  "@reduxjs/toolkit": "^2.x", // State management
  "react-redux": "^9.x",      // Redux integration
  "axios": "^1.x",            // HTTP client
  "tailwindcss": "^4.x"       // Styling
}
```

## 🎨 Styling

### Color Scheme
- **Primary**: Blue (#3B82F6, #2563EB)
- **Success/Income**: Green (#10B981, #059669)
- **Danger/Expense**: Red (#EF4444, #DC2626)
- **Warning**: Orange (#F59E0B)
- **Purple**: Purple (#8B5CF6)
- **Gray**: Various shades for text and backgrounds

### Responsive Design
- **Mobile**: Single column layout
- **Tablet**: Two-column grid for cards and charts
- **Desktop**: Full multi-column layout with fixed sidebar

### Dark Mode
- Toggle button in header
- Dark background (#1F2937)
- Light text on dark backgrounds
- Adjusted colors for better contrast

## 🔄 Data Flow

1. **Component Mount**:
   - Check authentication status
   - Redirect to login if not authenticated

2. **Data Fetching**:
   - Fetch all dashboard data in parallel using `Promise.all()`
   - Display loading spinner while fetching
   - Show error message if fetch fails

3. **Fallback to Mock Data**:
   - If API endpoints are not ready, use mock data
   - Ensures UI can be tested independently

4. **State Management**:
   - Local component state for dashboard data
   - Redux for authentication state
   - LocalStorage for token persistence

## 🚀 Usage

### Starting the Dashboard

```bash
# Navigate to frontend directory
cd fintrack-frontend

# Install dependencies (if not already done)
npm install

# Start development server
npm run dev
```

### Accessing the Dashboard

1. Navigate to `/login`
2. Enter credentials
3. After successful login, automatically redirected to `/dashboard`

### Navigation

- Use sidebar to navigate between different sections
- Click "View All" in transactions to see full transaction list
- Use quick action buttons for common tasks
- Click dark mode toggle to switch themes

## 🧪 Testing

### Manual Testing Checklist

- [ ] Dashboard loads after login
- [ ] Summary cards display correct data
- [ ] Bar chart renders properly
- [ ] Pie chart renders properly
- [ ] Recent transactions table displays data
- [ ] Sidebar navigation works
- [ ] Dark mode toggle works
- [ ] Quick action buttons navigate correctly
- [ ] Loading spinner appears while fetching data
- [ ] Error message displays on API failure
- [ ] Responsive design works on mobile
- [ ] Responsive design works on tablet
- [ ] Logout functionality works

### Mock Data Testing

The dashboard includes comprehensive mock data that allows full UI testing even if the backend is not ready. Mock data is automatically used if API calls fail.

## 🔐 Security

- JWT token stored in localStorage
- Token included in all API requests
- Automatic redirect to login if not authenticated
- Token removed on logout

## ⚡ Performance Optimizations

- Parallel data fetching using `Promise.all()`
- Lazy loading of chart libraries
- Responsive images and icons
- Minimal re-renders with proper state management
- CSS animations using Tailwind for better performance

## 🐛 Error Handling

- Try-catch blocks for all API calls
- Fallback to mock data if API fails
- User-friendly error messages
- Console logging for debugging
- Loading states to prevent user confusion

## 🔮 Future Enhancements

1. **Real-time Updates**: WebSocket integration for live data
2. **Export Functionality**: Export dashboard data to PDF/CSV
3. **Customizable Dashboard**: Drag-and-drop widget arrangement
4. **More Chart Types**: Line charts, area charts, etc.
5. **Date Range Filters**: Filter data by custom date ranges
6. **Comparison Views**: Compare current vs previous periods
7. **Notifications**: Alert users to important financial events
8. **Budget Progress Bars**: Visual budget tracking
9. **Goal Progress Indicators**: Track progress toward financial goals
10. **Multi-currency Support**: Support for different currencies

## 📝 Notes

### Backend Integration

To fully integrate with your backend:

1. Update the base URL in `src/services/api.js` if different from `http://localhost:5000/api`
2. Ensure your backend returns data in the expected format
3. Implement the required endpoints in your ReportsController
4. Add authentication middleware to protect dashboard endpoints

### Code Structure

```
src/
├── components/
│   ├── Sidebar.jsx          # Navigation sidebar
│   ├── SummaryCard.jsx      # Summary metric card
│   └── LoadingSpinner.jsx   # Loading indicator
├── pages/
│   ├── Dashboard.jsx        # Main dashboard
│   ├── auth/
│   │   ├── Login.jsx
│   │   └── SignUp.jsx
│   ├── Transactions.jsx
│   ├── Budgets.jsx
│   ├── Goals.jsx
│   ├── Reports.jsx
│   └── Profile.jsx
└── services/
    ├── api.js               # Axios instance with interceptors
    ├── authService.js       # Authentication services
    └── dashboardService.js  # Dashboard data services
```

## 🎓 Code Explanation

### Key Sections Explained

#### 1. Authentication Check
```javascript
useEffect(() => {
  const token = localStorage.getItem('token');
  if (!token && !isAuthenticated) {
    navigate('/login');
  }
}, [isAuthenticated, navigate]);
```
Ensures user is authenticated before showing dashboard.

#### 2. Parallel Data Fetching
```javascript
const [summaryData, incomeExpenseData, categoryData, transactionsData] = 
  await Promise.all([
    dashboardService.getSummary().catch(() => null),
    dashboardService.getIncomeExpenseSummary().catch(() => null),
    // ... more API calls
  ]);
```
Fetches all data simultaneously for better performance.

#### 3. Fallback to Mock Data
```javascript
if (summaryData) {
  setSummary(summaryData);
} else {
  // Use mock data
  setSummary({ totalIncome: 15000, totalExpenses: 8500, netBalance: 6500 });
}
```
Provides mock data if API is unavailable, allowing UI testing.

#### 4. Recharts Integration
```javascript
<BarChart data={incomeExpenseData}>
  <CartesianGrid strokeDasharray="3 3" />
  <XAxis dataKey="month" />
  <YAxis />
  <Tooltip content={<CustomTooltip />} />
  <Legend />
  <Bar dataKey="income" fill="#10B981" />
  <Bar dataKey="expenses" fill="#EF4444" />
</BarChart>
```
Configures responsive charts with custom styling.

---

## 📧 Support

For questions or issues with the dashboard implementation, please refer to the main project documentation or contact the development team.

**Happy Tracking! 💰📊**
