import { useState, useEffect } from 'react';
import Sidebar from '../components/Sidebar';
import api from '../services/api';
import LoadingSpinner from '../components/LoadingSpinner';
import { toast } from 'react-hot-toast';

const Goals = () => {
  const [goals, setGoals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showUpdateModal, setShowUpdateModal] = useState(false);
  const [editingGoal, setEditingGoal] = useState(null);
  const [updatingGoal, setUpdatingGoal] = useState(null);

  // Form states
  const [addFormData, setAddFormData] = useState({
    title: '',
    targetAmount: '',
    deadline: '',
    description: ''
  });

  const [updateFormData, setUpdateFormData] = useState({
    addAmount: ''
  });

  useEffect(() => {
    fetchGoals();
  }, []);

  const fetchGoals = async () => {
    try {
      setLoading(true);
      const response = await api.get('/goals');
      setGoals(response.data);
    } catch (error) {
      console.error('Error fetching goals:', error);
      toast.error('Failed to load goals');
    } finally {
      setLoading(false);
    }
  };

  const handleAddGoal = async (e) => {
    e.preventDefault();

    if (!addFormData.title || !addFormData.targetAmount || !addFormData.deadline) {
      toast.error('Please fill in all required fields');
      return;
    }

    try {
      const dataToSend = {
        ...addFormData,
        targetAmount: parseFloat(addFormData.targetAmount),
        currentAmount: 0
      };

      await api.post('/goals', dataToSend);
      toast.success('Goal added successfully');
      resetAddForm();
      fetchGoals();
    } catch (error) {
      console.error('Error adding goal:', error);
      toast.error(error.response?.data?.message || 'Failed to add goal');
    }
  };

  const handleUpdateProgress = async (e) => {
    e.preventDefault();

    if (!updateFormData.addAmount || parseFloat(updateFormData.addAmount) <= 0) {
      toast.error('Please enter a valid amount to add');
      return;
    }

    try {
      const newCurrentAmount = updatingGoal.currentAmount + parseFloat(updateFormData.addAmount);

      await api.put(`/goals/${updatingGoal.id}`, {
        ...updatingGoal,
        currentAmount: newCurrentAmount
      });

      toast.success('Progress updated successfully');
      resetUpdateForm();
      fetchGoals();
    } catch (error) {
      console.error('Error updating progress:', error);
      toast.error(error.response?.data?.message || 'Failed to update progress');
    }
  };

  const handleDeleteGoal = async (id) => {
    if (!window.confirm('Are you sure you want to delete this goal?')) {
      return;
    }

    try {
      await api.delete(`/goals/${id}`);
      toast.success('Goal deleted successfully');
      fetchGoals();
    } catch (error) {
      console.error('Error deleting goal:', error);
      toast.error('Failed to delete goal');
    }
  };

  const resetAddForm = () => {
    setAddFormData({
      title: '',
      targetAmount: '',
      deadline: '',
      description: ''
    });
    setShowAddModal(false);
  };

  const resetUpdateForm = () => {
    setUpdateFormData({
      addAmount: ''
    });
    setUpdatingGoal(null);
    setShowUpdateModal(false);
  };

  const openUpdateModal = (goal) => {
    setUpdatingGoal(goal);
    setUpdateFormData({ addAmount: '' });
    setShowUpdateModal(true);
  };

  const getProgressColor = (percentage) => {
    if (percentage >= 80) return 'bg-green-500';
    if (percentage >= 50) return 'bg-yellow-500';
    return 'bg-red-500';
  };

  const getProgressTextColor = (percentage) => {
    if (percentage >= 80) return 'text-green-600';
    if (percentage >= 50) return 'text-yellow-600';
    return 'text-red-600';
  };

  const isDeadlineNear = (deadline) => {
    const today = new Date();
    const deadlineDate = new Date(deadline);
    const diffTime = deadlineDate - today;
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays <= 7 && diffDays >= 0;
  };

  if (loading) {
    return (
      <div className="flex min-h-screen bg-gray-50">
        <Sidebar />
        <div className="flex-1 ml-64 p-8">
          <LoadingSpinner />
        </div>
      </div>
    );
  }

  return (
    <div className="flex min-h-screen bg-gray-50">
      <Sidebar />
      <div className="flex-1 ml-64 p-8">
        <div className="max-w-6xl mx-auto">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-3xl font-bold text-gray-800 mb-2">Financial Goals</h1>
            <p className="text-gray-600">Track your savings goals and financial targets</p>
          </div>

          {/* Add Goal Button */}
          <div className="mb-6">
            <button
              onClick={() => setShowAddModal(true)}
              className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition flex items-center gap-2 font-semibold"
            >
              <span className="text-xl">+</span>
              Add New Goal
            </button>
          </div>

          {/* Goals Grid */}
          {goals.length === 0 ? (
            <div className="bg-white rounded-lg shadow-md p-12 text-center">
              <div className="text-6xl mb-4">🎯</div>
              <h3 className="text-xl font-semibold text-gray-800 mb-2">No goals yet</h3>
              <p className="text-gray-600 mb-6">Start your financial journey by setting your first goal!</p>
              <button
                onClick={() => setShowAddModal(true)}
                className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition"
              >
                Create Your First Goal
              </button>
            </div>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {goals.map((goal) => {
                const progress = goal.progressPercentage;
                const isNearDeadline = isDeadlineNear(goal.deadline);

                return (
                  <div key={goal.id} className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition">
                    <div className="flex justify-between items-start mb-4">
                      <h3 className="text-lg font-semibold text-gray-800 truncate" title={goal.title}>
                        {goal.title}
                      </h3>
                      <button
                        onClick={() => handleDeleteGoal(goal.id)}
                        className="text-red-500 hover:text-red-700 text-xl"
                        title="Delete goal"
                      >
                        🗑️
                      </button>
                    </div>

                    <div className="space-y-3 mb-4">
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Target:</span>
                        <span className="font-semibold">Rs. {goal.targetAmount.toLocaleString()}</span>
                      </div>
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Saved:</span>
                        <span className="font-semibold text-green-600">Rs. {goal.currentAmount.toLocaleString()}</span>
                      </div>
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Progress:</span>
                        <span className={`font-semibold ${getProgressTextColor(progress)}`}>
                          {progress.toFixed(1)}%
                        </span>
                      </div>
                    </div>

                    {/* Progress Bar */}
                    <div className="w-full bg-gray-200 rounded-full h-3 mb-4">
                      <div
                        className={`h-3 rounded-full transition-all duration-300 ${getProgressColor(progress)}`}
                        style={{ width: `${Math.min(progress, 100)}%` }}
                      ></div>
                    </div>

                    {/* Deadline */}
                    <div className="flex justify-between items-center text-sm mb-4">
                      <span className="text-gray-600">Deadline:</span>
                      <span className={`font-medium ${isNearDeadline ? 'text-red-600' : 'text-gray-800'}`}>
                        {new Date(goal.deadline).toLocaleDateString()}
                        {isNearDeadline && ' ⚠️'}
                      </span>
                    </div>

                    {/* Description */}
                    {goal.description && (
                      <p className="text-sm text-gray-600 mb-4 line-clamp-2" title={goal.description}>
                        {goal.description}
                      </p>
                    )}

                    {/* Actions */}
                    <button
                      onClick={() => openUpdateModal(goal)}
                      className="w-full bg-green-600 text-white py-2 px-4 rounded-lg hover:bg-green-700 transition font-medium"
                    >
                      Update Progress
                    </button>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>

      {/* Add Goal Modal */}
      {showAddModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg shadow-xl max-w-md w-full max-h-[90vh] overflow-y-auto">
            <div className="p-6">
              <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-gray-800">Add New Goal</h2>
                <button
                  onClick={resetAddForm}
                  className="text-gray-400 hover:text-gray-600 text-2xl"
                >
                  ×
                </button>
              </div>

              <form onSubmit={handleAddGoal}>
                {/* Title */}
                <div className="mb-4">
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Goal Title <span className="text-red-500">*</span>
                  </label>
                  <input
                    type="text"
                    value={addFormData.title}
                    onChange={(e) => setAddFormData({ ...addFormData, title: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    placeholder="e.g., Buy a Laptop"
                    required
                  />
                </div>

                {/* Target Amount */}
                <div className="mb-4">
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Target Amount (Rs.) <span className="text-red-500">*</span>
                  </label>
                  <input
                    type="number"
                    step="0.01"
                    min="0"
                    value={addFormData.targetAmount}
                    onChange={(e) => setAddFormData({ ...addFormData, targetAmount: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    placeholder="0.00"
                    required
                  />
                </div>

                {/* Deadline */}
                <div className="mb-4">
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Deadline <span className="text-red-500">*</span>
                  </label>
                  <input
                    type="date"
                    value={addFormData.deadline}
                    onChange={(e) => setAddFormData({ ...addFormData, deadline: e.target.value })}
                    min={new Date().toISOString().split('T')[0]}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    required
                  />
                </div>

                {/* Description */}
                <div className="mb-6">
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Description (Optional)
                  </label>
                  <textarea
                    value={addFormData.description}
                    onChange={(e) => setAddFormData({ ...addFormData, description: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    rows="3"
                    placeholder="Add details about your goal..."
                  />
                </div>

                {/* Buttons */}
                <div className="flex gap-3">
                  <button
                    type="button"
                    onClick={resetAddForm}
                    className="flex-1 px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition"
                  >
                    Cancel
                  </button>
                  <button
                    type="submit"
                    className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
                  >
                    Add Goal
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}

      {/* Update Progress Modal */}
      {showUpdateModal && updatingGoal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg shadow-xl max-w-md w-full">
            <div className="p-6">
              <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-gray-800">Update Progress</h2>
                <button
                  onClick={resetUpdateForm}
                  className="text-gray-400 hover:text-gray-600 text-2xl"
                >
                  ×
                </button>
              </div>

              <div className="mb-6 p-4 bg-gray-50 rounded-lg">
                <h3 className="font-semibold text-gray-800 mb-2">{updatingGoal.title}</h3>
                <div className="text-sm text-gray-600 space-y-1">
                  <p>Current: Rs. {updatingGoal.currentAmount.toLocaleString()}</p>
                  <p>Target: Rs. {updatingGoal.targetAmount.toLocaleString()}</p>
                  <p>Progress: {updatingGoal.progressPercentage.toFixed(1)}%</p>
                </div>
              </div>

              <form onSubmit={handleUpdateProgress}>
                {/* Add Amount */}
                <div className="mb-6">
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Add Amount (Rs.) <span className="text-red-500">*</span>
                  </label>
                  <input
                    type="number"
                    step="0.01"
                    min="0"
                    value={updateFormData.addAmount}
                    onChange={(e) => setUpdateFormData({ ...updateFormData, addAmount: e.target.value })}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                    placeholder="Enter amount to add"
                    required
                  />
                </div>

                {/* Preview */}
                {updateFormData.addAmount && (
                  <div className="mb-6 p-3 bg-green-50 border border-green-200 rounded-lg">
                    <p className="text-sm text-green-800">
                      New total: Rs. {(updatingGoal.currentAmount + parseFloat(updateFormData.addAmount || 0)).toLocaleString()}
                    </p>
                    <p className="text-sm text-green-800">
                      New progress: {(((updatingGoal.currentAmount + parseFloat(updateFormData.addAmount || 0)) / updatingGoal.targetAmount) * 100).toFixed(1)}%
                    </p>
                  </div>
                )}

                {/* Buttons */}
                <div className="flex gap-3">
                  <button
                    type="button"
                    onClick={resetUpdateForm}
                    className="flex-1 px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition"
                  >
                    Cancel
                  </button>
                  <button
                    type="submit"
                    className="flex-1 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
                  >
                    Update Progress
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Goals;
