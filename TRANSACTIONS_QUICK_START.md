# 🚀 Transactions Module - Quick Start Guide

## ✅ Prerequisites
- Backend running on `http://localhost:5000`
- Frontend running on `http://localhost:5173`
- User account created and logged in

---

## 📝 Step-by-Step Testing Guide

### 1️⃣ Access the Transactions Page
1. Open your browser to `http://localhost:5173`
2. Log in with your credentials
3. Click on "Transactions" in the sidebar
4. You should see:
   - Summary cards (Total Income, Total Expense, Balance)
   - Filter bar
   - Transaction table
   - "Add Transaction" button

---

### 2️⃣ Add Your First Income Transaction

1. Click the **"+ Add Transaction"** button
2. Fill in the form:
   - **Type**: Select "Income"
   - **Category**: Select "Salary"
   - **Amount**: Enter `50000`
   - **Date**: Select today's date
   - **Description**: Type "Monthly Salary"
3. Click **"Add Transaction"**
4. ✅ You should see:
   - Green success toast notification
   - New transaction in the table with green "Income" badge
   - Total Income updated to Rs. 50,000
   - Balance updated to Rs. 50,000

---

### 3️⃣ Add Your First Expense Transaction

1. Click **"+ Add Transaction"** again
2. Fill in the form:
   - **Type**: Select "Expense"
   - **Category**: Select "Food"
   - **Amount**: Enter `1200`
   - **Date**: Select today's date
   - **Description**: Type "Lunch at restaurant"
3. Click **"Add Transaction"**
4. ✅ You should see:
   - Green success toast notification
   - New transaction with red "Expense" badge
   - Total Expense updated to Rs. 1,200
   - Balance updated to Rs. 48,800

---

### 4️⃣ Test Filtering

**Filter by Type:**
1. In the filter bar, select **Type**: "Expense"
2. ✅ Only expense transactions should be visible

**Filter by Date Range:**
1. Set **Start Date**: First day of this month
2. Set **End Date**: Last day of this month
3. ✅ Only transactions in this month should appear

**Search:**
1. In the Search box, type "lunch"
2. ✅ Only the lunch transaction should be visible

**Clear Filters:**
1. Click **"Clear Filters"**
2. ✅ All transactions should be visible again

---

### 5️⃣ Edit a Transaction

1. Click **"Edit"** on any transaction
2. Change the **Amount** to a different value
3. Click **"Update Transaction"**
4. ✅ You should see:
   - Success toast notification
   - Updated amount in the table
   - Summary cards updated accordingly

---

### 6️⃣ Delete a Transaction

1. Click **"Delete"** on any transaction
2. Confirm deletion in the popup
3. ✅ You should see:
   - Success toast notification
   - Transaction removed from the list
   - Summary cards updated

---

### 7️⃣ Test Pagination

1. Add 15+ transactions (mix of income and expense)
2. ✅ You should see:
   - Pagination controls at the bottom
   - "Page 1 of 2" indicator
   - "Previous" and "Next" buttons
3. Click **"Next"**
4. ✅ Second page of transactions should load

---

## 🎨 Expected UI Elements

### Summary Cards
```
┌─────────────────────────────────────────────────────────────────┐
│  💰 Total Income      │  💸 Total Expense   │  📊 Balance       │
│  Rs. 50,000           │  Rs. 1,200          │  Rs. 48,800       │
│  (Green)              │  (Red)              │  (Green/Red)      │
└─────────────────────────────────────────────────────────────────┘
```

### Filter Bar
```
┌─────────────────────────────────────────────────────────────────┐
│  [Type ▼] [Category ▼] [Start Date] [End Date] [Search...]     │
│  Clear Filters                          [+ Add Transaction]     │
└─────────────────────────────────────────────────────────────────┘
```

### Transaction Table
```
┌───────────────────────────────────────────────────────────────────┐
│ Date       │ Type    │ Category │ Description    │ Amount │ Actions│
├───────────────────────────────────────────────────────────────────┤
│ 10/07/2025 │ Income  │ Salary   │ Monthly Salary │ +Rs.50K│ Edit X │
│ 10/07/2025 │ Expense │ Food     │ Lunch          │ -Rs.1.2K│ Edit X│
└───────────────────────────────────────────────────────────────────┘
```

---

## 🔍 Testing API Endpoints (Optional)

You can also test the backend directly using tools like Postman or curl:

### Get All Transactions
```bash
GET http://localhost:5000/api/transactions
Headers: Authorization: Bearer YOUR_JWT_TOKEN
```

### Get Summary
```bash
GET http://localhost:5000/api/transactions/summary
Headers: Authorization: Bearer YOUR_JWT_TOKEN
```

### Create Transaction
```bash
POST http://localhost:5000/api/transactions
Headers: 
  Content-Type: application/json
  Authorization: Bearer YOUR_JWT_TOKEN
Body:
{
  "type": "Expense",
  "category": "Food",
  "amount": 1200,
  "date": "2025-10-07",
  "description": "Lunch"
}
```

### Filter Transactions
```bash
GET http://localhost:5000/api/transactions?type=Expense&category=Food&startDate=2025-10-01&endDate=2025-10-31
Headers: Authorization: Bearer YOUR_JWT_TOKEN
```

---

## ✅ Success Criteria

Your implementation is working correctly if:
- ✅ You can add both income and expense transactions
- ✅ Transactions appear in the table immediately after creation
- ✅ Summary cards update in real-time
- ✅ Filters work correctly (type, category, date range, search)
- ✅ Edit functionality updates transactions
- ✅ Delete removes transactions with confirmation
- ✅ Pagination works for 10+ transactions
- ✅ Color coding is correct (green for income, red for expense)
- ✅ Toast notifications appear for all actions
- ✅ Loading spinner shows while fetching data
- ✅ Only your own transactions are visible (user isolation)

---

## 🐛 Troubleshooting

### Problem: "Failed to load transactions"
**Solution:**
1. Check backend is running on `http://localhost:5000`
2. Check JWT token is valid (try logging in again)
3. Check browser console for error messages

### Problem: "Failed to save transaction"
**Solution:**
1. Ensure all required fields are filled
2. Check Type is exactly "Income" or "Expense"
3. Verify Amount is a positive number
4. Check network tab for detailed error message

### Problem: No transactions showing
**Solution:**
1. Check if filters are active (click "Clear Filters")
2. Verify you're logged in
3. Check if you have any transactions (add one to test)

### Problem: Summary cards showing 0
**Solution:**
1. Add at least one transaction
2. Refresh the page
3. Check browser console for errors

---

## 📊 Sample Test Data

Use these to quickly populate your transactions:

**Income Transactions:**
```
1. Salary - Rs. 50,000 - Monthly salary
2. Freelance - Rs. 15,000 - Web development project
3. Investment - Rs. 5,000 - Stock dividends
```

**Expense Transactions:**
```
1. Food - Rs. 1,200 - Restaurant lunch
2. Transport - Rs. 500 - Taxi fare
3. Bills - Rs. 3,000 - Electricity bill
4. Shopping - Rs. 2,500 - Clothing
5. Entertainment - Rs. 1,000 - Movie tickets
```

---

## 🎯 Next Steps

After testing the Transactions module:
1. Test with different user accounts (user isolation)
2. Test with 50+ transactions (performance)
3. Test on different screen sizes (responsiveness)
4. Test filter combinations
5. Test edge cases (very large amounts, old dates, etc.)

---

## 💡 Tips

- Use **Ctrl+Shift+C** to open browser DevTools for debugging
- Check the **Network** tab to see API requests/responses
- Check the **Console** tab for any JavaScript errors
- Use **React DevTools** extension to inspect component state

---

**Happy Testing! 🎉**

Need help? Refer to `TRANSACTIONS_MODULE_DOCUMENTATION.md` for detailed technical documentation.
