import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../../models/message.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-message-view',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './message-view.component.html',
  styleUrl: './message-view.component.scss',
})
export class MessageViewComponent implements OnInit {
  @Input() messages:Message[] = [];
 
  constructor() {
  }
  ngOnInit(): void {}
}
