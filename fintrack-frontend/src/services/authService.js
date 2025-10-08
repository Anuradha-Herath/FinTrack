import api from './api';

export const authService = {
  login: async (credentials) => {
    try {
      const response = await api.post('/auth/login', credentials);
      return response.data;
    } catch (error) {
      // Return the error data instead of throwing to avoid console errors
      return error.response?.data || { success: false, message: 'Login failed' };
    }
  },
  register: async (userData) => {
    try {
      const response = await api.post('/auth/register', userData);
      return response.data;
    } catch (error) {
      // Return the error data instead of throwing to avoid console errors
      return error.response?.data || { success: false, message: 'Registration failed' };
    }
  },
};