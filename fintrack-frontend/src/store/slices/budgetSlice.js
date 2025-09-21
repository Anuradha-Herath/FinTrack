import { createSlice } from '@reduxjs/toolkit';

const budgetSlice = createSlice({
  name: 'budget',
  initialState: {
    budgets: [],
    loading: false,
  },
  reducers: {
    setBudgets: (state, action) => {
      state.budgets = action.payload;
    },
    addBudget: (state, action) => {
      state.budgets.push(action.payload);
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
  },
});

export const { setBudgets, addBudget, setLoading } = budgetSlice.actions;
export default budgetSlice.reducer;