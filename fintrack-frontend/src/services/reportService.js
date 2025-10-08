import api from './api';

export const reportService = {
  getSummary: async (month, year) => {
    const params = {};
    if (month) params.month = month;
    if (year) params.year = year;
    const response = await api.get('/reports/summary', { params });
    return response.data;
  },
  getExpensesByCategory: async (month, year) => {
    const params = {};
    if (month) params.month = month;
    if (year) params.year = year;
    const response = await api.get('/reports/expenses-by-category', { params });
    return response.data;
  },
  getIncomeVsExpenseTrend: async (year) => {
    const response = await api.get('/reports/income-vs-expense-trend', { params: { year } });
    return response.data;
  },
};