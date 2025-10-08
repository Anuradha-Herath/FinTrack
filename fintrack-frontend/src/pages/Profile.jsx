import { useState, useEffect } from 'react';
import Sidebar from '../components/Sidebar';
import api from '../services/api';
import toast, { Toaster } from 'react-hot-toast';

const Profile = () => {
  // Default avatar as base64 data URL (a simple gray circle with user icon)
  const defaultAvatar = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTUwIiBoZWlnaHQ9IjE1MCIgdmlld0JveD0iMCAwIDE1MCAxNTAiIGZpbGw9Im5vbmUiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+CjxyZWN0IHdpZHRoPSIxNTAiIGhlaWdodD0iMTUwIiBmaWxsPSIjRTVFN0VCIi8+CjxwYXRoIGQ9Ik03NSA3NUMzOC42NTQ4IDc1IDkgODguNjU0OCA5IDEyNVY5Ljk0MTUzZS0wNkgxNDFWMTI1QzE0MSA4OC42NTQ4IDExMS4zNDUgNzUgNzUgNzVaIiBmaWxsPSIjOUM5Qzk5Ii8+CjxjaXJjbGUgY3g9Ijc1IiBjeT0iNDUiIHI9IjI1IiBmaWxsPSIjOUM5Qzk5Ii8+Cjwvc3ZnPgo=';
  
  const [profile, setProfile] = useState({
    userId: 0,
    name: '',
    email: '',
    phone: '',
    profilePicture: ''
  });
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(false);
  const [originalProfile, setOriginalProfile] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [passwordData, setPasswordData] = useState({
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [selectedFile, setSelectedFile] = useState(null);
  const [preview, setPreview] = useState(defaultAvatar);

  useEffect(() => {
    fetchProfile();
  }, []);

  useEffect(() => {
    console.log('isEditing state changed to:', isEditing);
  }, [isEditing]);

  const fetchProfile = async () => {
    try {
      console.log('Fetching profile from /api/profile...');
      const response = await api.get('/profile');
      console.log('Profile fetched successfully:', response.data);
      setProfile(response.data);
      setOriginalProfile(response.data); // Store original data for cancel
      // If profilePicture exists and is a relative path, make it absolute
      const profilePic = response.data.profilePicture;
      if (profilePic) {
        // If it's already a full URL (http/https), use it as is
        // Otherwise, prepend the API base URL
        const imageUrl = profilePic.startsWith('http') 
          ? profilePic 
          : `http://localhost:5000${profilePic}`;
        console.log('Setting profile picture:', imageUrl);
        setPreview(imageUrl);
      } else {
        console.log('No profile picture, using placeholder');
        setPreview(defaultAvatar);
      }
    } catch (error) {
      console.error('Error fetching profile:', error);
      if (error.response) {
        console.error('Server responded with:', error.response.status, error.response.data);
      }
      // Dummy data if backend not connected
      const dummyData = {
        userId: 1,
        name: 'Anuradha Herath',
        email: 'anuradha@example.com',
        phone: '+94771234567',
        profilePicture: ''
      };
      setProfile(dummyData);
      setOriginalProfile(dummyData);
      setPreview(defaultAvatar);
      toast.error('Failed to fetch profile. Using dummy data.');
    }
  };

  const handleProfileUpdate = async (e) => {
    e.preventDefault();
    
    // Only allow submission when in editing mode
    if (!isEditing) {
      console.log('Not in editing mode, preventing submit');
      return;
    }
    
    // Prevent multiple submissions
    if (isSubmitting || loading) {
      console.log('Already submitting, skipping...');
      return;
    }
    
    setIsSubmitting(true);
    setLoading(true);
    console.log('Updating profile with data:', { name: profile.name, email: profile.email, phone: profile.phone });
    
    try {
      const response = await api.put('/profile/update', {
        name: profile.name,
        email: profile.email,
        phone: profile.phone
      });
      console.log('Profile update response:', response.data);
      toast.success('Profile updated successfully');
      setIsEditing(false);
      setOriginalProfile(profile); // Update original after successful save
    } catch (error) {
      console.error('Error updating profile:', error);
      if (error.response) {
        console.error('Server error:', error.response.data);
        toast.error(error.response.data.message || 'Failed to update profile');
      } else {
        toast.error('Failed to update profile');
      }
    } finally {
      setLoading(false);
      // Add a small delay before allowing next submission
      setTimeout(() => {
        setIsSubmitting(false);
      }, 500);
    }
  };

  const handleCancelEdit = (e) => {
    e.preventDefault();
    e.stopPropagation();
    console.log('Cancel button clicked');
    // Restore original profile data
    if (originalProfile) {
      setProfile(originalProfile);
    }
    setIsEditing(false);
  };

  const handleEditClick = (e) => {
    e.preventDefault();
    e.stopPropagation();
    console.log('Edit Profile button clicked');
    console.log('Current isEditing state:', isEditing);
    setIsEditing(prev => {
      console.log('Setting isEditing from', prev, 'to true');
      return true;
    });
  };

  const handlePasswordChange = async (e) => {
    e.preventDefault();
    if (passwordData.newPassword !== passwordData.confirmPassword) {
      toast.error('New passwords do not match');
      return;
    }
    if (passwordData.newPassword.length < 6) {
      toast.error('Password must be at least 6 characters long');
      return;
    }
    setLoading(true);
    console.log('Changing password...');
    try {
      const response = await api.put('/profile/change-password', {
        oldPassword: passwordData.oldPassword,
        newPassword: passwordData.newPassword
      });
      console.log('Password change response:', response.data);
      toast.success('Password changed successfully');
      setPasswordData({ oldPassword: '', newPassword: '', confirmPassword: '' });
    } catch (error) {
      console.error('Error changing password:', error);
      if (error.response) {
        console.error('Server error:', error.response.data);
        toast.error(error.response.data.message || 'Failed to change password');
      } else {
        toast.error('Failed to change password');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setSelectedFile(file);
      const reader = new FileReader();
      reader.onload = () => setPreview(reader.result);
      reader.readAsDataURL(file);
    }
  };

  const handleUploadPicture = async () => {
    if (!selectedFile) return;
    const formData = new FormData();
    formData.append('file', selectedFile);
    setLoading(true);
    try {
      const response = await api.post('/profile/upload-picture', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      toast.success('Profile picture updated');
      const newProfilePic = response.data.profilePicture;
      setProfile({ ...profile, profilePicture: newProfilePic });
      // Update preview with the full URL
      const imageUrl = newProfilePic.startsWith('http') 
        ? newProfilePic 
        : `http://localhost:5000${newProfilePic}`;
      setPreview(imageUrl);
      setSelectedFile(null);
    } catch (error) {
      console.error('Error uploading picture:', error);
      toast.error('Failed to upload picture');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen bg-gray-50">
      <Sidebar />
      <div className="flex-1 ml-64 p-8">
        <Toaster />
        <h1 className="text-3xl font-bold text-gray-800 mb-8">Profile Management</h1>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          {/* Profile Information Section */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-2xl font-semibold mb-4">Profile Information</h2>

            <div className="flex items-center mb-4">
              <img
                src={preview || defaultAvatar}
                alt="Profile"
                className="w-24 h-24 rounded-full mr-4 object-cover border-2 border-gray-300"
              />
              <div>
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleFileChange}
                  className="mb-2"
                />
                {selectedFile && (
                  <button
                    type="button"
                    onClick={handleUploadPicture}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    disabled={loading}
                  >
                    {loading ? 'Uploading...' : 'Upload Picture'}
                  </button>
                )}
              </div>
            </div>

            <form onSubmit={handleProfileUpdate} onKeyDown={(e) => {
              // Prevent Enter key from submitting when not editing
              if (e.key === 'Enter' && !isEditing) {
                e.preventDefault();
              }
            }}>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2">Name</label>
                <input
                  type="text"
                  value={profile.name}
                  onChange={(e) => setProfile({ ...profile, name: e.target.value })}
                  disabled={!isEditing}
                  className={`w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-gray-900 ${!isEditing ? 'bg-gray-100 cursor-not-allowed' : 'bg-white'}`}
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2">Email</label>
                <input
                  type="email"
                  value={profile.email}
                  onChange={(e) => setProfile({ ...profile, email: e.target.value })}
                  disabled={!isEditing}
                  className={`w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-gray-900 ${!isEditing ? 'bg-gray-100 cursor-not-allowed' : 'bg-white'}`}
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2">Phone</label>
                <input
                  type="tel"
                  value={profile.phone || ''}
                  onChange={(e) => setProfile({ ...profile, phone: e.target.value })}
                  disabled={!isEditing}
                  className={`w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-gray-900 ${!isEditing ? 'bg-gray-100 cursor-not-allowed' : 'bg-white'}`}
                />
              </div>
              <div className="flex space-x-4">
                {!isEditing ? (
                  <button
                    type="button"
                    onClick={handleEditClick}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                  >
                    Edit Profile
                  </button>
                ) : (
                  <>
                    <button
                      type="submit"
                      className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
                      disabled={loading}
                    >
                      {loading ? 'Saving...' : 'Save Changes'}
                    </button>
                    <button
                      type="button"
                      onClick={handleCancelEdit}
                      className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
                    >
                      Cancel
                    </button>
                  </>
                )}
              </div>
            </form>
          </div>

          {/* Change Password Section */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-2xl font-semibold mb-4">Change Password</h2>
            <form onSubmit={handlePasswordChange}>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2">Old Password</label>
                <input
                  type="password"
                  value={passwordData.oldPassword}
                  onChange={(e) => setPasswordData({ ...passwordData, oldPassword: e.target.value })}
                  className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 bg-white text-gray-900"
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2">New Password</label>
                <input
                  type="password"
                  value={passwordData.newPassword}
                  onChange={(e) => setPasswordData({ ...passwordData, newPassword: e.target.value })}
                  className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 bg-white text-gray-900"
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2">Confirm New Password</label>
                <input
                  type="password"
                  value={passwordData.confirmPassword}
                  onChange={(e) => setPasswordData({ ...passwordData, confirmPassword: e.target.value })}
                  className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 bg-white text-gray-900"
                  required
                />
              </div>
              <button
                type="submit"
                className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
                disabled={loading}
              >
                {loading ? 'Changing...' : 'Update Password'}
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
