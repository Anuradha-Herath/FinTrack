import PropTypes from 'prop-types';

const BudgetCard = ({ budget, onEdit, onDelete }) => {
  const {
    category,
    limitAmount,
    spentAmount,
    remainingAmount,
    progressPercentage,
    status,
    month,
    year,
  } = budget;

  // Get month name
  const getMonthName = (monthNum) => {
    const months = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
    return months[monthNum - 1] || '';
  };

  // Get progress bar color
  const getProgressColor = () => {
    if (status === 'over-budget') return 'bg-red-500';
    if (status === 'warning') return 'bg-yellow-500';
    return 'bg-green-500';
  };

  // Get status badge color
  const getStatusBadgeColor = () => {
    if (status === 'over-budget') return 'bg-red-100 text-red-800';
    if (status === 'warning') return 'bg-yellow-100 text-yellow-800';
    return 'bg-green-100 text-green-800';
  };

  // Get category icon
  const getCategoryIcon = (cat) => {
    const icons = {
      Food: '🍔',
      Transport: '🚗',
      Entertainment: '🎬',
      Shopping: '🛍️',
      Healthcare: '⚕️',
      Education: '📚',
      Bills: '📄',
      Rent: '🏠',
      Insurance: '🛡️',
      Investment: '📈',
      Salary: '💰',
      Other: '📦',
    };
    return icons[cat] || '📦';
  };

  return (
    <div className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow p-6 border border-gray-200">
      {/* Header */}
      <div className="flex justify-between items-start mb-4">
        <div className="flex items-center gap-3">
          <span className="text-3xl">{getCategoryIcon(category)}</span>
          <div>
            <h3 className="text-lg font-semibold text-gray-800">{category}</h3>
            <p className="text-sm text-gray-500">
              {getMonthName(month)} {year}
            </p>
          </div>
        </div>
        
        {/* Status Badge */}
        <span className={`px-3 py-1 rounded-full text-xs font-medium ${getStatusBadgeColor()}`}>
          {status === 'over-budget' ? 'Over Budget' : status === 'warning' ? 'Warning' : 'On Track'}
        </span>
      </div>

      {/* Budget Info */}
      <div className="space-y-3 mb-4">
        <div className="flex justify-between text-sm">
          <span className="text-gray-600">Budget Limit:</span>
          <span className="font-semibold text-gray-800">₨{limitAmount.toFixed(2)}</span>
        </div>
        <div className="flex justify-between text-sm">
          <span className="text-gray-600">Spent:</span>
          <span className={`font-semibold ${spentAmount > limitAmount ? 'text-red-600' : 'text-gray-800'}`}>
            ₨{spentAmount.toFixed(2)}
          </span>
        </div>
        <div className="flex justify-between text-sm">
          <span className="text-gray-600">Remaining:</span>
          <span className={`font-semibold ${remainingAmount < 0 ? 'text-red-600' : 'text-green-600'}`}>
            ₨{remainingAmount.toFixed(2)}
          </span>
        </div>
      </div>

      {/* Progress Bar */}
      <div className="mb-4">
        <div className="flex justify-between items-center mb-2">
          <span className="text-xs text-gray-600">Progress</span>
          <span className={`text-xs font-semibold ${progressPercentage > 100 ? 'text-red-600' : 'text-gray-800'}`}>
            {progressPercentage.toFixed(1)}%
          </span>
        </div>
        <div className="w-full bg-gray-200 rounded-full h-3 overflow-hidden">
          <div
            className={`h-full rounded-full transition-all duration-300 ${getProgressColor()}`}
            style={{ width: `${Math.min(progressPercentage, 100)}%` }}
          ></div>
        </div>
        {progressPercentage > 100 && (
          <p className="text-xs text-red-600 mt-1">
            ⚠️ Exceeded budget by ₨{(spentAmount - limitAmount).toFixed(2)}
          </p>
        )}
      </div>

      {/* Action Buttons */}
      <div className="flex gap-2 pt-4 border-t border-gray-200">
        <button
          onClick={() => onEdit(budget)}
          className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors text-sm font-medium flex items-center justify-center gap-2"
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
          </svg>
          Edit
        </button>
        <button
          onClick={() => onDelete(budget)}
          className="flex-1 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors text-sm font-medium flex items-center justify-center gap-2"
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
          </svg>
          Delete
        </button>
      </div>
    </div>
  );
};

BudgetCard.propTypes = {
  budget: PropTypes.shape({
    id: PropTypes.number.isRequired,
    category: PropTypes.string.isRequired,
    limitAmount: PropTypes.number.isRequired,
    spentAmount: PropTypes.number.isRequired,
    remainingAmount: PropTypes.number.isRequired,
    progressPercentage: PropTypes.number.isRequired,
    status: PropTypes.string.isRequired,
    month: PropTypes.number.isRequired,
    year: PropTypes.number.isRequired,
  }).isRequired,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default BudgetCard;
