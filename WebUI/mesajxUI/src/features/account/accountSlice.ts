import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { User } from "../../constants/types/IUser";
import { FieldValues } from "react-hook-form";
import  requests from "../../api/requests";
import { useNavigate } from "react-router-dom";

interface AccountState {
    user: User | null;
}

const initialState: AccountState = {
    user: null,
};

export const loginUser = createAsyncThunk<User, FieldValues>(
    "user/Auth/login",
    async (userData, {rejectWithValue}) => 
        {
            try{
                const user = await requests.Account.login(userData);
                localStorage.setItem("user", JSON.stringify(user));
                return user;
            }
            catch (error: any) {
                return rejectWithValue({error: error.data});
            }

        }
)

export const accountSlice = createSlice({
    name: "account",
    initialState,
    reducers: { 
        logOut: (state) => {
            state.user = null;
            localStorage.removeItem("user");
            const navigate = useNavigate();
            navigate("/login");
            },
        setUser: (state, action) => {
            state.user = action.payload;
        }
    },
    extraReducers: (builder => {
        builder.addCase(loginUser.fulfilled, (state,action) =>{
            state.user = action.payload;
        })
    })

})

export const { logOut, setUser } = accountSlice.actions;
