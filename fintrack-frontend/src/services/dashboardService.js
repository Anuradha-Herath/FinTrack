import api from './api';

export const dashboardService = {
  // Get dashboard summary (total income, expenses, balance)
  getSummary: async () => {
    try {
      const response = await api.get('/reports/summary');
      return response.data;
    } catch (error) {
      console.warn('Summary endpoint not available, will use mock data');
      return null;
    }
  },

  // Get income vs expenses by month
  getIncomeExpenseSummary: async () => {
    try {
      const response = await api.get('/reports/income-expense-summary');
      return response.data;
    } catch (error) {
      console.warn('Income/Expense summary endpoint not available, will use mock data');
      return null;
    }
  },

  // Get expense breakdown by category
  getCategoryExpenseSummary: async () => {
    try {
      const response = await api.get('/reports/transactions-by-category');
      return response.data;
    } catch (error) {
      console.warn('Category summary endpoint not available, will use mock data');
      return null;
    }
  },

  // Get recent transactions
  getRecentTransactions: async (limit = 10) => {
    try {
      const response = await api.get(`/transactions?limit=${limit}`);
      return response.data;
    } catch (error) {
      console.warn('Recent transactions endpoint not available, will use mock data');
      return null;
    }
  },

  // Get user profile info
  getUserProfile: async () => {
    try {
      const response = await api.get('/auth/profile');
      return response.data;
    } catch (error) {
      console.warn('User profile endpoint not available');
      throw error;
    }
  }
};
