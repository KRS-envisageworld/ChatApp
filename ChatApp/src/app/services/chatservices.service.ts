import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';
import { Message } from '../models/message.model';
import { BehaviorSubject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PrivatechatComponent } from '../component/privatechat/privatechat.component';

@Injectable({
  providedIn: 'root',
})
export class Chatservices {
  myChatName = '';
  onlineUsers: string[] = [];
  messages: Message[] = [];
  privateMessages: Message[] = [];
  privateMessageinitated = false;

  public message$ = new BehaviorSubject<Message>({} as Message);
  private hubconnection?: signalR.HubConnection;
  constructor(private httpClient: HttpClient, private modalService: NgbModal) {
  }
  registerUser(user: User) {
    return this.httpClient.post(
      `${environment.apiUrl}api/chat/register`,
      user,
      { responseType: 'text', headers:{
        "Access-Control-Allow-Origin": "*",
      } },
    );
  }

  async createChatConnection() {
    this.hubconnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}techtalk`, {
        transport: HttpTransportType.WebSockets,
      })
      .withKeepAliveInterval(30000)
      .withStatefulReconnect({ bufferSize: 1000 })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubconnection.start().catch((err) => console.log(err));

    this.hubconnection.on('NewUserJoined', () => {
      this.addConnectionIdToUser();
    });
    this.hubconnection.on('UpdateOnlineUsers', (onlineUsers) => {
      this.onlineUsers = [...onlineUsers];
    });
    this.hubconnection.on('NewMessage', (response: Message) => {
      this.message$.next(response);
    });

    this.hubconnection.on('OpenPriavteChat', (response: Message) => {
      this.privateMessages = [...this.privateMessages, response];
      this.privateMessageinitated = true;
      const modalRef = this.modalService.open(PrivatechatComponent);
      modalRef.componentInstance.user = response.from;

    });

    this.hubconnection.on('NewPrivateMessage', (response: Message) => {
      this.privateMessages = [...this.privateMessages, response];
    });

    this.hubconnection.on('RemovePrivateChat', () => {
      this.privateMessages =[];
      this.privateMessageinitated = false;
      this.modalService.dismissAll();
    });
  }

  stopChatConnection() {
    this.hubconnection?.stop().catch((err) => console.log(err));
  }

  async addConnectionIdToUser() {
    return this.hubconnection
      ?.invoke('AddUserConnectionId', this.myChatName)
      .then(() => {})
      .catch((err) => console.log('Error occured: ', err));
  }
  sendMessage(messageToSend: string) {
    const message:Message ={
      from : this.myChatName,
      message:messageToSend,
      to:""
    } 
    return this.hubconnection?.invoke("RecieveMessage", message).then(()=>{
    })
    .catch((err) => console.log("Error occured: ",err));
  }

  closePriavteChat(otherUser: string) {
    return this.hubconnection
      ?.invoke('RemovePriavteChat', this.myChatName, otherUser)
      .then(() => {
      })
      .catch((err) => console.log('Error occured: ', err));
  }

  sendPrivateMessage(content: string, user: string) {
    const message: Message = {
      from: this.myChatName,
      message: content,
      to: user
    };

    if (!this.privateMessageinitated) {
      this.privateMessageinitated = true;
      this.hubconnection
        ?.invoke('CreatePrivateChat', message)
        .then(() => {
          this.privateMessages = [...this.privateMessages, message];
        })
        .catch((err) => console.log('Error occured: ', err));
    } else {
      this.hubconnection
        ?.invoke('RecivePrivateMessage', message)
        .catch((err) => console.log('Error occured: ', err));
    }
  }
}
