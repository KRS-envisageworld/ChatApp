import {
  Component,
  Output,
  EventEmitter,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { Chatservices } from '../../services/chatservices.service';
import { CommonModule } from '@angular/common';
import { ChatInputComponent } from '../chat-input/chat-input.component';
import { MessageViewComponent } from "../message-view/message-view.component";
import { Message } from '../../models/message.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, ChatInputComponent, MessageViewComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit, OnDestroy {
  @Output() closeChatEmitter = new EventEmitter();
  username = '';
  messages:Message[]=[];
  constructor(public chatservice: Chatservices) {
    debugger;
    this.chatservice.message$.subscribe((message)=> { debugger;this.messages.push(message)});
    this.username = chatservice.myChatName;
  }
  ngOnDestroy(): void {
    this.chatservice.stopChatConnection();
  }

  ngOnInit(): void {
    debugger;
    this.chatservice.createChatConnection();
  }
  backtoHome() {
    this.closeChatEmitter.emit();
  }

  sendMessage($event: any) {
    debugger
    this.chatservice.sendMessage($event);
  }
}
