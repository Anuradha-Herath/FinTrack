import Sidebar from '../components/Sidebar';

const Goals = () => {
  return (
    <div className="flex min-h-screen bg-gray-50">
      <Sidebar />
      <div className="flex-1 ml-64 p-8">
        <h1 className="text-3xl font-bold text-gray-800 mb-4">Goals</h1>
        <p className="text-gray-600">Goals management page coming soon...</p>
      </div>
    </div>
  );
};

export default Goals;
