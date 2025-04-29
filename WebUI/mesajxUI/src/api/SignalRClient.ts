// src/api/SignalRClient.ts
import * as signalR from '@microsoft/signalr';
import { Message } from './types';

export class SignalRClient {
  private connection: signalR.HubConnection;
  private onMessageReceived: (message: Message) => void;

  constructor(hubUrl: string, getToken: () => string | null, onMessageReceived: (message: Message) => void) {
    this.onMessageReceived = onMessageReceived;
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => getToken() || '',
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.on('ReceiveMessage', (message: Message) => {
      this.onMessageReceived(message);
    });
  }

  async start(): Promise<void> {
    try {
      await this.connection.start();
      console.log('SignalR connected');
    } catch (err) {
      console.error('SignalR connection error:', err);
      throw err;
    }
  }

  async joinGroupChat(chatId: string): Promise<void> {
    await this.connection.invoke('JoinGroupChat', chatId);
  }

  async leaveGroupChat(roomId: string): Promise<void> {
    await this.connection.invoke('LeaveGroupChat', roomId);
  }

  async sendMessage(userId: string, chatRoomId: string, content: string, mediaUrl?: string): Promise<void> {
    await this.connection.invoke('SendMessageAsync', userId, chatRoomId, content, mediaUrl);
  }

  async stop(): Promise<void> {
    await this.connection.stop();
  }
}