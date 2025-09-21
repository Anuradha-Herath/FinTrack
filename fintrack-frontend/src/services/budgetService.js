import api from './api';

export const budgetService = {
  getAll: async () => {
    const response = await api.get('/budgets');
    return response.data;
  },
  create: async (budget) => {
    const response = await api.post('/budgets', budget);
    return response.data;
  },
  update: async (id, budget) => {
    const response = await api.put(`/budgets/${id}`, budget);
    return response.data;
  },
  delete: async (id) => {
    await api.delete(`/budgets/${id}`);
  },
};