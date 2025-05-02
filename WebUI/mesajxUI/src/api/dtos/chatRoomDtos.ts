export interface CreateChatRoomDto {
    userId: string;
    chatRoomId: string;
    name?: string;
    photo?: string;
    isGroup: boolean;
    createdAt: string; 
  }
