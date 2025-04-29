import { createSlice } from "@reduxjs/toolkit";
import { Message } from "../../api/types";

interface MessageState {
    messages: Message[] | null;
}

const initialState: MessageState = {
    messages: null,
};

export const messageSlice = createSlice({
    name: "message",
    initialState,
    reducers: {}
});