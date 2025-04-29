  
  export interface SendMessageDto {
    userId: string;
    chatRoomId: string;
    content: string;
    mediaUrl?: string;
  }