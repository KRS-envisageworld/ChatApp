import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './chat-input.component.html',
  styleUrl: './chat-input.component.scss',
})
export class ChatInputComponent {
  message = '';
  @Output() messageEmiter = new EventEmitter();
  sendMessage() {
    if(this.message.trim() != ''){
      debugger
      this.messageEmiter.emit(this.message);
      this.message = '';
    }
  }
}
