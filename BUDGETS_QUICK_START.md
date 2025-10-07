# 🚀 Budgets Module - Quick Start Guide

## ⚡ Quick Setup (5 Minutes)

### 1️⃣ Backend Setup

#### Stop the Running API (if running)
Press `Ctrl+C` in the terminal where the API is running.

#### Run Database Migration
```bash
cd FinTrack.API
dotnet ef migrations add UpdateBudgetModelForBudgets
dotnet ef database update
```

#### Restart the API
```bash
dotnet run
```

The API should now be running on `http://localhost:5000`

---

### 2️⃣ Frontend Setup

The frontend code is already in place! Just ensure the dev server is running:

```bash
cd fintrack-frontend
npm run dev
```

The app should be running on `http://localhost:5173`

---

### 3️⃣ Test the Budgets Module

1. **Login** to your account
2. **Navigate** to "Budgets" from the sidebar
3. **Click** "Add Budget" button
4. **Fill in** the form:
   - Category: Select "Food"
   - Limit Amount: 5000
   - Month: October
   - Year: 2025
5. **Click** "Create Budget"
6. ✅ Your first budget is created!

---

## 📚 Common Operations

### Create a Budget
```javascript
POST /api/budgets
{
  "category": "Food",
  "limitAmount": 5000,
  "month": 10,
  "year": 2025
}
```

### Get All Budgets
```javascript
GET /api/budgets
```

### Update a Budget
```javascript
PUT /api/budgets/1
{
  "category": "Food",
  "limitAmount": 6000,
  "month": 10,
  "year": 2025
}
```

### Delete a Budget
```javascript
DELETE /api/budgets/1
```

---

## 🎨 UI Features

### Budget Card Shows:
- 📊 Progress bar (color-coded)
- 💰 Budget limit
- 💸 Amount spent
- 💵 Remaining amount
- 🎯 Progress percentage
- ⚠️ Status badge (On Track / Warning / Over Budget)

### Filters Available:
- 📅 Month
- 🗓️ Year  
- 🎯 Status (On Track / Warning / Over Budget)

### Sort Options:
- 🔤 Category
- 📊 Progress
- 💰 Budget Limit
- 💸 Amount Spent
- 📅 Date

---

## 💡 Tips

### Budget Status Colors:
- 🟢 **Green** (On Track): < 80% spent
- 🟡 **Yellow** (Warning): 80-100% spent
- 🔴 **Red** (Over Budget): > 100% spent

### Duplicate Prevention:
You cannot create two budgets for the same category in the same month/year. The system will prevent this.

### Automatic Calculations:
- Spent amount is calculated from your transactions automatically
- Progress percentage updates in real-time
- Remaining amount is calculated dynamically

---

## 🐛 Troubleshooting

### Issue: "Failed to fetch budgets"
**Solution:** Ensure the backend API is running on `http://localhost:5000`

### Issue: "Migration failed"
**Solution:** Stop the running API first, then run the migration

### Issue: "Duplicate budget error"
**Solution:** You already have a budget for that category in that month/year. Edit the existing one or choose a different category/month.

### Issue: "Token expired"
**Solution:** Login again to get a fresh token

---

## 📊 Sample Data

Want to see budgets in action? Create these sample budgets:

```javascript
// Food Budget
Category: Food
Limit: $5,000
Month: October
Year: 2025

// Transport Budget
Category: Transport
Limit: $3,000
Month: October
Year: 2025

// Entertainment Budget
Category: Entertainment
Limit: $2,000
Month: October
Year: 2025
```

Then add some expense transactions in those categories to see the progress bars in action!

---

## 🔗 Related Features

- **Transactions Module**: Add expenses to see budget progress
- **Dashboard**: View budget overview
- **Reports**: Analyze budget vs. actual spending

---

## ✅ Checklist

- [ ] Backend API is running
- [ ] Frontend dev server is running
- [ ] Database migration applied
- [ ] Can access login page
- [ ] Can login successfully
- [ ] Can see Budgets page
- [ ] Can create a budget
- [ ] Can edit a budget
- [ ] Can delete a budget
- [ ] Can filter budgets
- [ ] Can sort budgets

---

## 🎯 Next Steps

1. ✅ Create budgets for your main spending categories
2. 📝 Add transactions to see budget progress
3. 📊 Use filters to view specific budgets
4. 🎨 Sort by progress to see which budgets need attention
5. ⚠️ Monitor alerts for over-budget scenarios

---

## 📞 Need Help?

- Check the full documentation: `BUDGETS_MODULE_DOCUMENTATION.md`
- Review SQL reference: `BUDGETS_SQL_REFERENCE.sql`
- Check browser console for errors (F12)
- Verify API logs for backend issues

**Happy Budgeting! 💰📊**
