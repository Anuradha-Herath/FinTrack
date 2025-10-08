import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

const CATEGORIES = [
  'Salary',
  'Food',
  'Transport',
  'Entertainment',
  'Shopping',
  'Healthcare',
  'Education',
  'Bills',
  'Rent',
  'Insurance',
  'Investment',
  'Other',
];

const MONTHS = [
  { value: 1, label: 'January' },
  { value: 2, label: 'February' },
  { value: 3, label: 'March' },
  { value: 4, label: 'April' },
  { value: 5, label: 'May' },
  { value: 6, label: 'June' },
  { value: 7, label: 'July' },
  { value: 8, label: 'August' },
  { value: 9, label: 'September' },
  { value: 10, label: 'October' },
  { value: 11, label: 'November' },
  { value: 12, label: 'December' },
];

const BudgetModal = ({ isOpen, onClose, onSubmit, budget, isLoading }) => {
  const currentYear = new Date().getFullYear();
  const currentMonth = new Date().getMonth() + 1;
  
  const [formData, setFormData] = useState({
    category: '',
    limitAmount: '',
    month: currentMonth,
    year: currentYear,
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (budget) {
      setFormData({
        category: budget.category || '',
        limitAmount: budget.limitAmount || '',
        month: budget.month || currentMonth,
        year: budget.year || currentYear,
      });
    } else {
      setFormData({
        category: '',
        limitAmount: '',
        month: currentMonth,
        year: currentYear,
      });
    }
    setErrors({});
  }, [budget, isOpen, currentMonth, currentYear]);

  const validateForm = () => {
    const newErrors = {};

    if (!formData.category.trim()) {
      newErrors.category = 'Category is required';
    }

    if (!formData.limitAmount) {
      newErrors.limitAmount = 'Limit amount is required';
    } else if (parseFloat(formData.limitAmount) <= 0) {
      newErrors.limitAmount = 'Limit amount must be greater than 0';
    }

    if (!formData.month || formData.month < 1 || formData.month > 12) {
      newErrors.month = 'Valid month is required';
    }

    if (!formData.year || formData.year < 2000) {
      newErrors.year = 'Valid year is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    if (validateForm()) {
      onSubmit({
        ...formData,
        limitAmount: parseFloat(formData.limitAmount),
        month: parseInt(formData.month),
        year: parseInt(formData.year),
      });
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors((prev) => ({
        ...prev,
        [name]: '',
      }));
    }
  };

  if (!isOpen) return null;

  // Generate years (current year + 2 years back and forward)
  const years = [];
  for (let i = currentYear - 2; i <= currentYear + 2; i++) {
    years.push(i);
  }

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg shadow-xl w-full max-w-md max-h-[90vh] overflow-y-auto">
        <div className="p-6">
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-2xl font-bold text-gray-800">
              {budget ? 'Edit Budget' : 'Add New Budget'}
            </h2>
            <button
              onClick={onClose}
              className="text-gray-400 hover:text-gray-600 transition-colors"
              disabled={isLoading}
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            {/* Category */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Category <span className="text-red-500">*</span>
              </label>
              <select
                name="category"
                value={formData.category}
                onChange={handleChange}
                disabled={isLoading}
                className={`w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                  errors.category ? 'border-red-500' : 'border-gray-300'
                } disabled:bg-gray-100 disabled:cursor-not-allowed`}
              >
                <option value="">Select Category</option>
                {CATEGORIES.map((category) => (
                  <option key={category} value={category}>
                    {category}
                  </option>
                ))}
              </select>
              {errors.category && (
                <p className="mt-1 text-sm text-red-500">{errors.category}</p>
              )}
            </div>

            {/* Limit Amount */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Budget Limit <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <span className="absolute left-3 top-2 text-gray-500">$</span>
                <input
                  type="number"
                  name="limitAmount"
                  value={formData.limitAmount}
                  onChange={handleChange}
                  disabled={isLoading}
                  step="0.01"
                  min="0.01"
                  placeholder="0.00"
                  className={`w-full pl-8 pr-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                    errors.limitAmount ? 'border-red-500' : 'border-gray-300'
                  } disabled:bg-gray-100 disabled:cursor-not-allowed`}
                />
              </div>
              {errors.limitAmount && (
                <p className="mt-1 text-sm text-red-500">{errors.limitAmount}</p>
              )}
            </div>

            {/* Month & Year */}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Month <span className="text-red-500">*</span>
                </label>
                <select
                  name="month"
                  value={formData.month}
                  onChange={handleChange}
                  disabled={isLoading}
                  className={`w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                    errors.month ? 'border-red-500' : 'border-gray-300'
                  } disabled:bg-gray-100 disabled:cursor-not-allowed`}
                >
                  {MONTHS.map((month) => (
                    <option key={month.value} value={month.value}>
                      {month.label}
                    </option>
                  ))}
                </select>
                {errors.month && (
                  <p className="mt-1 text-sm text-red-500">{errors.month}</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Year <span className="text-red-500">*</span>
                </label>
                <select
                  name="year"
                  value={formData.year}
                  onChange={handleChange}
                  disabled={isLoading}
                  className={`w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                    errors.year ? 'border-red-500' : 'border-gray-300'
                  } disabled:bg-gray-100 disabled:cursor-not-allowed`}
                >
                  {years.map((year) => (
                    <option key={year} value={year}>
                      {year}
                    </option>
                  ))}
                </select>
                {errors.year && (
                  <p className="mt-1 text-sm text-red-500">{errors.year}</p>
                )}
              </div>
            </div>

            {/* Action Buttons */}
            <div className="flex gap-3 pt-4">
              <button
                type="button"
                onClick={onClose}
                disabled={isLoading}
                className="flex-1 px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Cancel
              </button>
              <button
                type="submit"
                disabled={isLoading}
                className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
              >
                {isLoading ? (
                  <>
                    <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    {budget ? 'Updating...' : 'Creating...'}
                  </>
                ) : (
                  budget ? 'Update Budget' : 'Create Budget'
                )}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

BudgetModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  budget: PropTypes.object,
  isLoading: PropTypes.bool,
};

export default BudgetModal;
