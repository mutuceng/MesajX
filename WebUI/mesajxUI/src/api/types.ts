// src/api/types.ts
export interface Message {
    messageId: string;
    userId: string;
    chatRoomId: string;
    content: string;
    mediaUrl?: string;
    sentAt: string; 
    isRead: number;
  }
  
export interface ChatRoom {
  chatRoomId: string;
  name: string;

}

export interface LoginResponse {
  access_token: string;
  expires_in: number;
}

export enum Role {
  Moderator,
  Member
}