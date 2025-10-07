import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api', // backend URL
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Add response interceptor for better error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      // The request was made and the server responded with a status code
      // that falls out of the range of 2xx
      if (error.response.status !== 404) {
        // Only log non-404 errors to reduce console noise during development
        console.error('API Error:', error.response.status, error.response.data);
      }
    } else if (error.request) {
      // The request was made but no response was received
      console.error('Network Error: Unable to reach server');
    } else {
      // Something happened in setting up the request that triggered an Error
      console.error('Error:', error.message);
    }
    return Promise.reject(error);
  }
);

export default api;