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
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PrivatechatComponent } from '../privatechat/privatechat.component';

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
  constructor(public chatservice: Chatservices, private modalService: NgbModal) {
    this.chatservice.message$.subscribe((message)=> { this.messages.push(message)});
    this.username = chatservice.myChatName;
  }
  ngOnDestroy(): void {
    this.chatservice.stopChatConnection();
  }

  ngOnInit(): void {
    this.chatservice.createChatConnection();
  }
  backtoHome() {
    this.closeChatEmitter.emit();
  }

  sendMessage($event: string) {
    this.chatservice.sendMessage($event);
  }
  openPrivateChat(toUser: string) {
    const modalRef = this.modalService.open(PrivatechatComponent);
    modalRef.componentInstance.user = toUser;
    }
}
