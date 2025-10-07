import api from './api';

export const budgetService = {
  // Get all budgets for logged-in user
  getAll: async () => {
    const response = await api.get('/budgets');
    return response.data;
  },

  // Get budget by ID
  getById: async (id) => {
    const response = await api.get(`/budgets/${id}`);
    return response.data;
  },

  // Create new budget
  create: async (budget) => {
    const response = await api.post('/budgets', budget);
    return response.data;
  },

  // Update budget
  update: async (id, budget) => {
    const response = await api.put(`/budgets/${id}`, budget);
    return response.data;
  },

  // Delete budget
  delete: async (id) => {
    await api.delete(`/budgets/${id}`);
  },
};

export default budgetService;