import { useState, useEffect } from 'react';
import Sidebar from '../components/Sidebar';
import BudgetCard from '../components/BudgetCard';
import BudgetModal from '../components/BudgetModal';
import LoadingSpinner from '../components/LoadingSpinner';
import { budgetService } from '../services/budgetService';

const Budgets = () => {
  const [budgets, setBudgets] = useState([]);
  const [filteredBudgets, setFilteredBudgets] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedBudget, setSelectedBudget] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState('');
  
  // Filter states
  const [filterMonth, setFilterMonth] = useState('all');
  const [filterYear, setFilterYear] = useState('all');
  const [filterStatus, setFilterStatus] = useState('all');
  const [sortBy, setSortBy] = useState('category');

  useEffect(() => {
    fetchBudgets();
  }, []);

  useEffect(() => {
    applyFiltersAndSort();
  }, [budgets, filterMonth, filterYear, filterStatus, sortBy]);

  const fetchBudgets = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const data = await budgetService.getAll();
      setBudgets(data);
    } catch (err) {
      console.error('Error fetching budgets:', err);
      setError(err.response?.data?.message || 'Failed to fetch budgets. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const applyFiltersAndSort = () => {
    let filtered = [...budgets];

    // Apply filters
    if (filterMonth !== 'all') {
      filtered = filtered.filter(b => b.month === parseInt(filterMonth));
    }
    if (filterYear !== 'all') {
      filtered = filtered.filter(b => b.year === parseInt(filterYear));
    }
    if (filterStatus !== 'all') {
      filtered = filtered.filter(b => b.status === filterStatus);
    }

    // Apply sorting
    filtered.sort((a, b) => {
      switch (sortBy) {
        case 'category':
          return a.category.localeCompare(b.category);
        case 'progress':
          return b.progressPercentage - a.progressPercentage;
        case 'limit':
          return b.limitAmount - a.limitAmount;
        case 'spent':
          return b.spentAmount - a.spentAmount;
        case 'date':
          if (a.year !== b.year) return b.year - a.year;
          return b.month - a.month;
        default:
          return 0;
      }
    });

    setFilteredBudgets(filtered);
  };

  const handleAddBudget = () => {
    setSelectedBudget(null);
    setIsModalOpen(true);
  };

  const handleEditBudget = (budget) => {
    setSelectedBudget(budget);
    setIsModalOpen(true);
  };

  const handleDeleteBudget = async (budget) => {
    if (!window.confirm(`Are you sure you want to delete the ${budget.category} budget for ${budget.month}/${budget.year}?`)) {
      return;
    }

    try {
      await budgetService.delete(budget.id);
      showSuccess(`Budget deleted successfully!`);
      fetchBudgets();
    } catch (err) {
      console.error('Error deleting budget:', err);
      setError(err.response?.data?.message || 'Failed to delete budget. Please try again.');
    }
  };

  const handleSubmitBudget = async (budgetData) => {
    try {
      setIsSubmitting(true);
      setError(null);

      if (selectedBudget) {
        await budgetService.update(selectedBudget.id, budgetData);
        showSuccess('Budget updated successfully!');
      } else {
        await budgetService.create(budgetData);
        showSuccess('Budget created successfully!');
      }

      setIsModalOpen(false);
      setSelectedBudget(null);
      fetchBudgets();
    } catch (err) {
      console.error('Error saving budget:', err);
      setError(err.response?.data?.message || 'Failed to save budget. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedBudget(null);
  };

  const showSuccess = (message) => {
    setSuccessMessage(message);
    setTimeout(() => setSuccessMessage(''), 3000);
  };

  // Get unique years from budgets
  const getUniqueYears = () => {
    const years = [...new Set(budgets.map(b => b.year))];
    return years.sort((a, b) => b - a);
  };

  // Calculate summary statistics
  const calculateSummary = () => {
    const totalLimit = filteredBudgets.reduce((sum, b) => sum + b.limitAmount, 0);
    const totalSpent = filteredBudgets.reduce((sum, b) => sum + b.spentAmount, 0);
    const overBudgetCount = filteredBudgets.filter(b => b.status === 'over-budget').length;
    const warningCount = filteredBudgets.filter(b => b.status === 'warning').length;

    return { totalLimit, totalSpent, overBudgetCount, warningCount };
  };

  const summary = calculateSummary();

  if (isLoading) {
    return (
      <div className="flex min-h-screen bg-gray-50">
        <Sidebar />
        <div className="flex-1 ml-64 flex items-center justify-center">
          <LoadingSpinner />
        </div>
      </div>
    );
  }

  return (
    <div className="flex min-h-screen bg-gray-50">
      <Sidebar />
      <div className="flex-1 ml-64 p-8">
        {/* Header */}
        <div className="mb-8">
          <div className="flex justify-between items-center mb-4">
            <div>
              <h1 className="text-3xl font-bold text-gray-800">Budget Management</h1>
              <p className="text-gray-600 mt-1">Track and manage your spending budgets</p>
            </div>
            <button
              onClick={handleAddBudget}
              className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2 font-medium shadow-md"
            >
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
              </svg>
              Add Budget
            </button>
          </div>

          {/* Success Message */}
          {successMessage && (
            <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded-lg mb-4 flex items-center">
              <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
              {successMessage}
            </div>
          )}

          {/* Error Message */}
          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4 flex items-center justify-between">
              <div className="flex items-center">
                <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
                {error}
              </div>
              <button onClick={() => setError(null)} className="text-red-700 hover:text-red-900">
                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          )}
        </div>

        {/* Summary Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-200">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-600 text-sm">Total Budgets</p>
                <p className="text-2xl font-bold text-gray-800 mt-1">{filteredBudgets.length}</p>
              </div>
              <div className="bg-blue-100 rounded-full p-3">
                <svg className="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                </svg>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-200">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-600 text-sm">Total Limit</p>
                <p className="text-2xl font-bold text-gray-800 mt-1">${summary.totalLimit.toFixed(2)}</p>
              </div>
              <div className="bg-green-100 rounded-full p-3">
                <svg className="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-200">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-600 text-sm">Total Spent</p>
                <p className="text-2xl font-bold text-gray-800 mt-1">${summary.totalSpent.toFixed(2)}</p>
              </div>
              <div className="bg-purple-100 rounded-full p-3">
                <svg className="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
                </svg>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border border-gray-200">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-600 text-sm">Alerts</p>
                <p className="text-2xl font-bold text-gray-800 mt-1">
                  {summary.overBudgetCount + summary.warningCount}
                </p>
                <p className="text-xs text-gray-500 mt-1">
                  {summary.overBudgetCount} over, {summary.warningCount} warning
                </p>
              </div>
              <div className="bg-red-100 rounded-full p-3">
                <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                </svg>
              </div>
            </div>
          </div>
        </div>

        {/* Filters and Sort */}
        <div className="bg-white rounded-lg shadow-md p-6 mb-6 border border-gray-200">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Month</label>
              <select
                value={filterMonth}
                onChange={(e) => setFilterMonth(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="all">All Months</option>
                {[...Array(12)].map((_, i) => (
                  <option key={i + 1} value={i + 1}>
                    {new Date(2000, i).toLocaleString('default', { month: 'long' })}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Year</label>
              <select
                value={filterYear}
                onChange={(e) => setFilterYear(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="all">All Years</option>
                {getUniqueYears().map(year => (
                  <option key={year} value={year}>{year}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Status</label>
              <select
                value={filterStatus}
                onChange={(e) => setFilterStatus(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="all">All Status</option>
                <option value="on-track">On Track</option>
                <option value="warning">Warning</option>
                <option value="over-budget">Over Budget</option>
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Sort By</label>
              <select
                value={sortBy}
                onChange={(e) => setSortBy(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="category">Category</option>
                <option value="progress">Progress</option>
                <option value="limit">Budget Limit</option>
                <option value="spent">Amount Spent</option>
                <option value="date">Date (Newest)</option>
              </select>
            </div>
          </div>
        </div>

        {/* Budget Cards */}
        {filteredBudgets.length === 0 ? (
          <div className="bg-white rounded-lg shadow-md p-12 text-center border border-gray-200">
            <svg className="w-16 h-16 text-gray-400 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
            <h3 className="text-xl font-semibold text-gray-800 mb-2">No Budgets Found</h3>
            <p className="text-gray-600 mb-6">
              {budgets.length === 0
                ? "Get started by creating your first budget!"
                : "No budgets match your current filters."}
            </p>
            {budgets.length === 0 && (
              <button
                onClick={handleAddBudget}
                className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors inline-flex items-center gap-2"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                </svg>
                Create Your First Budget
              </button>
            )}
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredBudgets.map((budget) => (
              <BudgetCard
                key={budget.id}
                budget={budget}
                onEdit={handleEditBudget}
                onDelete={handleDeleteBudget}
              />
            ))}
          </div>
        )}
      </div>

      {/* Budget Modal */}
      <BudgetModal
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        onSubmit={handleSubmitBudget}
        budget={selectedBudget}
        isLoading={isSubmitting}
      />
    </div>
  );
};

export default Budgets;
