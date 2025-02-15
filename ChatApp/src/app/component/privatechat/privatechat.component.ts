import { Component, Input, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Chatservices } from '../../services/chatservices.service';
import { ChatInputComponent } from '../chat-input/chat-input.component';
import { MessageViewComponent } from '../message-view/message-view.component';

@Component({
  selector: 'app-privatechat',
  standalone: true,
  imports: [ChatInputComponent, MessageViewComponent],
  templateUrl: './privatechat.component.html',
  styleUrl: './privatechat.component.scss',
})
export class PrivatechatComponent implements OnDestroy {
  @Input() user = '';
  constructor(
    public activeModal: NgbActiveModal,
    public chatService: Chatservices
  ) {}
  ngOnDestroy(): void {
    this.chatService.closePriavteChat(this.user);
  }

  sendMessage(content: string) {
    this.chatService.sendPrivateMessage(content, this.user);
  }
}
