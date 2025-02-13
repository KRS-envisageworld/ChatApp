import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { environment } from "../../environment/environment";
import * as signalR from '@microsoft/signalr';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';
import { Message } from '../models/message.model';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Chatservices {
  
  myChatName = '';
  onlineUsers: string[] = [];
  messages:Message[] = [];
  public message$ = new BehaviorSubject<Message>({} as Message);
  private hubconnection?: signalR.HubConnection;
  constructor(private httpClient: HttpClient) {
    // this.message$ = this.message
  }
  registerUser(user: User) {
    return this.httpClient.post(
      `${environment.apiUrl}api/chat/register`,
      user,
      { responseType: 'text' }
    );
  }

  async createChatConnection() {
    this.hubconnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}techtalk`, {transport: HttpTransportType.WebSockets})
      .withKeepAliveInterval(30000)
      .withStatefulReconnect({ bufferSize: 1000 })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubconnection.start().catch((err) => console.log(err));

    this.hubconnection.on('NewUserJoined', (data) => {
      this.addConnectionIdToUser();
    });
    this.hubconnection.on('UpdateOnlineUsers', (onlineUsers) => {
      this.onlineUsers = [...onlineUsers]
    });
    this.hubconnection.on('NewMessage', (response:Message) => {
      debugger
      this.message$.next(response);
    });
  }

  stopChatConnection() {
    debugger
    this.hubconnection?.stop().catch((err) => console.log(err));
  }

  async addConnectionIdToUser() {
    debugger
    return this.hubconnection?.invoke("AddUserConnectionId", this.myChatName).then(()=>{
        debugger
      })
      .catch((err) => console.log("Error occured: ",err));
  }

  sendMessage(messageToSend: string) {
    let message:Message ={
      from : this.myChatName,
      message:messageToSend,
      to:""
    } 
    return this.hubconnection?.invoke("RecieveMessage", message).then(()=>{
    })
    .catch((err) => console.log("Error occured: ",err));
  }
}