import { FormEvent, useState, useRef, ChangeEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { User, Eye, EyeOff, Loader2, Lock, Mail, MessageSquare, Upload, X, Phone } from 'lucide-react';
import { useDispatch } from 'react-redux';
import LogRegImage from '../components/LogRegImage';
import { setUser } from '../features/account/accountSlice';
import { AppDispatch } from '../store/store';
import { createAsyncThunk } from '@reduxjs/toolkit';
import requests from '../api/requests';

// Create a register user thunk that matches the controller requirements
export const registerUser = createAsyncThunk(
  'user/register',
  async (data: { userDto: FormData, profileImage: File }, { rejectWithValue }) => {
    try {
      const formData = new FormData();
      
      // Append all UserRegisterDto fields
      for (const [key, value] of data.userDto.entries()) {
        formData.append(key, value);
      }
      
      // Append profile image separately as expected by the controller
      if (data.profileImage) {
        formData.append('profileImage', data.profileImage);
      }
      
      const response = await requests.Account.register(formData);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || 'Registration failed');
    }
  }
);

function SignUpPage() {
  const navigate = useNavigate();
  const dispatch = useDispatch<AppDispatch>();
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [formData, setFormData] = useState<{
    Username: string;
    Email: string;
    BirthDate: string;
    Password: string;
    PhoneNumber: string;
  }>({
    Username: '',
    Email: '',
    BirthDate: '',
    Password: '',
    PhoneNumber: '',
  });
  const [profileImage, setProfileImage] = useState<File | null>(null);
  const [isSigningUp, setIsSigningUp] = useState<boolean>(false);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const validateForm = (): boolean => {
    if (!formData.Username.trim()) {
      toast.error('Username is required');
      return false;
    }
    if (!formData.Email.trim()) {
      toast.error('Email is required');
      return false;
    }
    if (!/\S+@\S+\.\S+/.test(formData.Email)) {
      toast.error('Invalid email format');
      return false;
    }
    if (!formData.Password) {
      toast.error('Password is required');
      return false;
    }
    if (formData.Password.length < 6) {
      toast.error('Password must be at least 6 characters');
      return false;
    }
    if (!formData.BirthDate) {
      toast.error('Birth date is required');
      return false;
    }
    if (!profileImage) {
      toast.error('Profile photo is required');
      return false;
    }

    const birthDate = new Date(formData.BirthDate);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    
    if (age < 18) {
      toast.error('You must be at least 18 years old to register');
      return false;
    }

    return true;
  };

  const handlePhotoChange = (e: ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    
    if (file) {
      // Check if the file is an image
      if (!file.type.startsWith('image/')) {
        toast.error('Please select an image file');
        return;
      }
      
      // Check file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        toast.error('Image size should be less than 5MB');
        return;
      }

      setProfileImage(file);
      
      // Create preview URL
      const reader = new FileReader();
      reader.onload = () => {
        setPreviewUrl(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const removePhoto = () => {
    setProfileImage(null);
    setPreviewUrl(null);
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const isValid = validateForm();
    if (!isValid) return;

    setIsSigningUp(true);

    try {
      // Create FormData for UserRegisterDto
      const userDtoFormData = new FormData();
      userDtoFormData.append('Username', formData.Username);
      userDtoFormData.append('Email', formData.Email);
      userDtoFormData.append('Password', formData.Password);
      userDtoFormData.append('BirthDate', new Date(formData.BirthDate).toISOString());
      
      if (formData.PhoneNumber) {
        userDtoFormData.append('PhoneNumber', formData.PhoneNumber);
      }
      
      // Dispatch with both userDto and profileImage as expected by the controller
      const resultAction = await dispatch(
        registerUser({
          userDto: userDtoFormData,
          profileImage: profileImage!
        })
      );
      
      if (registerUser.fulfilled.match(resultAction)) {
        toast.success('Registration successful!');
        
        // If registration returns user data, set it in Redux state
        if (resultAction.payload && typeof resultAction.payload !== 'string') {
          dispatch(setUser(resultAction.payload));
          localStorage.setItem('user', JSON.stringify(resultAction.payload));
        }
        
        // Reset form
        setFormData({
          Username: '',
          Email: '',
          Password: '',
          BirthDate: '',
          PhoneNumber: '',
        });
        setProfileImage(null);
        setPreviewUrl(null);
        
        // Navigate to login page
        navigate('/login');
      } else {
        // Handle error case
        const errorPayload = resultAction.payload;
        if (Array.isArray(errorPayload)) {
          // Handle array of error messages from backend
          errorPayload.forEach(error => toast.error(error));
        } else {
          const errorMessage = typeof errorPayload === 'string' 
            ? errorPayload 
            : 'Registration failed';
          toast.error(errorMessage);
        }
      }
    } catch (error) {
      console.error('Registration error:', error);
      toast.error('Server error occurred.');
    } finally {
      setIsSigningUp(false);
    }
  };

  return (
    <div className="min-h-screen grid lg:grid-cols-2">
      {/* Left side */}
      <div className="flex flex-col justify-center items-center p-6 sm:p-12">
        <div className="w-full max-w-md space-y-8">
          {/* LOGO */}
          <div className="text-center mb-8">
            <div className="flex flex-col items-center gap-2 group">
              <div
                className="size-12 rounded-xl bg-primary/10 flex items-center justify-center 
                group-hover:bg-primary/20 transition-colors"
              >
                <MessageSquare className="size-6 text-primary" />
              </div>
              <h1 className="text-2xl font-bold mt-2">Create Account</h1>
              <p className="text-base-content/60">Get started with your free account</p>
            </div>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Profile Photo Upload */}
            <div className="form-control">
              <label className="label">
                <span className="label-text font-medium">Profile Photo</span>
              </label>
              <div className="flex flex-col items-center gap-4">
                {previewUrl ? (
                  <div className="relative">
                    <img 
                      src={previewUrl} 
                      alt="Profile Preview" 
                      className="w-24 h-24 rounded-full object-cover border-2 border-primary"
                    />
                    <button 
                      type="button"
                      onClick={removePhoto}
                      className="absolute -top-2 -right-2 bg-error text-white rounded-full p-1"
                    >
                      <X className="size-4" />
                    </button>
                  </div>
                ) : (
                  <div 
                    className="w-24 h-24 rounded-full bg-base-200 flex items-center justify-center cursor-pointer"
                    onClick={() => fileInputRef.current?.click()}
                  >
                    <Upload className="size-8 text-base-content/40" />
                  </div>
                )}
                
                <input
                  type="file"
                  ref={fileInputRef}
                  className="hidden"
                  accept="image/*"
                  onChange={handlePhotoChange}
                />
                
                <button
                  type="button"
                  onClick={() => fileInputRef.current?.click()}
                  className="btn btn-outline btn-sm"
                >
                  {previewUrl ? 'Change Photo' : 'Upload Photo'}
                </button>
              </div>
            </div>

            <div className="form-control">
              <label className="label">
                <span className="label-text font-medium">Username</span>
              </label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <User className="size-5 text-base-content/40" />
                </div>
                <input
                  type="text"
                  className="input input-bordered w-full pl-10"
                  placeholder="johndoe"
                  value={formData.Username}
                  onChange={(e) =>
                    setFormData({ ...formData, Username: e.target.value })
                  }
                />
              </div>
            </div>

            <div className="form-control">
              <label className="label">
                <span className="label-text font-medium">Email</span>
              </label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Mail className="size-5 text-base-content/40" />
                </div>
                <input
                  type="email"
                  className="input input-bordered w-full pl-10"
                  placeholder="example@email.com"
                  value={formData.Email}
                  onChange={(e) =>
                    setFormData({ ...formData, Email: e.target.value })
                  }
                />
              </div>
            </div>

            <div className="form-control">
              <label className="label">
                <span className="label-text font-medium">Phone Number</span>
                <span className="label-text-alt">(Optional)</span>
              </label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Phone className="size-5 text-base-content/40" />
                </div>
                <input
                  type="tel"
                  className="input input-bordered w-full pl-10"
                  placeholder="+1234567890"
                  value={formData.PhoneNumber}
                  onChange={(e) =>
                    setFormData({ ...formData, PhoneNumber: e.target.value })
                  }
                />
              </div>
            </div>

            <div className="form-control">
              <label className="label">
                <span className="label-text font-medium">Birth Date</span>
              </label>
              <input
                type="date"
                className="input input-bordered w-full"
                value={formData.BirthDate}
                onChange={(e) =>
                  setFormData({ ...formData, BirthDate: e.target.value })
                }
                max={new Date(new Date().setFullYear(new Date().getFullYear() - 18)).toISOString().split('T')[0]}
              />
              <label className="label">
                <span className="label-text-alt">You must be at least 18 years old</span>
              </label>
            </div>

            <div className="form-control">
              <label className="label">
                <span className="label-text font-medium">Password</span>
              </label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Lock className="size-5 text-base-content/40" />
                </div>
                <input
                  type={showPassword ? 'text' : 'password'}
                  className="input input-bordered w-full pl-10"
                  placeholder="••••••••"
                  value={formData.Password}
                  onChange={(e) =>
                    setFormData({ ...formData, Password: e.target.value })
                  }
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute inset-y-0 right-0 pr-3 flex items-center"
                >
                  {showPassword ? <EyeOff className="size-5" /> : <Eye className="size-5" />}
                </button>
              </div>
            </div>

            <button
              type="submit"
              className="btn btn-primary w-full"
              disabled={isSigningUp}
            >
              {isSigningUp ? (
                <>
                  <Loader2 className="size-5 animate-spin" />
                  Loading...
                </>
              ) : (
                'Create Account'
              )}
            </button>
          </form>

          <div className="text-center">
            <p className="text-base-content/60">
              Already have an account?{' '}
              <Link to="/login" className="link link-primary">
                Sign in
              </Link>
            </p>
          </div>
        </div>
      </div>

      <LogRegImage
        title="Join our community"
        subtitle="Connect with friends, share moments, and stay in touch with your loved ones."
      />
    </div>
  );
}

export default SignUpPage;