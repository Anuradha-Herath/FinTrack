import api from './api';

export const transactionService = {
  getAll: async () => {
    const response = await api.get('/transactions');
    return response.data;
  },
  create: async (transaction) => {
    const response = await api.post('/transactions', transaction);
    return response.data;
  },
  update: async (id, transaction) => {
    const response = await api.put(`/transactions/${id}`, transaction);
    return response.data;
  },
  delete: async (id) => {
    await api.delete(`/transactions/${id}`);
  },
};